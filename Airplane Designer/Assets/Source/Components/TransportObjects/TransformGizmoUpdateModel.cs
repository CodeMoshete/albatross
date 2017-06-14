using Enums;
using UnityEngine;

namespace Models.History
{
    public class TransformGizmoUpdateModel
    {
        public TransformGizmo PreviousGizmo { get; private set; }
        public TransformGizmo NewGizmo { get; private set; }
        
        public TransformGizmoUpdateModel(TransformGizmo previousGizmo, TransformGizmo newGizmo)
        {
            PreviousGizmo = previousGizmo;
            NewGizmo = newGizmo;
        }
    }
}