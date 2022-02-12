using InputSystem;
using UnityEngine;
using Valve.VR;

public class MoveAction : MonoBehaviour
{
    public SteamVR_Action_Boolean ActionTrigger;
    public GameObject Pivot;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private Vector3 lastPosition;

    void Start()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    void Update()
    {
        if (ActionTrigger.GetStateDown(m_Pose.inputSource))
        {
            lastPosition = transform.position;
        }

        if (ActionTrigger.GetState(m_Pose.inputSource))
        {
        }
    }
}
