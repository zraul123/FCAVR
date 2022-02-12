using Assets.Scripts.Parser;
using Factories;
using Ganter.Algorithm;
using Model;
using System.Collections.Generic;
using System.Linq;
using Valve.VR.InteractionSystem;

class ContextRepository
{
    IFactory<Node> factory;
    IList<Node> nodes;

    CXTParser parser;
    FormalContext formalContext;

    public ContextRepository(string filepath)
    {
        parser = new CXTParser(filepath);
        factory = new NodeFactory();
        nodes = new List<Node>();

        InitializeFormalContext();
        InitializeNodes();
    }

    private void InitializeFormalContext()
    {
        var attributes = parser.Attributes;
        var items = parser.Items;
        var matrix = parser.Matrix;

        formalContext = new FormalContext(attributes, items, matrix);
    }

    private void InitializeNodes()
    {
        var nodesDictionary = ExtractNodeDictionary();
        nodesDictionary.Select(dictionary => factory.createInstance(dictionary))
            .ForEach(node => nodes.Add(node));
    }

    private IList<IDictionary<string, string>> ExtractNodeDictionary()
    {
        IList<IDictionary<string, string>> dictionaryList = new List<IDictionary<string, string>>();
        List<List<Item>> extents = new List<List<Item>>();
        List<List<Attribute>> intents = formalContext.PerformAlgorithm();
        var transitiveReduction = true;

        Dictionary<int, HashSet<int>> result = formalContext.FormOutput(intents, extents, transitiveReduction);

        IEnumerator<List<Attribute>> intentEnumerator = intents.GetEnumerator();
        IEnumerator<List<Item>> extentEnumerator = extents.GetEnumerator();
        int i = 0;
        while (intentEnumerator.MoveNext() && extentEnumerator.MoveNext())
        {
            Dictionary<string, string> nodeDictionary = new Dictionary<string, string>();
            nodeDictionary["ID"] = i.ToString();
            nodeDictionary["Intent"] = string.Join(", ", intentEnumerator.Current.Select(a => a.Name));
            nodeDictionary["Extent"] = string.Join(", ", extentEnumerator.Current.Select(e => e.Name));
            i++;
            dictionaryList.Add(nodeDictionary);
        }

        for (i = 0; i < result.Count; i++)
        {
            dictionaryList[i]["Relation"] = string.Format("{0}", result[i].RelationsString(i));
        }
        return dictionaryList;
    }

    public Node FindById(int id)
    {
        return nodes[id];
    }

    public int Depth()
    {
        Node startingNode = nodes[nodes.Count - 1];
        return NodeDepth(startingNode);
    }

    public int NodeDepth(int id)
    {
        return NodeDepth(FindById(id));
    }

    public int NodeDepth(Node checkedNode)
    {
        int max = 0;
        foreach (int childNodeID in checkedNode.getRelations())
        {
            Node childNode = FindById(childNodeID);
            int childDepth = NodeDepth(childNode);
            max = (max >= childDepth) ? max : childDepth;
        }
        return max + 1;
    }

    public IEnumerable<Node> GetAll()
    {
        return nodes;
    }
}
