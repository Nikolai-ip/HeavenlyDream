using UnityEngine;

namespace _Game.Source.Infrastructure.Input.Pointer
{
    /// <summary>
    /// Conditions for correct work:
    /// Object has a Collider2D component.
    /// Camera has Physics 2D Raycaster component.
    /// Project has an EventSystem object.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PointerInteractionListener : PointerInteractionListenerBase
    {
        protected override bool HasCompatibleCollider =>
            TryGetComponent<Collider2D>(out _);

        protected override string RequiredColliderName => nameof(Collider2D);
    }
}