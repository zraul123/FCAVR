using FCA.Data;
using System.Collections.Generic;
using UnityEngine;

namespace FCA.UI
{
    class ScalaStrategy : LabelStrategy
    {
        private Lattice lattice;
        private List<string> conditions;
        private DataContainer data;

        public ScalaStrategy(DataContainer container)
        {
            this.conditions = new List<string>();
            this.data = container;
        }

        public ScalaStrategy(Stack<string> pastConditions, DataContainer container)
        {
            this.conditions = new List<string>(pastConditions);
            this.data = container;
        }

        public void setupLabels(GameObject latticeObject)
        {
            this.lattice = latticeObject.GetComponent<Lattice>();

            foreach (GameObject node in this.lattice.getNodeList())
            {
                LabelSystem labelSystem = new LabelSystem(node);
                setNodeObjectText(node);
                setNodeAttributeText(node);
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

        // TO DO: MAKE THIS ABSTRACT IN ISCALALABELSTRATEGY
        private void setNodeObjectText(GameObject node)
        {
            LabelSystem system = node.GetComponent<GameobjectNode>().getLabelSystem();
            string objectContigent = node.GetComponent<GameobjectNode>().getConcept().getObjectsString();

            if ("".Equals(objectContigent.Trim()))
            {
                system.setObjectText("");
                return;
            }


            List<string> nodeCondition = new List<string>(conditions);
            nodeCondition.Add(objectContigent);

            string objectText = data.getTotal(nodeCondition).ToString();

            system.setObjectText(objectText);
        }

        public void updateCondition(List<string> conditions)
        {
            this.conditions = conditions;
            foreach (GameObject node in this.lattice.getNodeList())
            {
                setNodeObjectText(node);
            }
        }
    }
}
