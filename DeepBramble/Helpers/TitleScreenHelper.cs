using NewHorizons.Utility;
using System;
using UnityEngine.Events;

namespace DeepBramble.Helpers
{
    public static class TitleScreenHelper
    {
        /**
         * Do NH setup stuff
         */
        internal static void Initialize()
        {
            //Do stuff when a title screen loads
            UnityEvent<String, int> titleScreenLoadedEvent = DeepBramble.instance.NewHorizonsAPI.GetTitleScreenLoadedEvent();
            titleScreenLoadedEvent.AddListener(OnTitleScreenLoadedEvent);
        }

        /**
         * Do some manual changes when our title screen is loaded
         */
        private static void OnTitleScreenLoadedEvent(string modUniqueName, int index)
        {
            if (modUniqueName == DeepBramble.instance.ModHelper.Manifest.UniqueName)
            {
                DeepBramble.debugPrint("First time title edits");

                //Change the campfire appearance
                CampFireHelper.ChangeFireAppearance(SearchUtilities.Find("Scene/Background/PlanetPivot/PlanetRoot/Prefab_HEA_Campfire/Controller_Campfire").GetComponent<Campfire>());

                DeepBramble.debugPrint("Title edits complete");
            }
        }
    }
}
