using Datastructures;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [Range(0, 5)]
    public float xradius = 5;
    [Range(0, 5)]
    public float yradius = 5;

    public float scaleFactor;

    public List<GameObject> circleNodes;
    public int circleCount;
    public int depth { get; private set; }

    public void initialize(int depth)
    {
        circleNodes = new List<GameObject>();
        this.depth = depth;
    }

    public void addNode(GameObject addedNode)
    {

        // Setting the parent to be the circle
        addedNode.transform.SetParent(this.gameObject.transform);

        // Adding the node to the node list
        circleNodes.Add(addedNode);
        addedNode.name = $"[{gameObject.name}][{addedNode.GetComponent<GameobjectNode>().getConcept().getID()}]";
        addedNode.GetComponent<GameobjectNode>().depth = depth;

        // Updating the position list
        update();
    }

    public void update()
    {
        circleCount = circleNodes.Count;
        setPositions();
    }

    void setPositions()
    {
        // Calculating the positions for the number of nodes we're holding
        List<Position> positions = calculateNodePositions();

        IEnumerator<Position> positionEnumerator = positions.GetEnumerator();
        IEnumerator<GameObject> nodeEnumerator = circleNodes.GetEnumerator();

        while (positionEnumerator.MoveNext() && nodeEnumerator.MoveNext())
        {
            Vector3 position = positionEnumerator.Current.getPosition();
            GameObject node = nodeEnumerator.Current;
            node.transform.position = position;
        }
    }

    private List<Position> calculateNodePositions()
    {
        float x, z;
        List<Position> positionList = new List<Position>();
        float angle = 20f;
        // Minimum 1
        // Maximum clamped : 10 => [1,10]
        // Desired interval = [1,2.5f]

        float scaleFactor = (Mathf.Clamp(circleNodes.Count, 1, 10) / 4f) + 1;


        // If only one set the position
        if (circleNodes.Count == 1)
        {
            Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            positionList.Add(new Position(position));
            return positionList;
        }

        // If more than one
        for (int i = 0; i < (circleNodes.Count); i++)
        {
            // Calculating the X and Z
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * (xradius * scaleFactor);
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * (yradius * scaleFactor);

            // Creating the Vector3 and adding them to the positionlist
            Vector3 position = new Vector3(x, this.transform.position.y, z);
            positionList.Add(new Position(position));

            // Increment
            angle += (360f / circleNodes.Count);
        }
        return positionList;
    }

    public List<GameObject> getCircleNodes()
    {
        return this.circleNodes;
    }

    public void setScaleFactor(float newScaleFactor)
    {
        this.scaleFactor = newScaleFactor;
    }

    internal Position findFirstFreePosition(List<Position> positionList)
    {
        foreach (Position position in positionList)
        {
            if (!position.isOccupied())
                return position;
        }
        return null;
    }
}