using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputModule : BaseInput
{
    public Camera eventCamera = null;

    public SteamVR_Action_Boolean Trigger;
    public SteamVR_Behaviour_Pose Pose;

    protected override void Awake()
    {
        GetComponent<BaseInputModule>().inputOverride = this;
        SteamVR_Actions.HoverUI.Activate();
    }

    public override bool GetMouseButton(int button)
    {
        var state = Trigger.GetState(Pose.inputSource);
        Debug.Log("Mouse Button: " + state);
        return state;
    }

    public override bool GetMouseButtonDown(int button)
    {
        var state = Trigger.GetStateDown(Pose.inputSource);
        return state;
    }

    public override bool GetMouseButtonUp(int button)
    {
        var state = Trigger.GetStateUp(Pose.inputSource);
        return state;
    }

    public override Vector2 mousePosition
    { 
        get 
        {
            return new Vector2(eventCamera.pixelWidth / 2, eventCamera.pixelHeight / 2);
        }
    }
}
