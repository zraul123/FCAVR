using FCA.Data;
using FCA.UI;
using InputSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FCA.Repository
{
    class LifetrackBuilder
    {
        private GameObject arrowPrefab;
        GameObject latticeObject;
        GameObject originalLattice;
        Dictionary<string, string> tConditions;
        string sCondition;
        DataContainer data;

        public LifetrackBuilder()
        {
            arrowPrefab = GameObject.Find("EventManager").GetComponent<EventManager>().getArrowPrefab();
        }

        public LifetrackBuilder subjectCondition(string condition)
        {
            this.sCondition = condition;
            return this;
        }

        public LifetrackBuilder timeConditions(Dictionary<string, string> tConditions)
        {
            this.tConditions = tConditions;
            return this;
        }

        public LifetrackBuilder lattice(GameObject latticeObject)
        {
            this.originalLattice = latticeObject;
            return this;
        }

        public LifetrackBuilder dataContainer(DataContainer data)
        {
            this.data = data;
            return this;
        }


        public GameObject build()
        {
            IDictionary<string, int> infimumDictionary = createInfimumDictionary();
            LabelStrategy labelStrategy = new LifetrackStrategy(infimumDictionary);
            originalLattice.GetComponent<Lattice>().setLabelStrategy(labelStrategy);

            buildArrows(infimumDictionary);


            return originalLattice;
        }


        private IDictionary<string, int> createInfimumDictionary()
        {
            WeekComparer weekComparer = new WeekComparer();
            IDictionary<string, int> returnedDictionary = new SortedDictionary<string, int>(weekComparer);

            foreach (KeyValuePair<string, string> nameCondition in tConditions)
            {
                List<string> conditionList = new List<string>();
                conditionList.Add(sCondition);
                conditionList.Add(nameCondition.Value);
                int lowestID = getInfimumId(conditionList);
                returnedDictionary.Add(nameCondition.Key, lowestID);

            }


            return returnedDictionary;
        }

        private List<GameObject> getObjectNodes(List<string> condition)
        {
            List<GameObject> returnedList = new List<GameObject>();

            // TO REFACTOR : DO THIS WITHOUT QUEUE
            IEnumerable<GameObject> nodeList = originalLattice.GetComponent<Lattice>().getNodeList();

            foreach (GameObject node in nodeList)
            {
                GameobjectNode nodeScript = node.GetComponent<GameobjectNode>();
                string objectCondition = nodeScript.getConcept().getObjectsString().Trim();

                if ("".Equals(objectCondition))
                    continue;

                if (condition.Count == 3)
                {
                    condition[2] = objectCondition;
                }
                else
                {
                    condition.Add(objectCondition);
                }

                int total = data.getLifetrackCondition(condition[0], condition[1], condition[2]);
                if (total > 0)
                    returnedList.Add(node);
            }

            return returnedList;
        }

        private int getInfimumId(List<string> conditions)
        {
            Lattice lattice = originalLattice.GetComponent<Lattice>();
            List<GameObject> objectList = getObjectNodes(conditions);
            GameObject infimum = lattice.getInfimum(objectList);


            return infimum.GetComponent<GameobjectNode>().getConcept().getID();
        }

        // TO REFACTOR : THIS FUNCTION NEEDS TO BE MOVED FROM HERE!
        private GameObject getArrowHolder()
        {
            Transform transform = originalLattice.transform.Find("Arrows");
            if (transform == null)
            {
                GameObject arrowHolder = new GameObject("Arrows");
                arrowHolder.transform.SetParent(originalLattice.transform);
                transform = arrowHolder.transform;
            }

            return transform.gameObject;
        }

        private void buildArrows(IDictionary<string, int> infimumDictionary)
        {
            if (infimumDictionary.Count <= 1)
                throw new ArgumentException("You cannot have less than 2 time nodes!");

            GameObject arrowHolder = getArrowHolder();
            restartArrowHolder(arrowHolder);

            GameObject current;
            GameObject next;
            IComparer<string> comparer = new WeekComparer();
            Lattice latticeScript = originalLattice.GetComponent<Lattice>();
            List<string> orderList = infimumDictionary.Keys.ToList();
            orderList.Sort(comparer);
            IEnumerator<string> firstEnumerator = orderList.GetEnumerator();
            IEnumerator<string> secondEnumerator = orderList.GetEnumerator();
            secondEnumerator.MoveNext();
            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
            {
                current = latticeScript.FindGameobjectNodeById(infimumDictionary[firstEnumerator.Current]);
                next = latticeScript.FindGameobjectNodeById(infimumDictionary[secondEnumerator.Current]);

                if (current != next)
                {
                    // Draw arrow
                    GameObject createdArrow = createArrowBetween(current, next);
                    createdArrow.SetActive(true);
                    createdArrow.transform.SetParent(arrowHolder.transform);
                }
            }
        }


        private GameObject createArrowBetween(GameObject parent, GameObject child)
        {
            GameObject arrowObject = GameObject.Instantiate(arrowPrefab);

            Arrow arrowComponent = arrowObject.AddComponent<Arrow>();
            arrowComponent.parentNode = parent;
            arrowComponent.childNode = child;

            // Set the position of the connection capsule
            //arrowObject.transform.position = (child.transform.position - parent.transform.position) / 2f + parent.transform.position;

            return arrowObject;
        }

        private void restartArrowHolder(GameObject arrowHolder)
        {
            foreach (Transform transform in arrowHolder.transform)
            {
                GameObject.Destroy(transform.gameObject);
            }
        }
    }
}
