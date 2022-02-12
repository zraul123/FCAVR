using UnityEngine;

public class RotateSlowly : MonoBehaviour
{
    [Range(0f, 10f)]
    public float angle = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, angle);
    }
}
