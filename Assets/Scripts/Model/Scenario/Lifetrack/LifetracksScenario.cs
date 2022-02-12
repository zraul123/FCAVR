using Assets.Scripts.Scenario.Lifetrack;
using FCA.UI;
using System.Collections.Generic;
using UnityEngine;

namespace FCA.Repository
{
    class LifetracksScenario : ScalaScenario
    {
        private GameObject timeDiagram;
        private GameObject lifetrackLattice;
        private State currentState;

        public LifetracksScenario(string filename) : base(filename, true) {}

        public void BuildLifetrack(LifetracksScenarioStartupConfiguration configuration)
        {
            addDiagram(configuration.Subject);
            addDiagram(configuration.Searched);

            State smallerState = new State(new Vector3(0.3f, 0.3f, 0.3f));
            State biggerState = new State(new Vector3(0.7f, 0.7f, 0.7f));

            smallerState.setNextState(biggerState);
            biggerState.setNextState(smallerState);

            currentState = smallerState;

            IReadOnlyDictionary<GameObject, int> nodeDictionary = CreateDictionary(configuration.Time);
            timeDiagram = CreateLatticeGameobject(nodeDictionary);
        }

        private Dictionary<string, string> CreateTimeConditions()
        {
            Dictionary<string, string> returnedDictionary = new Dictionary<string, string>();

            IEnumerable<GameObject> timeDiagramNodes = timeDiagram.GetComponent<Lattice>().getNodeList();

            foreach (GameObject timeNode in timeDiagramNodes)
            {
                string objectContigent = timeNode.GetComponent<GameobjectNode>().getConcept().getObjectsString().Trim();

                if (!"".Equals(objectContigent))
                {
                    string attributeContigent = timeNode.GetComponent<GameobjectNode>().getConcept().getAttributesString().Trim();
                    if (!"".Equals(attributeContigent))
                        returnedDictionary.Add(attributeContigent, objectContigent);
                }
            }

            return returnedDictionary;
        }

        protected GameObject CreateLatticeGameobject(IReadOnlyDictionary<GameObject, int> depthDictionary)
        {
            GameObject newLattice = new GameObject("Lattice");
            Lattice latticeComponent = newLattice.AddComponent<Lattice>();


            int depth = repository.Depth();
            newLattice.GetComponent<Lattice>().setNodesList(depthDictionary).setDepth(depth).build();
            newLattice.transform.Rotate(new Vector3(-180, 0, 0));


            LabelStrategy labelStrategy = new ScalaStrategy(conditions, dataContainer);
            newLattice.GetComponent<Lattice>().setLabelStrategy(labelStrategy);
            //labelStrategy.setupLabels(newLattice);

            newLattice.SetActive(false);

            return newLattice;
        }

        public override void nextDiagram(GameObject intoNode)
        {
            if (intoNode == null)
                return;

            string intoNodeContigent = intoNode.GetComponent<GameobjectNode>().getConcept().getObjectsString().Trim();
            if ("".Equals(intoNodeContigent))
                return;

            Dictionary<string, string> timeConditions = CreateTimeConditions();

            LifetrackBuilder builder = new LifetrackBuilder();
            GameObject lifetrackLattice = builder
                .dataContainer(dataContainer)
                .subjectCondition(intoNodeContigent)
                .lattice(scalaList[currentNode.Next.Value])
                .timeConditions(timeConditions)
                .build();

            showLifetrack(lifetrackLattice);
        }

        private void showLifetrack(GameObject lattice)
        {
            scalaList[currentNode.Value].SetActive(false);
            this.lifetrackLattice = lattice;
            lattice.SetActive(true);
        }

        public override void touchpadPress()
        {
            GameObject arrowHolder = getArrowHolder();
            changeState(arrowHolder);
        }

        private void changeState(GameObject arrowHolder)
        {
            currentState = currentState.getNextState();
            currentState.execute(arrowHolder);
        }

        private GameObject getArrowHolder()
        {
            Transform transform = lifetrackLattice.transform.Find("Arrows");
            if (transform == null)
            {
                GameObject arrowHolder = new GameObject("Arrows");
                arrowHolder.transform.SetParent(lifetrackLattice.transform);
                transform = arrowHolder.transform;
            }

            return transform.gameObject;
        }
    }
}
