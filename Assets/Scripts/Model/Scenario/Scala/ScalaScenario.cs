using Assets.Scripts.Scenario;
using Assets.Scripts.Scenario.Scala;
using Factories;
using FCA.Data;
using FCA.Filesystem;
using FCA.UI;
using Model;
using System.Collections.Generic;
using UnityEngine;

namespace FCA.Repository
{
    public class ScalaScenario : Scenario
    {
        protected ScalaRepository repository;

        public IDictionary<string, GameObject> scalaList;

        public IEnumerable<string> Diagrams => repository.Diagrams;

        protected LinkedList<string> order;
        protected LinkedListNode<string> currentNode;
        protected DataContainer dataContainer;
        protected Stack<string> conditions;

        public ScalaScenario(string filename, bool lifetracks = false)
        {
            var filepath = lifetracks ?
                DirectoryConfiguration.Instance.GetLifetrackFilepath(filename) :
                DirectoryConfiguration.Instance.GetScaleFilepath(filename);

            repository = new ScalaRepository(filepath);
            scalaList = new Dictionary<string, GameObject>();
            order = new LinkedList<string>();
            dataContainer = new DataContainer(repository.DatabaseFilename);
            conditions = new Stack<string>();
            sphereFactory = new GameobjectNodeFactory();
        }

        #region Creation

        protected IReadOnlyDictionary<GameObject, int> CreateDictionary(string diagramName)
        {
            Dictionary<GameObject, int> gameobjectDepthPairs = new Dictionary<GameObject, int>();
            IEnumerable<Node> nodes = repository.GetAll(diagramName);
            foreach (Node node in nodes)
            {
                GameObject nodeSphere = sphereFactory.createInstance(node);
                int depth = repository.NodeDepth(node);
                gameobjectDepthPairs.Add(nodeSphere, depth);
            }

            return gameobjectDepthPairs;
        }

        private GameObject CreateLatticeGameobject(IReadOnlyDictionary<GameObject, int> depthDictionary)
        {
            GameObject newLattice = new GameObject("Lattice");
            Lattice latticeComponent = newLattice.AddComponent<Lattice>();


            int depth = repository.Depth();
            latticeComponent.setNodesList(depthDictionary).setDepth(depth).build();
            newLattice.transform.Rotate(new Vector3(-180, 0, 0));


            LabelStrategy labelStrategy = new ScalaStrategy(conditions, dataContainer);
            newLattice.GetComponent<Lattice>().setLabelStrategy(labelStrategy);
            //labelStrategy.setupLabels(newLattice);

            newLattice.SetActive(false);

            return newLattice;
        }

        #endregion

        #region Diagram control

        public void addDiagram(string diagramName)
        {
            Debug.Log($"Trying to add diagram {diagramName}");
            if (scalaList.ContainsKey(diagramName))
                return;

            IReadOnlyDictionary<GameObject, int> nodeDictionary = CreateDictionary(diagramName);
            GameObject lattice = CreateLatticeGameobject(nodeDictionary);

            scalaList.Add(diagramName, lattice);
            order.AddLast(diagramName);

            if (scalaList.Count == 1)
            {
                currentNode = order.First;
                updateLattice();
            }
        }

        public void removeDiagram(string diagramName)
        {
            if (scalaList.ContainsKey(diagramName))
                scalaList.Remove(diagramName);
        }

        public virtual void nextDiagram(GameObject intoNode)
        {
            if (currentNode.Next != null)
            {
                scalaList[currentNode.Value].SetActive(false);
                currentNode = currentNode.Next;
                conditions.Push(intoNode.GetComponent<GameobjectNode>().getConcept().getObjectsString());
                updateLattice();
            }
        }

        public virtual void previousDiagram()
        {
            if (currentNode.Previous != null)
            {
                scalaList[currentNode.Value].SetActive(false);
                currentNode = currentNode.Previous;
                conditions.Pop();
                updateLattice();
            }
        }

        #endregion

        #region Controls

        public void updateLattice()
        {
            scalaList[currentNode.Value].SetActive(true);
            scalaList[currentNode.Value].GetComponent<Lattice>().updateLabels(conditions);


            this.lattice = scalaList[currentNode.Value];
        }

        public override void longSelect(GameObject node)
        {
            if ("Default".Equals(node.name))
            {
                previousDiagram();
            }
            else
            {
                if (isValidForward(node))
                {
                    nextDiagram(node);
                }
            }
        }

        private bool isValidForward(GameObject node)
        {
            string nodeCondition = node.GetComponent<GameobjectNode>().getConcept().getObjectsString();
            return (!"".Equals(nodeCondition.Trim()));
        }

        public override void touchpadPress()
        {
            // To see!
        }

        #endregion
    }
}
