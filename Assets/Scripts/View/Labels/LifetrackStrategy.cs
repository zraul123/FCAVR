using System;
using System.Collections.Generic;
using UnityEngine;

namespace FCA.UI
{
    class LifetrackStrategy : LabelStrategy
    {
        IDictionary<int, string> timeDictionary;

        public LifetrackStrategy(IDictionary<string, int> timeDictionary)
        {
            this.timeDictionary = reverseDictionary(timeDictionary);
        }

        private IDictionary<int, string> reverseDictionary(IDictionary<string, int> weekIDDictionary)
        {
            Dictionary<int, string> returnedDictionary = new Dictionary<int, string>();

            foreach (KeyValuePair<string, int> weekId in weekIDDictionary)
            {
                if (returnedDictionary.ContainsKey(weekId.Value))
                    returnedDictionary[weekId.Value] += $", {weekId.Key}";
                else
                    returnedDictionary.Add(weekId.Value, weekId.Key);
            }

            return returnedDictionary;
        }

        public void setupLabels(GameObject latticeObject)
        {
            foreach (GameObject node in latticeObject.GetComponent<Lattice>().getNodeList())
            {
                LabelSystem labelSystem = new LabelSystem(node);
                setNodeAttributeText(node);
            }

            setNodeObjectText(latticeObject);
        }

        public void updateCondition(List<string> conditions)
        {
            throw new NotImplementedException();
        }

        private void setNodeObjectText(GameObject latticeObject)
        {
            foreach (KeyValuePair<int, string> idWeek in timeDictionary)
            {
                GameObject node = latticeObject.GetComponent<Lattice>().FindGameobjectNodeById(idWeek.Key);
                LabelSystem labelSystem = node.GetComponent<GameobjectNode>().getLabelSystem();
                labelSystem.setObjectText(idWeek.Value);
            }
        }

        // TO DO : MAKE NEW ISCALALABELSTRATEGY AND PUSH THIS THERE
        private void setNodeAttributeText(GameObject node)
        {
            LabelSystem system = node.GetComponent<GameobjectNode>().getLabelSystem();
            string attributeText = node.GetComponent<GameobjectNode>().getConcept().getAttributesString();

            if ("".Equals(attributeText))
            {
                system.setAttributeText("");
                return;
            }

            system.setAttributeText(attributeText);
        }
    }
}
