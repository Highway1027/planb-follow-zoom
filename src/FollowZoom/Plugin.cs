using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Configuration;
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
        public const string PluginVersion = "0.2.0";

        internal static ManualLogSource Log;
        internal static bool Enabled = true;
        // BepInEx\config\highway1027.planb.followzoom.cfg. A plain static setting, so a future
        // Lua layer or Options menu entry can drive it the same way.
        internal static ConfigEntry<bool> KeepZoomAfterFollow;
        private Harmony _harmony;

        private void Awake()
        {
            Log = Logger;
            try
            {
                KeepZoomAfterFollow = Config.Bind("General", "KeepZoomAfterFollow", true,
                    "true: when a follow ends, or you hop to the next vehicle, the camera keeps the zoom " +
                    "you picked during the follow. false: it goes back to the zoom from before the follow " +
                    "(vanilla). Only applies once you zoomed during the follow.");
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
                    FollowState.InNormalFollow = false;
                    return;
                }
                FollowState.InNormalFollow = true;
                // The wheel and keys change zoomLevelTarget (via SMouse): that means the player zoomed.
                if (GCamera.zoomLevelTarget != FollowState.TargetSetByGame)
                    FollowState.PlayerZoomed = true;
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

    // What we know about the current follow, shared by the patches.
    internal static class FollowState
    {
        // Set by the Update postfix: the current follow is a vehicle, drone or building follow.
        internal static bool InNormalFollow;
        // The player changed the zoom during this follow (see FollowUpdatePatch.Postfix).
        internal static bool PlayerZoomed;
        // The zoom level Play() set (1). Any other value during the follow came from the player.
        internal static int TargetSetByGame = 1;

        internal static bool KeepZoom => Plugin.Enabled && Plugin.KeepZoomAfterFollow.Value;
    }

    // Patch 3 (prefix + postfix on Play): clicking the follow button again while following hops to
    // the next vehicle by calling Play() again. Vanilla then saves the current (player's) zoom as
    // "the zoom from before the follow" and resets the zoom to level 1. With KeepZoomAfterFollow we
    // keep the zoom the player picked and the real zoom from before the first follow.
    [HarmonyPatch(typeof(CinematicCameraFollow), nameof(CinematicCameraFollow.Play))]
    internal static class FollowPlayPatch
    {
        private static readonly AccessTools.FieldRef<CinematicCameraFollow, int> InitialZoomLevel =
            AccessTools.FieldRefAccess<CinematicCameraFollow, int>("_initialZoomLevel");

        private struct Hop
        {
            public bool IsHop;
            public bool PlayerZoomed;
            public int PlayerLevel;
            public int InitialLevel;
        }

        private static void Prefix(CinematicCameraFollow __instance, out Hop __state)
        {
            __state = default;
            if (!FollowState.KeepZoom) return;
            try
            {
                if (__instance.IsPlaying() && FollowState.InNormalFollow)
                {
                    __state.IsHop = true;
                    __state.PlayerZoomed = FollowState.PlayerZoomed;
                    __state.PlayerLevel = GCamera.zoomLevelTarget;
                    __state.InitialLevel = InitialZoomLevel(__instance);
                }
            }
            catch (Exception e)
            {
                Plugin.Disable(e);
            }
        }

        private static void Postfix(CinematicCameraFollow __instance, Hop __state)
        {
            try
            {
                if (__state.IsHop && Plugin.Enabled && FollowUpdatePatch.IsNormalFollow(__instance))
                {
                    // Always keep the zoom from before the first follow (vanilla would save level 1).
                    InitialZoomLevel(__instance) = __state.InitialLevel;
                    if (__state.PlayerZoomed)
                    {
                        GCamera.zoomLevelTarget = __state.PlayerLevel;
                        FollowState.TargetSetByGame = __state.PlayerLevel;
                        FollowState.PlayerZoomed = true;
                        return;
                    }
                }
                // A fresh follow: Play() just set the zoom target to 1.
                FollowState.TargetSetByGame = GCamera.zoomLevelTarget;
                FollowState.PlayerZoomed = false;
            }
            catch (Exception e)
            {
                Plugin.Disable(e);
            }
        }
    }

    // Patch 4 (prefix + postfix on Stop): vanilla Stop() puts back the zoom level from before the
    // follow. With KeepZoomAfterFollow, and once the player zoomed during a normal follow, we put
    // the player's zoom level back after Stop() ran. SMouse then keeps the camera at that level.
    [HarmonyPatch(typeof(CinematicCameraFollow), nameof(CinematicCameraFollow.Stop))]
    internal static class FollowStopPatch
    {
        private static void Prefix(out int __state)
        {
            __state = -1;
            if (!FollowState.KeepZoom) return;
            if (FollowState.InNormalFollow && FollowState.PlayerZoomed)
                __state = GCamera.zoomLevelTarget;
        }

        private static void Postfix(int __state)
        {
            FollowState.InNormalFollow = false;
            FollowState.PlayerZoomed = false;
            if (__state >= 0 && Plugin.Enabled)
                GCamera.zoomLevelTarget = __state;
        }
    }
}
