using UnityEngine;

namespace _Game.Source.Infrastructure.Input.Pointer
{
    /// <summary>
    /// Receives pointer events for a GameObject with a 3D Collider.
    /// An EventSystem must exist in the scene and the event camera must have
    /// a PhysicsRaycaster component.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PointerInteractionListener3D : PointerInteractionListenerBase
    {
        protected override bool HasCompatibleCollider =>
            TryGetComponent<Collider>(out _);

        protected override string RequiredColliderName => nameof(Collider);
    }
}
