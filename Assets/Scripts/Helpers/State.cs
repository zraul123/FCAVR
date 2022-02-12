using UnityEngine;

namespace FCA.Repository
{
    class State
    {
        private Vector3 scaleVector;
        private State nextState;

        public State(Vector3 scaleVector)
        {
            this.scaleVector = scaleVector;
        }

        public void setNextState(State newState)
        {
            this.nextState = newState;
        }

        public State getNextState()
        {
            return this.nextState;
        }

        public void execute(GameObject arrowHolder)
        {
            foreach (Transform child in arrowHolder.transform)
            {
                child.GetComponent<Arrow>().updateScale(scaleVector);
            }
        }

    }
}
