using UnityEngine;
using Valve.VR;

public class TestLabel : MonoBehaviour
{
    LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPositions(new Vector3[] { transform.position, transform.forward * 10 });

        SteamVR_Actions.HoverUI.Activate();
    }

}
