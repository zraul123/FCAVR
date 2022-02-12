using System.Collections.Generic;
using System.Xml;

namespace Assets.Scripts.Parser
{
    class CSXParser
    {
        public string DatabaseName { get; set; }

        public List<string> Diagrams { get; set; }

        public IDictionary<string, IList<IDictionary<string, string>>> NodesDictionary { get; set; }

        public CSXParser(string filepath)
        {
            Diagrams = new List<string>();
            NodesDictionary = new Dictionary<string, IList<IDictionary<string, string>>>();

            ReadXml(filepath);
        }

        #region Reading

        private void ReadXml(string filepath)
        {
            var xmlDocument = LoadXml(filepath);
            var nodeList = ReadDiagramsNodeList(xmlDocument);

            ReadDatabaseName(xmlDocument);
            ReadDiagramsFromNodeList(nodeList);
        }

        private XmlDocument LoadXml(string filepath)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(filepath);

            return xmlDocument;
        }

        private void ReadDatabaseName(XmlDocument document)
        {
            DatabaseName = document.GetElementsByTagName("embed")[0].Attributes["url"].Value;
        }
        private XmlNodeList ReadDiagramsNodeList(XmlDocument document)
        {
            return document.GetElementsByTagName("diagram");
        }

        private void ReadDiagramsFromNodeList(XmlNodeList nodes)
        {
            foreach (XmlNode diagram in nodes)
            {
                string name = diagram.Attributes["title"].Value;

                Diagrams.Add(name);
                IList<IDictionary<string, string>> diagramNodeList = TransformNode(diagram);
                NodesDictionary.Add(name, diagramNodeList);
            }
        }

        #endregion

        #region Helpers

        private IList<IDictionary<string, string>> TransformNode(XmlNode node)
        {
            if (node == null)
                return null;
            IList<IDictionary<string, string>> returnedList = new List<IDictionary<string, string>>();
            XmlNodeList nodeList = node.ChildNodes;
            foreach (XmlNode childNode in nodeList)
            {
                if (childNode.Name.Equals("node"))
                {
                    IDictionary<string, string> objectDictionary = new Dictionary<string, string>();
                    objectDictionary.Add("ID", childNode.Attributes["id"].Value);
                    objectDictionary.Add("Relation", "");
                    foreach (XmlNode subNode in childNode.ChildNodes)
                    {
                        if (subNode.Name.Equals("concept"))
                        {
                            foreach (XmlNode contigentNode in subNode.ChildNodes)
                            {
                                if (contigentNode.Name.Equals("objectContingent"))
                                {
                                    XmlNode objectContigentNode = contigentNode.FirstChild;
                                    if (objectContigentNode != null)
                                        objectDictionary.Add("Extent", objectContigentNode.InnerText);
                                    else
                                        objectDictionary.Add("Extent", "");
                                }
                                else if (contigentNode.Name.Equals("attributeContingent"))
                                {
                                    XmlNode attributeContigentNode = contigentNode.FirstChild;
                                    if (attributeContigentNode != null)
                                        objectDictionary.Add("Intent", attributeContigentNode.InnerText);
                                    else
                                        objectDictionary.Add("Intent", "");
                                }

                            }
                        }
                    }
                    returnedList.Add(objectDictionary);
                }
                else if (childNode.Name.Equals("edge"))
                {
                    string from = childNode.Attributes["from"].Value;
                    string to = childNode.Attributes["to"].Value;
                    foreach (IDictionary<string, string> localDictionary in returnedList)
                    {
                        if (localDictionary["ID"].Equals(from))
                        {
                            if (localDictionary["Relation"].Equals(""))
                                localDictionary["Relation"] = $"({from},{to}) ";
                            else
                                localDictionary["Relation"] = localDictionary["Relation"] + $", ({from},{to})";
                        }
                    }
                }
            }

            return returnedList;
        }

        #endregion
    }
}
