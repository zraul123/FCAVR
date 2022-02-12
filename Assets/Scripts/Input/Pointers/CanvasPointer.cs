using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Input.Pointers
{
    class CanvasPointer : MonoBehaviour
    {
        public float defaultLength = 3.0f;
        public EventSystem eventSystem;
        public StandaloneInputModule inputModule = null;

        private LineRenderer lineRenderer = null;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            this.enabled = false;
        }

        private void Update()
        {
            UpdateLength();
        }

        private void UpdateLength()
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, GetEnd());
        }

        private Vector3 GetEnd()
        {
            float distance = GetCanvasDistance();
            Vector3 endPosition = CalculateEnd(defaultLength);

            if (distance != 0.0f)
                endPosition = CalculateEnd(distance);

            return endPosition;
        }

        private float GetCanvasDistance()
        {
            PointerEventData eventData = new PointerEventData(eventSystem);
            eventData.position = inputModule.inputOverride.mousePosition;

            var results = new List<RaycastResult>();
            eventSystem.RaycastAll(eventData, results);

            RaycastResult closestResult = FindFirstRaycast(results);
            float distance = closestResult.distance;

            distance = Mathf.Clamp(distance, 0, defaultLength);

            return distance;
        }

        private RaycastResult FindFirstRaycast(List<RaycastResult> raycastResults)
        {
            foreach (RaycastResult result in raycastResults)
            {
                if (!result.gameObject)
                    continue;

                return result;
            }

            return new RaycastResult();
        }

        private Vector3 CalculateEnd(float length)
        {
            return transform.position + (transform.forward * length);
        }
    }
}
