using FCA.UI;
using Model;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum HighlightLevel
{
    Selected,
    Normal,
    Shallow
}

public delegate void transformHasChanged(object source, EventArgs args);

public class GameobjectNode : MonoBehaviour
{
    Node concept;
    List<GameObject> relations;
    List<GameObject> lowerRelation;
    Material ownMaterial;
    LabelSystem labelSystem;

    public int ID;
    public string attributes;
    public string objects;

    public event transformHasChanged OnTransformChanged;


    // Selection attributes
    private bool isSelected = false;
    Color defaultColor = Color.white;

    Dictionary<HighlightLevel, Color> highlightMap = new Dictionary<HighlightLevel, Color>(){
        { HighlightLevel.Selected, Color.yellow },
        { HighlightLevel.Normal, Color.white},
        { HighlightLevel.Shallow, new Color(218,223,225,0.3f) }
        };

    HighlightLevel currentHighlight = HighlightLevel.Normal;

    List<GameObject> connectionLines;
    public int depth { get; set; }

    void Awake()
    {

        // Setting everything up
        relations = new List<GameObject>();
        connectionLines = new List<GameObject>();
        lowerRelation = new List<GameObject>();

        ownMaterial = GetComponent<Renderer>().material;
        ownMaterial.shader = Shader.Find("Transparent/Diffuse");
    }

    void Update()
    {
        if (transform.hasChanged)
        {
            fixConnections(); // Fix the lower connections
            foreach (GameObject node in lowerRelation) // Fix the upper connections
            {
                node.GetComponent<GameobjectNode>().fixNodeConnection(this.gameObject);
            }
            OnTransformChanged?.Invoke(null, new EventArgs());
            transform.hasChanged = false;
        }
    }

    void OnEnable()
    {
        if (labelSystem != null)
            labelSystem.enable();
    }

    void OnDisable()
    {
        if (labelSystem != null)
            labelSystem.disable();
    }


    /*
     * Getters and setters
     */

    public void setLabelSystem(LabelSystem system)
    {
        this.labelSystem = system;
    }

    public LabelSystem getLabelSystem()
    {
        return this.labelSystem;
    }

    public void setConcept(Node conceptNode)
    {
        this.concept = conceptNode;
        this.ID = conceptNode.getID();
        this.attributes = conceptNode.getAttributesString();

    }

    public void addRelation(GameObject relationNode)
    {
        relations.Add(relationNode);
        relationNode.GetComponent<GameobjectNode>().addLowerRelation(this.gameObject);
    }

    public void addLowerRelation(GameObject relationNode)
    {
        lowerRelation.Add(relationNode);
    }


    public Node getConcept()
    {
        return concept;
    }
    /*
     * Helper functions to fix the transform of an connection capsule
     */

    // Method to create/fix a connection capsule between two nodes
    public void fixConnectionTransform(GameObject targetNode, GameObject connectionCapsule)
    {
        float distanceToTargetNode = Vector3.Distance(transform.position, targetNode.transform.position); // Calculate the distance between parent node and target node
        // Set the scaling of the connection capsule
        connectionCapsule.transform.localScale = new Vector3(0.1f, distanceToTargetNode / 2, 0.1f);
        // Set the correct rotation of the connection capsule
        connectionCapsule.transform.rotation = Quaternion.FromToRotation(Vector3.up, targetNode.transform.position - transform.position);
        // Set the position of the connection capsule
        connectionCapsule.transform.position = (targetNode.transform.position - transform.position) / 2f + transform.position;

        connectionCapsule.transform.SetParent(this.transform);
    }

    // Method to fix all lower neighbours connections
    public void fixConnections()
    {
        // Get an enumerator for both list to get through them at the same time
        var lowerNeighboursEnumerator = relations.GetEnumerator(); // Get the node we're currently working on
        var connectionCapsuleEnumerator = connectionLines.GetEnumerator(); // And it's capsule
        while (lowerNeighboursEnumerator.MoveNext() && connectionCapsuleEnumerator.MoveNext())
        {
            GameObject targetNode = lowerNeighboursEnumerator.Current; // Get the GameObject references
            GameObject connectionCapsule = connectionCapsuleEnumerator.Current; // To both of them
            fixConnectionTransform(targetNode, connectionCapsule);
        }
    }

    // Method to create or re-make the connection lines
    public void createConnections()
    {
        foreach (GameObject g in relations)
        {
            GameObject connectionCapsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            connectionCapsule.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            connectionCapsule.name = this.concept.getID().ToString();
            connectionLines.Add(connectionCapsule);
        }

        fixConnections();
    }

    // Method to fix connection only with one node
    public void fixNodeConnection(GameObject targetNode)
    {
        int nodeIndex = relations.IndexOf(targetNode); // Get the index of the target node
        fixConnectionTransform(targetNode, connectionLines[nodeIndex]);
    }


    public void setHighlightLevel(HighlightLevel highlightLevel, bool permanent = false, bool selected = false)
    {
        Color nodeColor = highlightMap[highlightLevel];
        if (permanent)
        {
            this.defaultColor = nodeColor;
            this.isSelected = selected;
            currentHighlight = highlightLevel;
        }

        this.ownMaterial.color = nodeColor;

    }

    public void highlightConnectionLines(HighlightLevel highlightLevel)
    {
        Color highlightColor = highlightMap[highlightLevel];
        foreach (GameObject node in relations)
        {
            if ((node.GetComponent<GameobjectNode>().getHighlightLevel() == highlightLevel) && (currentHighlight == highlightLevel))
            {
                int nodeIndex = relations.IndexOf(node);
                connectionLines[nodeIndex].GetComponent<Renderer>().material.color = highlightColor;
            }
        }
    }

    public void resetHighlightDefault()
    {
        this.ownMaterial.color = defaultColor;
    }

    public string getInfoTextString()
    {
        string returnedString = "[ID] " + concept.getID() + "\n" +
                                "[Attributes]" + concept.getAttributesString() + "\n" +
                                "[Objects]" + concept.getObjectsString();
        return returnedString;
    }

    // TODO: REFACTOR EVERYTHING SO THE NAMES MATCHES!
    public List<GameObject> getLowerRelations()
    {
        return this.relations;
    }

    public List<GameObject> getUpperRelations()
    {
        return this.lowerRelation;
    }

    public List<GameObject> getAllLowerRelations()
    {
        List<GameObject> allLowerRelations = new List<GameObject>();
        List<GameObject> iteratingList = new List<GameObject>();
        iteratingList.AddRange(this.relations);

        while (iteratingList.Count > 0)
        {
            GameObject current = iteratingList[0];
            allLowerRelations.Add(current);
            iteratingList.Remove(current);
            iteratingList.AddRange(current.GetComponent<GameobjectNode>().getLowerRelations());
        }

        return allLowerRelations;
    }

    public List<GameObject> getAllUpperRelations()
    {
        List<GameObject> allUpperRelations = new List<GameObject>();
        List<GameObject> iteratingList = new List<GameObject>();
        iteratingList.AddRange(this.lowerRelation);

        while (iteratingList.Count > 0)
        {
            GameObject current = iteratingList[0];
            allUpperRelations.Add(current);
            iteratingList.Remove(current);
            iteratingList.AddRange(current.GetComponent<GameobjectNode>().getUpperRelations());
        }
        return allUpperRelations;
    }

    public List<GameObject> getAllUpperConnectionLines()
    {
        List<GameObject> allUpperConnectionLines = new List<GameObject>();
        List<GameObject> iteratingList = getAllUpperRelations();

        foreach (GameObject node in iteratingList)
        {

        }
        return allUpperConnectionLines;
    }

    public HighlightLevel getHighlightLevel()
    {
        return this.currentHighlight;
    }

}
