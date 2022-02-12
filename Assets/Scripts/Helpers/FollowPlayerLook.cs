using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerLook : MonoBehaviour
{
    public GameObject PlayerCamera;

    [Range(1f, 2.5f)]
    public float distance = 2;

    [Range(-2f, 2f)]
    public float yOffset = 0.5f;

    public bool FollowPosition = true;
    public bool ConstraintY = true;
    public bool LookAtPlayer = true;

    private Transform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FollowPosition)
            FollowPlayerPosition();

        if (LookAtPlayer)
            FollowPlayerEyeRotation();
    }

    void FollowPlayerPosition()
    {
        var position = PlayerCamera.transform.position + (PlayerCamera.transform.forward * distance);
        if (ConstraintY)
            position.y = PlayerCamera.transform.position.y + yOffset;

        rectTransform.position = position;
    }

    void FollowPlayerEyeRotation()
    {
        var lookPos = PlayerCamera.transform.position - rectTransform.position;
        lookPos.y = 0;

        Vector3 myEulerAngles = Quaternion.LookRotation(lookPos).eulerAngles;
        Quaternion myFixedQuaternion = Quaternion.Euler(-myEulerAngles.x, myEulerAngles.y, myEulerAngles.z);

        rectTransform.rotation = myFixedQuaternion;//Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }
}
