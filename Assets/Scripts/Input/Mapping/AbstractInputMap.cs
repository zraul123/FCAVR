using InputSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Input
{
    public enum InputMode
    {
        Menu,
        Game
    }

    public abstract class AbstractInputMap : MonoBehaviour
    {
        public InputMode InputMode;

        protected EventManager EventManager { get; set; }

        void Start()
        {
            EventManager = Configuration.Configuration.Instance.EventManager;
        }

        #region Overrides

        public abstract void ShortGripPress();

        public abstract void LongGripPress();

        public abstract void TriggerPressDown();

        public abstract void TriggerPressed();

        public abstract void MenuPress();

        public abstract void TouchpadPress();

        #endregion

    }
}
