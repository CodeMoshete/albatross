using UnityEngine;

namespace Models.History
{
    public class TransformSelectModel
    {
        public Transform NewModel { get; private set; }
        public Transform PreviousModel { get; private set; }
        
        public TransformSelectModel(Transform previousModel, Transform newModel)
        {
            NewModel = newModel;
            PreviousModel = previousModel;
        }
    }
}