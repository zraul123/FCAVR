using Model;
using UnityEngine;

namespace Factories
{
    public class GameobjectNodeFactory
    {
        public GameObject createInstance(Node creatingNode)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.AddComponent<GameobjectNode>();
            sphere.GetComponent<GameobjectNode>().setConcept(creatingNode);
            return sphere;
        }
    }
}
