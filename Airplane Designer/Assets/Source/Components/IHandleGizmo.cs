using Enums;
using UnityEngine;

namespace Components
{
    public interface IHandleGizmo
    {
        void Start();

        /// Called when a finger touches the screen.
        void Press(int id, Vector2 screenPosition);
        
        /// Called when a previously-pressed finger moves or remains stationary.
        void Drag(int id, Vector2 screenPositionLast, Vector2 screenPosition);

        /// Called when a finger is released from the screen.
        void Release(int id, Vector2 screenPositionLast, Vector2 screenPosition);

        /// Tell the HandleGizmo which transform to set on.
        void SetGizmoOnTarget(Transform target);

        /// Used to see whether or not the handle gizmo gameobject is active
        bool IsActive();

        /// Used to query the current gizmo's type.
        TransformGizmo GetGizmoType();

        /// Updated by the controller class.
        void Update(float dt);

        /// Used to hide or show a given handle.
        void SetActive(bool active);

        void Unload();
    }
}