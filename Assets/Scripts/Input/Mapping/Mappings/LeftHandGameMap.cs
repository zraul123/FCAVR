using Assets.Scripts.Input.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Assets.Scripts.Input.Mapping.Mappings
{
    class LeftHandGameMap : AbstractInputMap
    {
        private Vector3 LastLocalControllerPosition;
        private GameObject Player;
        private Pivot Pivot;

        void Start()
        {
            Player = Configuration.Configuration.Instance.Player;
            Pivot = Configuration.Configuration.Instance.Pivot;
        }

        public override void ShortGripPress()
        {

        }

        public override void LongGripPress()
        {

        }

        public override void MenuPress()
        {

        }

        public override void TouchpadPress()
        {

        }

        public override void TriggerPressed()
        {
            MovePivotAndPlayer();
        }

        public override void TriggerPressDown()
        {
            LastLocalControllerPosition = transform.localPosition;
        }

        #region Actions

        private void MovePivotAndPlayer()
        {
            var movementVector = transform.localPosition - LastLocalControllerPosition;

            Player.transform.position = Player.transform.position + movementVector;
        }

        #endregion
    }
}
