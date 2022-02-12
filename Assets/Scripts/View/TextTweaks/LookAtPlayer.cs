using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            Camera m_Camera = Camera.main;
            transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
        }
    }
}
