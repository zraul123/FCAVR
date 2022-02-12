using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FCA.UI
{
    class ContextStrategy : LabelStrategy
    {
        private List<string> getAttributeList(IEnumerable<GameObject> nodeEnumerable)
        {
            List<string> attributeList = new List<string>();

            foreach (GameObject node in nodeEnumerable)
            {
                List<string> nodeAttributes = node.GetComponent<GameobjectNode>().getConcept().getAttributeList();

                foreach (string attribute in nodeAttributes)
                {
                    string trimmedText = attribute.Trim();

                    if ("".Equals(trimmedText))
                    {
                        continue;
                    }

                    if (!attributeList.Exists(x => x.Trim().Equals(trimmedText)))
                    {
                        attributeList.Add(trimmedText);
                    }
                }
            }

            return attributeList;
        }

        private List<string> getObjectList(IEnumerable<GameObject> nodeEnumearble)
        {
            List<string> objectList = new List<string>();

            foreach (GameObject node in nodeEnumearble)
            {
                List<string> nodeObjects = node.GetComponent<GameobjectNode>().getConcept().getObjectList();

                foreach (string objectText in nodeObjects)
                {
                    string trimmedText = objectText.Trim();

                    if ("".Equals(trimmedText))
                    {
                        continue;
                    }

                    if (!objectList.Exists(x => x.Trim().Equals(trimmedText)))
                    {
                        objectList.Add(trimmedText);
                    }
                }
            }

            return objectList;
        }


        public void setupLabels(GameObject latticeObject)
        {
            IEnumerable<GameObject> nodeList = latticeObject.GetComponent<Lattice>().getNodeList();

            setAttributes(nodeList);
            setObjects(nodeList.Reverse());
        }

        private void setAttributes(IEnumerable<GameObject> nodeList)
        {
            List<string> attributeList = getAttributeList(nodeList);

            foreach (GameObject node in nodeList)
            {
                List<string> nodeAttributes = new List<string>(node.GetComponent<GameobjectNode>().getConcept().getAttributeList().Select(x => x.Trim()));
                List<string> attributeUnion = new List<string>(attributeList.Intersect(nodeAttributes));
                string joinedAttribute = string.Join(", ", attributeUnion);

                if (!"".Equals(joinedAttribute))
                {
                    LabelSystem labelSystem = node.GetComponent<GameobjectNode>().getLabelSystem() ?? new LabelSystem(node);
                    labelSystem.setAttributeText(joinedAttribute);
                    foreach (string attribute in attributeUnion)
                    {
                        attributeList.Remove(attribute);
                    }
                }
            }

        }

        private void setObjects(IEnumerable<GameObject> nodeList)
        {
            List<string> objectList = getObjectList(nodeList);

            foreach (GameObject node in nodeList)
            {
                List<string> nodeObjects = new List<string>(node.GetComponent<GameobjectNode>().getConcept().getObjectList().Select(x => x.Trim()));
                List<string> objectUnion = new List<string>(objectList.Intersect(nodeObjects));
                string joinedObjects = string.Join(", ", objectUnion);

                if (!"".Equals(joinedObjects))
                {
                    LabelSystem labelSystem = node.GetComponent<GameobjectNode>().getLabelSystem() ?? new LabelSystem(node);
                    labelSystem.setObjectText(joinedObjects);
                    foreach (string objectText in objectUnion)
                    {
                        objectList.Remove(objectText);
                    }
                }
            }
        }


        public void updateCondition(List<string> conditions)
        {
            throw new NotImplementedException();
        }
    }
}
