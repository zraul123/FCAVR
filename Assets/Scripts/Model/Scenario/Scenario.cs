using Factories;
using UnityEngine;

namespace Assets.Scripts.Scenario
{
    public abstract class Scenario
    {
        protected GameobjectNodeFactory sphereFactory;
        protected GameObject lattice;

        public Scenario()
        {}

        public abstract void longSelect(GameObject node);

        public void setSelected(GameObject node)
        {
            lattice.GetComponent<Lattice>().setHighlightedChain(node);
        }

        public void resetSelected()
        {
            lattice.GetComponent<Lattice>().shallowNodes(false);
        }

        public abstract void touchpadPress();
    }
}
