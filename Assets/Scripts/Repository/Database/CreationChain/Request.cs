using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FCA.Data.Chain
{
    public class Request
    {
        private string creationString;
        private string[] insertionStrings;
        private string connectionString;
        private string filepath;
        private string tableName;

        public Request(string filepath)
        {
            this.filepath = filepath;
            extractStrings();
        }


        public Request(string creationString, string[] insertionStrings)
        {
            this.creationString = creationString;
            this.insertionStrings = insertionStrings;
        }

        public string getCreationString() { return this.creationString; }
        public string[] getInsertionStrings() { return this.insertionStrings; }
        public void setConnectionString(string newConnectionString) { this.connectionString = newConnectionString; }
        public string getConnectionString() { return this.connectionString; }
        public void setTableName(string newTableName) { this.tableName = newTableName; }
        public string getTableName() { return this.tableName; }


        public void extractStrings()
        {
            string[] sourceStrings = File.ReadAllLines(filepath);
            IList<string> creationString = new List<string>();
            IList<string> insertionStrings = new List<string>();

            string currentLine;
            int i = 0;
            do
            {
                currentLine = sourceStrings[i];
                creationString.Add(currentLine);
                i++;
            } while (!currentLine.Contains(");"));
            for (int x = i; x < sourceStrings.Length; x++)
            {
                insertionStrings.Add(sourceStrings[x]);
            }

            this.creationString = String.Join("", creationString.ToArray());
            this.insertionStrings = insertionStrings.ToArray();
        }
    }
}
