using System;
using UnityEngine;

namespace Datastructures
{
    class Position
    {
        readonly Vector3 position;
        GameObject holding;
        bool occupied;

        public Position(Vector3 position)
        {
            this.position = position;
            this.occupied = false;
        }

        public Vector3 getPosition()
        {
            return this.position;
        }

        public Boolean isOccupied()
        {
            return occupied;
        }

        public void setNode(GameObject node)
        {
            node.transform.position = position;
            occupied = true;
            holding = node;
        }
    }
}
