using FCA.UI;
using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;

public class Lattice : MonoBehaviour
{
    private IReadOnlyDictionary<GameObject, int> nodesList;
    private IList<GameObject> circleList;
    private int latticeDepth;
    private LabelStrategy labelStrategy;
    private float fadeTime = 0.5f;

    [SerializeField] public float inbetweenDepth = 4f;


    private void Awake()
    {
        circleList = new List<GameObject>();
    }

    public IEnumerable<GameObject> getNodeList()
    {
        return nodesList.Keys;
    }

    public Lattice setNodesList(IReadOnlyDictionary<GameObject, int> nodeList)
    {
        this.nodesList = nodeList;
        return this;
    }

    public Lattice setDepth(int depth)
    {
        this.latticeDepth = depth;
        return this;
    }

    public void build()
    {
        CreateCircles();
        SetNodePositions();
        CreateRelations(nodesList);
        CreateNodeConnections();
        this.transform.position += new Vector3(0, 2, 0);
        this.transform.tag = "Lattice";
    }

    public float GetLatticeHeight()
    {
        return ((circleList.Count - 1) * inbetweenDepth);
    }

    private void CreateCircles()
    {
        for (int i = 0; i < latticeDepth; i++)
        {
            GameObject newCircle = new GameObject("Circle - " + i);
            newCircle.AddComponent<Circle>();
            newCircle.GetComponent<Circle>().initialize(i);
            newCircle.transform.SetParent(this.transform);
            newCircle.transform.position = new Vector3(newCircle.transform.position.x, newCircle.transform.position.y + (inbetweenDepth * i) * (-1), newCircle.transform.position.z);
            circleList.Add(newCircle);
        }
    }

    private void SetNodePositions()
    {
        foreach (GameObject nodeObject in nodesList.Keys)
        {
            int depth = nodesList[nodeObject] - 1;
            circleList[depth].GetComponent<Circle>().addNode(nodeObject);
        }
    }

    private void CreateRelations(IReadOnlyDictionary<GameObject, int> nodeList)
    {
        IEnumerator<KeyValuePair<GameObject, int>> enumerator = nodeList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            Node concept = enumerator.Current.Key.GetComponent<GameobjectNode>().getConcept(); //enumerator.Current.GetComponent<GameobjectNode>().getConcept();
            foreach (int relationId in concept.getRelations())
            {
                enumerator.Current.Key.GetComponent<GameobjectNode>().addRelation(FindGameobjectNodeById(relationId));
            }
        }
    }

    private void CreateNodeConnections()
    {
        foreach (GameObject nodeObject in nodesList.Keys)
        {
            nodeObject.GetComponent<GameobjectNode>().createConnections();
        }
    }

    public GameObject FindGameobjectNodeById(int id)
    {
        foreach (KeyValuePair<GameObject, int> keyValue in nodesList)
        {
            if (keyValue.Key.GetComponent<GameobjectNode>().ID == id)
            {
                return keyValue.Key;
            }
        }
        return null;
    }

    public void setupLabels()
    {
        labelStrategy.setupLabels(this.gameObject);
    }

    public void updateLabels(Stack<string> conditions)
    {
        List<string> conditionList = new List<string>(conditions);
        this.labelStrategy.updateCondition(conditionList);
    }

    public void setLabelStrategy(LabelStrategy strategy)
    {
        this.labelStrategy = strategy;
        setupLabels();
    }

    public LabelStrategy getLabelStrategy()
    {
        return this.labelStrategy;
    }

    public void setHighlightedChain(GameObject node)
    {
        // Make everything else shallow
        shallowNodes(true);

        // Highlight selected chain
        node.GetComponent<GameobjectNode>().setHighlightLevel(HighlightLevel.Selected, true, true);
        List<GameObject> upperRelations = node.GetComponent<GameobjectNode>().getAllUpperRelations();
        List<GameObject> lowerRelations = node.GetComponent<GameobjectNode>().getAllLowerRelations();
        foreach (GameObject upperChainNode in upperRelations)
        {
            upperChainNode.GetComponent<GameobjectNode>().setHighlightLevel(HighlightLevel.Selected, true, true);
        }
        foreach (GameObject lowerChainNode in lowerRelations)
        {
            lowerChainNode.GetComponent<GameobjectNode>().setHighlightLevel(HighlightLevel.Selected, true, true);
        }

        highlightCorrespondingConnectionlines(HighlightLevel.Selected);
    }

    private void highlightCorrespondingConnectionlines(HighlightLevel level)
    {
        foreach (GameObject node in nodesList.Keys)
        {
            node.GetComponent<GameobjectNode>().highlightConnectionLines(level);
        }
    }

    public void shallowNodes(bool shallow, bool withReset = false)
    {
        foreach (GameObject node in nodesList.Keys)
        {
            node.GetComponent<GameobjectNode>().setHighlightLevel(HighlightLevel.Shallow, true, false);
        }

        highlightCorrespondingConnectionlines(HighlightLevel.Shallow);
    }

    public void unselectChain()
    {
        foreach (GameObject node in nodesList.Keys)
        {
            node.GetComponent<GameobjectNode>().setHighlightLevel(HighlightLevel.Normal, true, false);
        }
    }

    public GameObject getScalaEverythingNode()
    {
        return circleList[circleList.Count - 1].transform.GetChild(0).gameObject;
    }

    public GameObject getScalaNothingNode()
    {
        return circleList[0].transform.GetChild(0).gameObject;
    }

    public GameObject getInfimum(List<GameObject> objects)
    {
        if (objects.Count == 1)
            return objects[0];
        if (objects.Count == 0)
            return getScalaEverythingNode();

        List<GameObject> intersections = getIntersections(objects);
        GameObject bestNode = null;
        int bestDepth = int.MinValue;
        foreach (GameObject intersectionNode in intersections)
        {
            if (intersectionNode.GetComponent<GameobjectNode>().depth > bestDepth)
            {
                bestNode = intersectionNode;
                bestDepth = intersectionNode.GetComponent<GameobjectNode>().depth;
            }
        }

        return bestNode;
    }


    private List<GameObject> getIntersections(List<GameObject> objects)
    {
        List<GameObject> intersectedObjects = new List<GameObject>();
        foreach (GameObject node in objects)
        {
            List<GameObject> lowerNodes = node.GetComponent<GameobjectNode>().getAllLowerRelations();
            lowerNodes.Add(node);
            if (intersectedObjects.Count == 0)
            {
                intersectedObjects.AddRange(lowerNodes);
            }
            else
            {
                IEnumerable<GameObject> intersectedNodes = intersectedObjects.Intersect(lowerNodes);
                List<GameObject> tempList = new List<GameObject>(intersectedNodes);
                intersectedObjects.Clear();
                intersectedObjects.AddRange(tempList);
            }
        }
        return intersectedObjects;
    }

    public void StartFade()
    {
        StartCoroutine(FadeScreen());
    }

    private IEnumerator FadeScreen()
    {
        yield return new WaitForSeconds(fadeTime);
    }

    public void UnfadeScreen()
    {
        SteamVR_Fade.Start(Color.clear, fadeTime, true);
    }
}
