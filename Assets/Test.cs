using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var hand = GetComponent<Hand>();
        ControllerButtonHints.ShowButtonHint(hand, SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip"));
    }
}
