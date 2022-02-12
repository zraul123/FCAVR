using Assets.Scripts.Scenario;
using Assets.Scripts.Scenario.Context;
using Factories;
using FCA.Filesystem;
using FCA.UI;
using Model;
using System.Collections.Generic;
using UnityEngine;

namespace FCA.Repository
{
    class ContextScenario : Scenario
    {
        private ContextRepository repository;

        #region Initialization

        public ContextScenario(ContextScenarioStartupConfiguration configuration) : base()
        {
            var filepath = DirectoryConfiguration.Instance.GetContextFilepath(configuration.Name);
            sphereFactory = new GameobjectNodeFactory();
            repository = new ContextRepository(filepath);

            Initalize();
        }

        private void Initalize()
        {
            IReadOnlyDictionary<GameObject, int> nodeDictionary = CreateDictionary();
            GameObject lattice = CreateLatticeGameobject(nodeDictionary);

            this.lattice = lattice;
            lattice.SetActive(true);
        }

        #endregion

        #region Creation

        public IReadOnlyDictionary<GameObject, int> CreateDictionary()
        {
            Dictionary<GameObject, int> gameobjectDepthPairs = new Dictionary<GameObject, int>();
            IEnumerable<Node> nodes = repository.GetAll();
            foreach (Node node in nodes)
            {
                GameObject nodeSphere = sphereFactory.createInstance(node);
                int depth = repository.NodeDepth(node);
                gameobjectDepthPairs.Add(nodeSphere, depth);
            }

            return gameobjectDepthPairs;
        }

        public GameObject CreateLatticeGameobject(IReadOnlyDictionary<GameObject, int> depthDictionary)
        {
            GameObject newLattice = new GameObject("Lattice");
            Lattice latticeComponent = newLattice.AddComponent<Lattice>();

            int depth = repository.Depth();
            latticeComponent.setNodesList(depthDictionary).setDepth(depth).build();
            newLattice.transform.position += new Vector3(0, newLattice.GetComponent<Lattice>().GetLatticeHeight(), 0);

            LabelStrategy labelStrategy = new ContextStrategy();
            newLattice.GetComponent<Lattice>().setLabelStrategy(labelStrategy);
            //labelStrategy.setupLabels(newLattice);

            return newLattice;
        }

        #endregion

        #region Controls


        public override void longSelect(GameObject node)
        {
        }


        public override void touchpadPress()
        {
        }

        #endregion
    }
}
