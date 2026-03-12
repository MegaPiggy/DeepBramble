using NewHorizons;
using NewHorizons.Components.Props;
using NewHorizons.Handlers;
using UnityEngine;

namespace DeepBramble.BaseInheritors
{
    public class InjectorItem : NHItem
    {
        /**
         * Need to set up a couple of things when the object wakes up
         */
        public override void Awake()
        {
            base.Awake();
            _localDropNormal = new Vector3(0, 0, -1);
            _localDropOffset = new Vector3(0, 0, -0.0698f);

            _type = DeepBramble.InjectorItemType;

            // UI translation
            DisplayName = "Toxin Injector";

            PickupAudio = AudioType.Lantern_Pickup;
            DropAudio = AudioType.Lantern_Drop;
            SocketAudio = AudioType.Lantern_Insert;
            UnsocketAudio = AudioType.Lantern_Remove;
        }

        /**
         * Play the socketing animation
         */
        public override void PlaySocketAnimation()
        {
            GetComponentInChildren<Animator>().SetTrigger("socket");
        }
    }
}
