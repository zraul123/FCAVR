using Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Factories
{
    class NodeFactory : IFactory<Node>
    {
        private string regexPattern = @"\([0-9]*,[0-9]*\)";
        public Node createInstance(IDictionary<string, string> readData)
        {
            int newID = int.Parse(readData["ID"]);
            List<string> attributes = readData["Intent"].Split(',').ToList();
            List<string> objects = readData["Extent"].Split(',').ToList();
            List<int> relations = new List<int>();

            Regex regex = new Regex(regexPattern);
            Match relationMatch = regex.Match(readData["Relation"]);
            while (relationMatch.Success)
            {
                string[] relationValue = relationMatch.Value.Replace(')', ' ').Replace('(', ' ').Trim().Split(',').ToArray();

                int checkID = int.Parse(relationValue[0]);
                int relationID = int.Parse(relationValue[1]);

                if (checkID == newID)
                {
                    relations.Add(relationID);
                }
                relationMatch = relationMatch.NextMatch();
            }

            if (attributes == null)
                attributes = new List<string>();

            if (objects == null)
                objects = new List<string>();

            return new Node(newID, attributes, objects, relations);

        }
    }
}


