using Assets.Scripts.Configuration;
using Assets.Scripts.Input;
using Assets.Scripts.Input.Actions;
using Assets.Scripts.Input.Pointers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

namespace InputSystem
{
    public class InputMapper : MonoBehaviour
    {
        public SteamVR_Input_Sources InputSource;

        //SteamVR_LaserPointer laserPointer;
        private Dictionary<InputMode, AbstractInputMap> InputMap = new Dictionary<InputMode, AbstractInputMap>();
        private EventManager EventManager;

        private InputMode CurrentMode => EventManager.CurrentMode;

        private float GripStateDownTime;
        private GameObject HoveringNode;

        [Header("Pointer Hand")]
        public bool IsPointerHand = false;
        private CanvasPointer CanvasPointer;
        private SteamVR_LaserPointer LaserPointer;

        //public SteamVR_Action_Boolean touchpadPress;

        //private GameObject hoveringNode;
        //private GameObject defaultNode;
        //private bool didLongSelect;


        void Start()
        {
            EventManager = Configuration.Instance.EventManager;
            
            if (IsPointerHand)
            {
                CanvasPointer = GetComponent<CanvasPointer>();
                LaserPointer = GetComponent<SteamVR_LaserPointer>();
                LaserPointer.PointerIn += LaserPointer_In;
                LaserPointer.PointerOut += LaserPointer_Out;
            }
            //defaultNode = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //defaultNode.name = "Default";
            //defaultNode.SetActive(false);

            RegisterInputMaps();
            RegisterInputEvents();
        }

        void Update()
        {
            if (SteamVR_Actions.HoverUI.TriggerPress.GetStateDown(InputSource))
                if (InputMap.ContainsKey(CurrentMode))
                    InputMap[CurrentMode].TriggerPressDown();

            if (SteamVR_Actions.HoverUI.TriggerPress.GetState(InputSource))
                if (InputMap.ContainsKey(CurrentMode))
                    InputMap[CurrentMode].TriggerPressed();


        }

        #region Registering Maps

        private void RegisterInputMaps()
        {
            var attachedAbstractInputMaps = GetComponents<AbstractInputMap>();
            foreach (AbstractInputMap inputMap in attachedAbstractInputMaps)
            {
                RegisterModeInput(inputMap.InputMode, inputMap);
            }
        }

        private void RegisterModeInput(InputMode mode, AbstractInputMap map)
        {
            if (!InputMap.ContainsKey(mode))
                InputMap.Add(mode, map);
        }

        #endregion

        #region Registering Input Events

        private void RegisterInputEvents()
        {
            SteamVR_Actions.HoverUI.Activate();
            SteamVR_Actions.HoverUI.GripPress.AddOnStateDownListener(GripPress_StateDown, InputSource);
            SteamVR_Actions.HoverUI.GripPress.AddOnStateUpListener(GripPress_StateUp, InputSource);
            SteamVR_Actions.HoverUI.TouchpadPress.AddOnStateDownListener(TouchpadPress_StateDown, InputSource);
            SteamVR_Actions.HoverUI.MenuPress.AddOnStateDownListener(MenuPress_StateDown, InputSource);
        }

        #endregion

        #region Event Handlers

        private void GripPress_StateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            GripStateDownTime = Time.time;
        }

        private void GripPress_StateUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            if (!InputMap.ContainsKey(CurrentMode))
                return;

            if (Time.time - GripStateDownTime < Configuration.Instance.WarpSeconds)
            {
                InputMap[CurrentMode].ShortGripPress();
            }
            else
            {
                InputMap[CurrentMode].LongGripPress();
            }
        }

        private void MenuPress_StateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            if (InputMap.ContainsKey(CurrentMode))
                InputMap[CurrentMode].MenuPress();
        }

        private void TouchpadPress_StateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            if (InputMap.ContainsKey(CurrentMode))
                InputMap[CurrentMode].TouchpadPress();
        }

        #endregion

        /*
        //// EXAMINING METHODS
        private void MenuPress_StateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            InputMap.MenuPress();
        }

        private void TouchPadPress_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            eventManager.touchpadPress();
        }

        #region Grip Events

        private void GripPress_onStateUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            StopCoroutine("LongSelect");
            if (!didLongSelect)
            {
                eventManager.ShortSelect(hoveringNode);
            }
        }

        private void GripPress_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            // Used defaultNode to not let courutine argument list be empty!
            GameObject givenObject = hoveringNode ?? defaultNode;
            didLongSelect = false;
            StartCoroutine("LongSelect", givenObject);
        }

        IEnumerator LongSelect(GameObject hoveredObject)
        {
            yield return new WaitForSeconds(eventManager.GetWarpSeconds());
            didLongSelect = true;
            eventManager.LongSelect(hoveredObject);
        }

        #endregion

        
    */
    #region Laser Events

        private void LaserPointer_In(object sender, PointerEventArgs args)
        {
            if (args.target.GetComponent<GameobjectNode>() != null)
            {
                EventManager.HitNode(args);
                HoveringNode = args.target.gameObject;
            }
        }

        private void LaserPointer_Out(object sender, PointerEventArgs args)
        {
            if (args.target.GetComponent<GameobjectNode>() != null)
            {
                EventManager.OutNode(args);
                HoveringNode = null;
            }
        }

        #endregion
    }
}