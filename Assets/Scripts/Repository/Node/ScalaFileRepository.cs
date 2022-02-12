using Assets.Scripts.Parser;
using Factories;
using Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace FCA.Repository
{
    public class ScalaRepository
    {
        CSXParser parser;

        IDictionary<string, IList<IDictionary<string, string>>> scalaList;
        IFactory<Node> factory;
        IList<Node> currentNodeList;

        public string DatabaseFilename => parser.DatabaseName;

        public List<string> Diagrams => parser.Diagrams;

        public ScalaRepository(string filepath)
        {
            parser = new CSXParser(filepath);
            factory = new NodeFactory();

            scalaList = parser.NodesDictionary;
        }

        public IEnumerable<Node> GetAll(string scalaName)
        {
            UnityEngine.Debug.Log($"Trying to get {scalaName}");
            IList<IDictionary<string, string>> keyValuePairs = scalaList[scalaName];
            currentNodeList = new List<Node>();
            if (keyValuePairs != null)
            {
                foreach (Dictionary<string, string> nodeRepresentation in keyValuePairs)
                {
                    Node newNode = factory.createInstance(nodeRepresentation);
                    currentNodeList.Add(newNode);
                }
            }
            return currentNodeList;
        }

        public int Depth()
        {
            int maxDepth = currentNodeList.Select((x) => NodeDepth(x)).Max();
            return maxDepth;
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

        public Node FindById(int id)
        {
            return currentNodeList.SkipWhile((x) => x.getID() != id).First();
        }
    }
}
