using NewHorizons.Components.Props;

namespace DeepBramble.BaseInheritors
{
    public class InjectorSocket : NHItemSocket
    {

        /**
         * Need to give the transform before the base awake method (it'll always be the active transform)
         */
        public override void Awake()
        {
            _socketTransform = transform.Find("guide_transform");
            _acceptableType = DeepBramble.InjectorItemType;

            base.Awake();
        }

        /**
         * When something gets slotted in, need to kill the dilation node
         * 
         * @param item The item that was placed
         */
        public override bool PlaceIntoSocket(OWItem item)
        {
            bool ret = base.PlaceIntoSocket(item);
            if (ret)
            {
                DeepBramble.debugPrint("Injector socket should kill node");
                EnableInteraction(false);
                ForgottenLocator.dilationNodeKiller.KillNode();
            }
            return ret;
        }
    }
}
