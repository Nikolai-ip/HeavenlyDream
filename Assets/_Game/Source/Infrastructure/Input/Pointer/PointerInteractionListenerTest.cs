using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Source.Infrastructure.Input.Pointer
{
    public class PointerInteractionListenerTest: MonoBehaviour
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUp");
        }
        public void OnPointerStartDrag(PointerEventData eventData)
        {
            Debug.Log("OnPointerStartDrag");
        }
        public void OnPointerDrag(PointerEventData eventData)
        {
            Debug.Log("OnPointerDrag");
        }
        public void OnPointerEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnPointerEndDrag");
        }
    }
}