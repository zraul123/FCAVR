using Assets.Scripts.Input.Actions;
using Assets.Scripts.Input.Pointers;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

namespace Assets.Scripts.Input.Mapping.Mappings
{

    class RightHandGameMap : AbstractInputMap
    {
        public float RotateSpeed = 3f;

        private Vector3 LastLocalControllerPosition;
        private GameObject Player;

        void Start()
        {
            Player = Configuration.Configuration.Instance.Player;
            EventManager = Configuration.Configuration.Instance.EventManager;
        }

        #region Overrides

        public override void ShortGripPress()
        {
            // Select node
        }

        public override void LongGripPress()
        {
            // Next/previous diagram (Scala)

        }

        public override void MenuPress()
        {
            EventManager.ToggleMenuVisibility(true);
        }

        public override void TouchpadPress()
        {
        }

        public override void TriggerPressed()
        {
            RotateAroundPivot();
        }

        public override void TriggerPressDown()
        {
            LastLocalControllerPosition = transform.localPosition;
        }

        #endregion

        #region Actions

        private void RotateAroundPivot()
        {
            float angle = RotateSpeed * Vector2.SignedAngle(LastLocalControllerPosition, transform.localPosition);

            if (angle == 0)
                return;

            Player.transform.RotateAround(Vector3.zero, Vector3.up, angle);
            LastLocalControllerPosition = transform.localPosition;
        }

        #endregion
    }
}
