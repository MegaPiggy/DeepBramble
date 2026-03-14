using DeepBramble.Triggers;
using NewHorizons.Components.EyeOfTheUniverse;
using NewHorizons.Handlers;
using UnityEngine;

namespace DeepBramble.Helpers
{
    public static class EyeSystemHelper
    {
        public static bool doEyeStuff = false;
        public static OWAudioSource ditySource = null;
        public static bool dityFadeStarted = false;

        /**
         * Does the fixes for the eye system
         */
        public static void FixEyeSystem()
        {
            //Find the campsite root transform
            Transform campRoot = Component.FindObjectOfType<QuantumCampsiteController>().transform;

            //If Ditylum isn't here, delete our additions and exit early
            if(!PlayerData.GetPersistentCondition("MET_DITYLUM"))
            {
                DeepBramble.debugPrint("Ditylum isn't coming");
                doEyeStuff = false;
                return;
            }

            doEyeStuff = true;

            //Disable the original ground
            campRoot.Find("Terrain_Campfire/Terrain_EYE_ForestFloor_Tomb/ForestOfGalaxies_Center_new").gameObject.SetActive(false);

            //Do stuff for the zone
            FixZone(campRoot);

            //Do stuff to the inflation controller
            FixInflationController(campRoot);
        }

        /**
         * Fixes things relating to ditylum's zone
         * 
         * @param campRoot the root of the campsite
         */
        private static void FixZone(Transform campRoot)
        {
            Transform ditylumZone = campRoot.Find("InstrumentZones/DitylumZone");

            //Give the quantum instrument the things to enable
            Transform poem = ditylumZone.Find("poem");
            QuantumInstrument instrument = poem.GetComponent<QuantumInstrument>();
            ArrayHelpers.Append(ref instrument._activateObjects, campRoot.Find("Terrain_Campfire/Terrain_EYE_ForestFloor_Tomb/forest_new_ground/actual_ground/ditylum_patch").gameObject);

            //Add the zone trigger to the necessary component
            DitylumZoneTrigger zoneTrigger = ditylumZone.Find("warp_override_trigger").gameObject.AddComponent<DitylumZoneTrigger>();
            zoneTrigger.warpCylinder = campRoot.Find("Volumes_Campfire/EndlessCylinder_Forest").GetComponent<EndlessCylinder>();

            //Set up the gather logic
            instrument.OnFinishGather += OnFinishGather;

            //Set up the signal
            AudioSignal signal = ditylumZone.GetComponentInChildren<AudioSignal>();
            signal.SetSector(campRoot.GetComponent<Sector>());
            SignalSwitchTrigger switchTrigger = ditylumZone.Find("signal_move_trigger").gameObject.AddComponent<SignalSwitchTrigger>();
            switchTrigger.signalTransform = signal.transform;
            switchTrigger.originalTransform = ditylumZone.Find("signal_start_socket");
            switchTrigger.poemTransform = poem;

            //Make the quantum seed not eat the player
            ditylumZone.Find("quantum_bramble/player_block_trigger").gameObject.AddComponent<QuantumBlockTrigger>();

            var trigger = instrument.gameObject.AddComponent<QuantumInstrumentTrigger>();
            trigger.gatherCondition = "EyeGatherDitylum";

            var travelerData = EyeSceneHandler.GetEyeTravelerData("Ditylum");
            if (travelerData != null)
            {
                travelerData.quantumInstruments.Add(instrument);
            }
        }

        /**
         * When the player gathers the instrument, teleport them back and re-enable the teleport field
         */
        private static void OnFinishGather()
        {
            //Teleport the player
            Transform campRoot = Component.FindObjectOfType<QuantumCampsiteController>().transform;
            Transform returnSocket = campRoot.Find("InstrumentZones/DitylumZone/return_socket");
            Locator.GetPlayerBody().SetPosition(returnSocket.position);
            Locator.GetPlayerBody().SetRotation(returnSocket.rotation);
            Locator.GetPlayerBody().SetVelocity(Vector3.zero);

            //Re-enable the distance thing
            campRoot.Find("Volumes_Campfire/EndlessCylinder_Forest").GetComponent<EndlessCylinder>().SetActivation(true);
        }

        /**
         * Fixes the inflation controller
         * 
         * @param campRoot The root of the campsite sector
         */
        private static void FixInflationController(Transform campRoot)
        {
            CosmicInflationController inflator = campRoot.GetComponentInChildren<CosmicInflationController>();

            //Finally, give it the ground and the patch
            inflator._groundRenderers = AddToArray<OWRenderer>(campRoot.Find("Terrain_Campfire/Terrain_EYE_ForestFloor_Tomb/forest_new_ground/actual_ground")
                .GetComponent<OWRenderer>(), inflator._groundRenderers);
            inflator._groundRenderers = AddToArray<OWRenderer>(campRoot.Find("Terrain_Campfire/Terrain_EYE_ForestFloor_Tomb/forest_new_ground/actual_ground/ditylum_patch")
                .GetComponent<OWRenderer>(), inflator._groundRenderers);
        }

        /**
         * Adds the given element to the given array
         * 
         * @param element The thing to add
         * @param arr The array to add to
         * @return A copy of arr with the added element
         */
        private static T[] AddToArray<T>(T element, T[] arr)
        {
            T[] temp = new T[arr.Length + 1];
            arr.CopyTo(temp, 0);
            temp[temp.Length - 1] = element;
            return temp;
        }
    }
}
