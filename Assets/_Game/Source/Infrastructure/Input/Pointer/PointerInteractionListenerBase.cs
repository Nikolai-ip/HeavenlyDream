using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _Game.Source.Infrastructure.Input.Pointer
{
    public abstract class PointerInteractionListenerBase : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        [FormerlySerializedAs("_pointerDownInterval")]
        [SerializeField, Min(0f)] private float pointerDownInterval = 0.2f;
        [FormerlySerializedAs("_pointerUpInterval")]
        [SerializeField, Min(0f)] private float pointerUpInterval = 0.2f;
        [SerializeField] private bool _debug;

        [Header("Events with PointerEventData argument")]
        public UnityEvent<PointerEventData> onPointerDownEventData = new();
        public UnityEvent<PointerEventData> onPointerUpEventData = new();
        public UnityEvent<PointerEventData> onBeginDragEventData = new();
        public UnityEvent<PointerEventData> onDragEventData = new();
        public UnityEvent<PointerEventData> onEndDragEventData = new();

        [Space]
        [Header("Events with empty arguments")]
        public UnityEvent onPointerDown = new();
        public UnityEvent onPointerUp = new();
        public UnityEvent onBeginDrag = new();
        public UnityEvent onDrag = new();
        public UnityEvent onEndDrag = new();

        private readonly Subject<PointerEventData> _pointerDownSubject = new();
        private readonly Subject<PointerEventData> _pointerUpSubject = new();
        private CompositeDisposable _compositeDisposable;

        protected abstract bool HasCompatibleCollider { get; }
        protected abstract string RequiredColliderName { get; }

        protected virtual void Awake()
        {
            if (HasCompatibleCollider)
                return;

            Debug.LogError(
                $"{GetType().Name} requires a {RequiredColliderName}.",
                this);
            enabled = false;
        }

        protected virtual void OnEnable()
        {
            _compositeDisposable = new CompositeDisposable();

            _pointerDownSubject
                .ThrottleFirst(TimeSpan.FromSeconds(pointerDownInterval))
                .Subscribe(InvokePointerDown)
                .AddTo(_compositeDisposable);

            _pointerUpSubject
                .ThrottleFirst(TimeSpan.FromSeconds(pointerUpInterval))
                .Subscribe(InvokePointerUp)
                .AddTo(_compositeDisposable);
        }

        protected virtual void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        public void OnPointerDown(PointerEventData eventData) =>
            _pointerDownSubject.OnNext(eventData);

        public void OnPointerUp(PointerEventData eventData) =>
            _pointerUpSubject.OnNext(eventData);    

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDragEventData.Invoke(eventData);
            onBeginDrag.Invoke();
            Log(nameof(OnBeginDrag));
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
            Log(nameof(OnEndDrag));
        }

        private void InvokePointerDown(PointerEventData eventData)
        {
            onPointerDownEventData.Invoke(eventData);
            onPointerDown.Invoke();
            Log(nameof(OnPointerDown));
        }

        private void InvokePointerUp(PointerEventData eventData)
        {
            onPointerUpEventData.Invoke(eventData);
            onPointerUp.Invoke();
            Log(nameof(OnPointerUp));
        }

        private void Log(string eventName)
        {
            if (_debug)
                Debug.Log($"[{GetType().Name}] {eventName}", this);
        }
    }
}
