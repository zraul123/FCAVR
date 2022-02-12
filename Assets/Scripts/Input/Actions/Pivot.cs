using UnityEngine;
using Assets.Scripts.Configuration;
using Valve.VR.InteractionSystem;

namespace Assets.Scripts.Input.Actions
{
    class Pivot : MonoBehaviour
    {
        public Vector3 Position => transform.position;

        private GameObject Player;

        void Start()
        {
            Player = Configuration.Configuration.Instance.InputConfiguration.Player;
        }

        public void SnapToActiveLatticeMiddle()
        {
            var activeLattice = GetActiveLattice();
            if (activeLattice != null)
            {
                var position = activeLattice.transform.position;
                position.y = activeLattice.GetComponent<Lattice>().GetLatticeHeight() / 2;
                transform.position = position;
            }
        }

        #region Helpers

        private GameObject GetActiveLattice()
        {
            var lattices = FindObjectsOfType<Lattice>();
            foreach(Lattice lattice in lattices)
            {
                if (lattice.gameObject.activeInHierarchy)
                    return lattice.gameObject;
            }

            return null;
        }

        #endregion
    }
}
