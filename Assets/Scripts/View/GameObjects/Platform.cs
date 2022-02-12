using UnityEngine;

public class Platform : MonoBehaviour
{

    public Material inactiveMaterial;
    public Material activeMaterial;

    void Awake()
    {
        GetComponent<Renderer>().material = inactiveMaterial;
    }

    public Material getInactiveMaterial()
    {
        return this.inactiveMaterial;
    }

    public Material getActiveMaterial()
    {
        return this.activeMaterial;
    }
}
