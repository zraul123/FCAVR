using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FCA.UI
{
    public class LabelSystem
    {
        private GameObject owningObject;
        private LineRenderer lineRenderer;
        private string attributeText = "", objectText = "";
        private Vector3 offsetCoords;
        private bool initialized = false;
        private GameObject worldCanvas;
        private GameObject attributeTextObject, objectTextObject;

        public LabelSystem(GameObject owner)
        {
            this.owningObject = owner;
            this.owningObject.GetComponent<GameobjectNode>().setLabelSystem(this);
        }

        private void initialize()
        {
            lineRenderer = owningObject.GetComponent<LineRenderer>();
            if (lineRenderer == null)
                lineRenderer = owningObject.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = true;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.startColor = Color.grey;
            lineRenderer.endColor = Color.grey;
            lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
            lineRenderer.material.color = Color.grey;
            this.lineRenderer.enabled = false;
            initialized = true;
        }

        public void enable()
        {
            setRecursivelyActive(true);
        }

        public void disable()
        {
            setRecursivelyActive(false);
        }

        private void setRecursivelyActive(bool active)
        {
            if (this.lineRenderer != null)
                this.lineRenderer.enabled = active;

            if (this.attributeTextObject != null)
                this.attributeTextObject.SetActive(active);

            if (this.objectTextObject != null)
                this.objectTextObject.SetActive(false);
        }

        /// <summary>
        /// Should check whether attribute and object texts are valid, and if so display them.
        /// </summary>
        private void update()
        {
            if (!initialized)
                initialize();

            List<Vector3> positions = new List<Vector3>();

            float theX;
            float theZ;
            calculateEndpointPosition(out theX, out theZ);

            // To be more efficient, we assume that the lattices origin point is at (0,0,0).    
            if (!"".Equals(attributeText.Trim()))
            {
                positions.Add(owningObject.transform.position);
                Vector3 labelPosition = new Vector3(theX, owningObject.transform.position.y + 1.5f, theZ);
                if (attributeTextObject == null)
                {
                    attributeTextObject = createText(attributeText, labelPosition);
                }
                else
                {
                    attributeTextObject.GetComponent<Text>().text = attributeText;
                    attributeTextObject.transform.position = labelPosition;
                }


                positions.Add(labelPosition);
                this.lineRenderer.enabled = true;

            }
            /*
            if (!"".Equals(objectText.Trim()) && (!"0".Equals(objectText.Trim())))
            {
                positions.Add(owningObject.transform.position);
                Vector3 labelPosition = new Vector3(theX, owningObject.transform.position.y - 1.5f, theZ);
                if (objectTextObject == null)
                {
                    objectTextObject = createText(objectText, labelPosition);
                }
                else
                {
                    objectTextObject.GetComponent<Text>().text = objectText;
                    objectTextObject.transform.position = labelPosition;
                }

                positions.Add(labelPosition);
                this.lineRenderer.enabled = true;
            }
            */
            assignPositions(positions);
        }


        private void calculateEndpointPosition(out float newX, out float newZ)
        {
            float length = 1;
            float slopeOffset = 0;
            float slope;

            if (owningObject.transform.position.x == 0)
            {
                slope = 90 * Mathf.Sign(owningObject.transform.position.z);
            }
            else
            {
                slope = (owningObject.transform.position.z / owningObject.transform.position.x);
            }

            float alpha = Mathf.Atan(slope);
            if (owningObject.transform.position.x < 0)
            {
                alpha += Mathf.PI;
            }

            float xOffset = length * Mathf.Cos(alpha + slopeOffset);
            float zOffset = length * Mathf.Sin(alpha + slopeOffset);

            newX = (xOffset + owningObject.transform.position.x);
            newZ = (zOffset + owningObject.transform.position.z);

        }

        private GameObject createText(string text, Vector3 position)
        {
            if (worldCanvas == null)
                worldCanvas = GameObject.Find("WorldCanvas");

            GameObject createdText = new GameObject();
            createdText.transform.position = position;
            Text myText = createdText.AddComponent<Text>();
            myText.text = text;
            myText.font = Font.CreateDynamicFontFromOSFont("Tahoma", 1);
            myText.fontSize = 20;
            myText.alignment = TextAnchor.MiddleCenter;
            createdText.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            createdText.transform.localScale = new Vector3(3f, 3f, 1);

            // TESTING
            createdText.AddComponent<LookAtPlayer>();

            createdText.transform.SetParent(worldCanvas.transform);

            return createdText;
        }

        public void setAttributeText(string newText)
        {
            this.attributeText = newText;
            update();
        }

        public void setObjectText(string newText)
        {
            this.objectText = newText;
            update();
        }

        private void assignPositions(List<Vector3> positionList)
        {
            lineRenderer.positionCount = positionList.Count;
            for (int i = 0; i < positionList.Count; i++)
            {
                lineRenderer.SetPosition(i, positionList[i]);
            }
        }

        public void refreshPosition()
        {
            update();
        }
    }
}
