using System.Collections.Generic;

namespace Model
{
    public class Node
    {
        int id;
        List<string> attributes;
        List<string> objects;
        List<int> relations;

        public Node(int id, List<string> attributes, List<string> objects, List<int> relations)
        {
            this.id = id;
            this.attributes = attributes;
            this.objects = objects;
            this.relations = relations;
        }

        public override string ToString()
        {
            string returnString = " ID : " + id + "\n";

            foreach (string attr in attributes)
            {
                returnString += attr + "\n";
            }
            foreach (string obj in objects)
                returnString += obj + "\n";
            return returnString;
        }

        public IList<int> getRelations()
        {
            return relations;
        }

        public int getID()
        {
            return this.id;
        }

        public string getAttributesString()
        {
            string attributeString = "";
            foreach (string attr in attributes)
            {
                attributeString += attr + " ";
            }
            return attributeString;
        }

        public string getObjectsString()
        {
            string objectsString = "";
            foreach (string obj in objects)
            {
                objectsString += obj + " ";
            }
            return objectsString;
        }

        public List<string> getAttributeList()
        {
            return this.attributes;
        }

        public List<string> getObjectList()
        {
            return this.objects;
        }
    }
}

