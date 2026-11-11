using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace FollowZoom
{
    // A "Keep zoom after follow" checkbox in the game's own Options menu (General tab).
    // It is a normal game COptionBool, but it reads and saves the BepInEx config entry instead of
    // PlayerPrefs, so the .cfg file stays the one place the setting lives.
    // The game's Confirm button saves it, Cancel reloads it, Default resets it, like any option.
    internal class KeepZoomOption : COptionBool
    {
        internal const string Label = "Keep zoom after follow (Follow Zoom)";
        internal const string Tooltip =
            "On: when you stop following a vehicle, or hop to the next one, the camera keeps the zoom " +
            "you picked during the follow. Off: it goes back to the zoom from before the follow.";

        internal KeepZoomOption()
        {
            defaultValue = true;
        }

        protected override bool GetSavedValue()
        {
            return Plugin.KeepZoomAfterFollow.Value;
        }

        protected override void DoSave()
        {
            Plugin.KeepZoomAfterFollow.Value = value; // BepInEx writes the .cfg file on set.
        }

        public override void Reset()
        {
            value = defaultValue;
            DoSave();
        }

        public override void InstantiateUI(Transform uiOptionsContainer, bool isInLauncher = false)
        {
            base.InstantiateUI(uiOptionsContainer, isInLauncher);
            // The game takes the label from its text table (SLoc), which has no entry for us and
            // shows "< OPTION_... >". The row we just made is the container's last child: set the
            // label and tooltip ourselves. English only for now.
            Transform row = uiOptionsContainer.GetChild(uiOptionsContainer.childCount - 1);
            Text label = row.Find("Label")?.GetComponent<Text>();
            if (label != null) label.text = Label;
            CTooltipTarget tip = row.GetComponent<CTooltipTarget>() ?? row.gameObject.AddComponent<CTooltipTarget>();
            tip.text = Tooltip;
        }
    }

    // Patch 5 (prefix on the private SSceneOptions.Refresh): the Options menu builds its rows fresh
    // from GOptions.listOptions[tab] each time it opens or changes tab. Just before that, we make sure
    // our checkbox is at the end of the General tab (tab 0). If this fails, the checkbox is missing
    // but the setting still works from the .cfg file.
    [HarmonyPatch(typeof(SSceneOptions), "Refresh")]
    internal static class OptionsMenuPatch
    {
        internal static KeepZoomOption Option;
        private static bool _failed;

        private static void Prefix()
        {
            if (_failed) return;
            try
            {
                if (Option == null)
                {
                    Option = new KeepZoomOption();
                    Option.Init("followZoom_keepZoomAfterFollow");
                }
                COption[] general = GOptions.listOptions[0];
                if (general != null && !general.Contains(Option))
                    GOptions.listOptions[0] = general.Concat(new COption[] { Option }).ToArray();
            }
            catch (Exception e)
            {
                _failed = true;
                Plugin.Log.LogError($"Could not add the Follow Zoom checkbox to the Options menu (setting still works in the .cfg file): {e}");
            }
        }
    }
}
