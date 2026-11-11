using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace FollowZoom
{
    // Follow Zoom: lets the mouse wheel and zoom keys zoom while the camera follows a vehicle.
    // Why two patches are needed (see docs/HOW_IT_WORKS.md):
    //  1. CinematicCameraFollow.Update stops the follow on any mouse wheel input.
    //  2. It also forces GCamera.zoomLog to zoom level 1 every frame, and SMouse skips its own
    //     zoom animation while a cinematic overrides the camera.
    // SMouse still updates GCamera.zoomLevelTarget from the wheel and keys, so we only have to
    // stop the follow from ending and move zoomLog towards that target ourselves.
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "highway1027.planb.followzoom";
        public const string PluginName = "Follow Zoom";
        public const string PluginVersion = "0.1.0";

        internal static ManualLogSource Log;
        internal static bool Enabled = true;
        private Harmony _harmony;

        private void Awake()
        {
            Log = Logger;
            try
            {
                _harmony = new Harmony(PluginGuid);
                _harmony.PatchAll();
                Log.LogInfo($"{PluginName} {PluginVersion} loaded");
            }
            catch (Exception e)
            {
                Enabled = false;
                Log.LogError($"{PluginName} could not patch the game, mod is off: {e}");
            }
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        // Called on any error: log once and fall back to vanilla behaviour.
        internal static void Disable(Exception e)
        {
            if (!Enabled) return;
            Enabled = false;
            Log.LogError($"{PluginName} hit an error and is now off (vanilla follow camera): {e}");
        }
    }

    [HarmonyPatch(typeof(CinematicCameraFollow), nameof(CinematicCameraFollow.Update))]
    internal static class FollowUpdatePatch
    {
        // Same speed and fade as SMouse uses for normal zooming, so it feels the same.
        private const float ZoomSpeed = 4f;
        private const float ZoomFade = 0.5f;

        private static float _zoomLog;
        private static float _lastT = -1f;

        // True when the follow uses the normal fixed zoom (vehicles, drones, watched buildings).
        // Animals, satellites, space lifts and rockets set their own zoom: those stay vanilla.
        internal static bool IsNormalFollow(CinematicCameraFollow follow)
        {
            if (follow.animalFollowing != null || follow.satelliteFollowing != null) return false;
            CItem_Content content = SSingleton<SWorld>.Inst.GetContent(follow.coordsWatching);
            return !(content is CItem_ContentSpaceLift) && !(content is CItem_ContentRocket);
        }

        // Patch 1 (transpiler): in Update, the check "mouseScrollDelta != zero -> stop following"
        // reads Input.mouseScrollDelta. We swap that one read for ScrollDeltaForStopCheck, which
        // reports no scroll during a normal follow. Right and middle mouse still stop the follow.
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo getScroll = AccessTools.PropertyGetter(typeof(Input), nameof(Input.mouseScrollDelta));
            MethodInfo replacement = AccessTools.Method(typeof(FollowUpdatePatch), nameof(ScrollDeltaForStopCheck));
            int replaced = 0;
            foreach (CodeInstruction ins in instructions)
            {
                if (ins.Calls(getScroll))
                {
                    // ldarg.0 (this) + call replacement(this) leaves a Vector2 on the stack, like the original.
                    CodeInstruction loadThis = new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(ins);
                    yield return loadThis;
                    yield return new CodeInstruction(OpCodes.Call, replacement);
                    replaced++;
                    continue;
                }
                yield return ins;
            }
            if (replaced != 1)
                Plugin.Log.LogWarning($"Expected 1 mouse wheel check in CinematicCameraFollow.Update, found {replaced}. The game may have changed.");
        }

        private static Vector2 ScrollDeltaForStopCheck(CinematicCameraFollow follow)
        {
            Vector2 real = Input.mouseScrollDelta;
            if (!Plugin.Enabled) return real;
            try
            {
                return IsNormalFollow(follow) ? Vector2.zero : real;
            }
            catch (Exception e)
            {
                Plugin.Disable(e);
                return real;
            }
        }

        // Patch 2 (postfix): after the game has set zoomLog to level 1, overwrite it with our own
        // smoothed value that moves towards the zoom level the player picked (GCamera.zoomLevelTarget).
        private static void Postfix(CinematicCameraFollow __instance)
        {
            if (!Plugin.Enabled) return;
            try
            {
                if (!__instance.IsPlaying() || !IsNormalFollow(__instance))
                {
                    _lastT = -1f;
                    return;
                }
                // A new follow starts with t near 0: start from the zoom the game set this frame,
                // which is still the zoom from before the follow.
                if (_lastT < 0f || __instance.t < _lastT)
                    _zoomLog = GCamera.zoomLog;
                _lastT = __instance.t;

                int level = Mathf.Clamp(GCamera.zoomLevelTarget, 0, GCamera.zoomLogs.Length - 1);
                _zoomLog = SMisc.SmoothDamp_realtime(_zoomLog, GCamera.zoomLogs[level], ZoomSpeed, ZoomFade);
                GCamera.zoomLog = _zoomLog;
            }
            catch (Exception e)
            {
                Plugin.Disable(e);
            }
        }
    }
}
