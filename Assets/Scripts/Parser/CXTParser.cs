using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts.Parser
{
    class CXTParser
    {
        private int _objectCount, _attributeCount;
        string[] lines;

        public IList<string> Attributes { get; set; }
        public IList<string> Items { get; set; }
        public bool[,] Matrix { get; set; }

        public CXTParser(string filepath)
        {
            lines = File.ReadAllLines(filepath);
            ReadMetadata();
            Attributes = new List<string>();
            Items = new List<string>();
            Matrix = new bool[_objectCount, _attributeCount];
            ReadData();
        }

        #region Reading

        private void ReadMetadata()
        {
            _objectCount = int.Parse(lines[2]);
            _attributeCount = int.Parse(lines[3]);
        }

        private void ReadData()
        {
            ReadAttributes();
            ReadItems();
            ReadMatrix();
        }

        private void ReadAttributes()
        {
            for (int i = 5 + _objectCount; i < (5 + _objectCount) + _attributeCount; i++)
            {
                Attributes.Add(lines[i]);
            }
        }

        private void ReadItems()
        {
            for (int i = 5; i < 5 + _objectCount; i++)
            {
                Items.Add(lines[i]);
            }
        }

        private void ReadMatrix()
        {
            for (int i = 0; i < _objectCount; i++)
            {
                bool[] lineBooleans = ProcessLine(lines[i + 5 + _objectCount + _attributeCount]);
                for (int j = 0; j < _attributeCount; j++)
                {
                    Matrix[i, j] = lineBooleans[j];
                }
            }
        }

        #endregion

        #region Helpers

        private bool[] ProcessLine(string line)
        {
            bool[] lineBoolean = new bool[_attributeCount];
            for (int i = 0; i < _attributeCount; i++)
            {
                if (line[i].Equals('X'))
                {
                    lineBoolean[i] = true;
                }
                else if (line[i].Equals('.'))
                {
                    lineBoolean[i] = false;
                }
                else
                {
                    throw new InvalidDataException();
                }
            }
            return lineBoolean;
        }

        #endregion
    }

}
