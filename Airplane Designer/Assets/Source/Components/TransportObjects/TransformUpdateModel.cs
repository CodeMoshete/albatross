using UnityEngine;

namespace Models.History
{
    public class TransformUpdateModel
    {
        public Vector3 NewValues;
        public Vector3 OriginalValues;
        public Transform Model;
        
        public TransformUpdateModel(Transform model, Vector3 newValues, Vector3 originalValues)
        {
            Model = model;
            NewValues = newValues;
            OriginalValues = originalValues;
        }
    }
}