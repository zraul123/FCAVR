using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Input.Pointers
{
    public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color enterColor = Color.white;
        [SerializeField] private Color downColor = Color.white;
        [SerializeField] private UnityEvent OnClick = new UnityEvent();

        private MeshRenderer meshRenderer = null;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            meshRenderer.material.color = enterColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            meshRenderer.material.color = normalColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            meshRenderer.material.color = downColor;
            print("Down");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            meshRenderer.material.color = enterColor;
            print("Up");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke();
            print("Click");
        }
    }


}
