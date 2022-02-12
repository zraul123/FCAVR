using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace FCA.Filesystem
{
    class DirectoryConfiguration
    {
        private static DirectoryConfiguration _instance;
        private static string DataPath = Application.dataPath + "/lattices";

        public static DirectoryConfiguration Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DirectoryConfiguration();
                return _instance;
            }
        }

        private DirectoryConfiguration() { }

        public List<string> ContextLattices => GetFilesInDataDirectory("context", "*.cxt");

        public List<string> ScalaLattices => GetFilesInDataDirectory("scala", "*.csx");

        public List<string> LifetrackLattices => GetFilesInDataDirectory("lifetrack", "*.csx");

        public string GetContextFilepath(string filename) => GetFilepath("context", filename);

        public string GetScaleFilepath(string filename) => GetFilepath("scala", filename);

        public string GetLifetrackFilepath(string filename) => GetFilepath("lifetrack", filename);

        private List<string> GetFilesInDataDirectory(string dataDirectory, string fileExtension)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(DataPath + "/" + dataDirectory);
            return directoryInfo.GetFiles(fileExtension).Select(file => file.Name).ToList();
        }

        private string GetFilepath(string dataDirectory, string filename)
        {
            return DataPath + "/" + dataDirectory + "/" + filename;
        }
    }
}
