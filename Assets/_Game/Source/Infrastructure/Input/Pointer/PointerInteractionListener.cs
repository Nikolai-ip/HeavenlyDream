using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _Game.Source.Infrastructure.Input.Pointer
{
    /// <summary>
    /// Conditions for correct work:
    /// Object has Collider component on it. (use right collider, 3d not works for 2d and reverse)
    /// Camera has Physics 2D Raycaster component.
    /// Project has EventSystem object. (can be alone without canvas and etc.)
    /// </summary>
    public class PointerInteractionListener: MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler 
    {
        [Header("Events with PointerEventData argument")]
        public UnityEvent<PointerEventData> onPointerDownEventData;
        public UnityEvent<PointerEventData> onPointerUpEventData;
        public UnityEvent<PointerEventData> onBeginDragEventData;
        public UnityEvent<PointerEventData> onDragEventData;
        public UnityEvent<PointerEventData> onEndDragEventData;
        [Space]
        [Header("Events with empty arguments")]
        public UnityEvent onPointerDown;
        public UnityEvent onPointerUp;
        public UnityEvent onBeginDrag;
        public UnityEvent onDrag;
        public UnityEvent onEndDrag;


        
        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDownEventData.Invoke(eventData);
            onPointerDown.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUpEventData.Invoke(eventData);
            onPointerUp.Invoke();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDragEventData.Invoke(eventData);
            onBeginDrag.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            onDragEventData.Invoke(eventData);
            onDrag.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDragEventData.Invoke(eventData);
            onEndDrag.Invoke();
        }
    }
}