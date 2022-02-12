using Assets.Scripts.Input.Actions;
using UnityEngine;
using Valve.VR;

public class RotateAction : MonoBehaviour
{
    public SteamVR_Action_Boolean ActionTrigger;
    public GameObject Pivot;

    private Vector3 lastLocalPosition;
    private SteamVR_Behaviour_Pose _pose;
    private GameObject _player;

    void Start()
    {
        _pose = GetComponent<SteamVR_Behaviour_Pose>();
        _player = GameObject.Find("Player");
    }
    void Update()
    {

        if (ActionTrigger.GetStateDown(_pose.inputSource))
        {
            lastLocalPosition = transform.localPosition;
        }

        if (ActionTrigger.GetState(_pose.inputSource))
        {
            float angle = 3f * Vector2.SignedAngle(lastLocalPosition, transform.localPosition);

            if (angle != 0)
            {
                _player.transform.RotateAround(Vector3.zero, Vector3.up, angle);

                lastLocalPosition = transform.localPosition;
            }
        }
    }
}
