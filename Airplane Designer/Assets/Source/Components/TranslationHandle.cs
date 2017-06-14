using System;
using Enums;
using Models.History;
using Utils;
using UnityEngine;
using Services;
using Events;

namespace Components
{
    public class TranslationHandle : IHandleGizmo
    {
        private enum TranslationAxes
        {
            XAxis,
            YAxis,
            ZAxis,
            XYAxis,
            YZAxis,
            XZAxis,
        }

        private const float OBJECT_SELECT_TIME = 0.2f;
        private const float OBJECT_FOCUS_TIME = 0.2f;

        public Transform currentSelected { get; private set; }
        private Transform gizmo;
        private bool isDraggingGizmo;
        private bool isOrbiting;
        private TranslationAxes currentAxis;

        private int layerMaskGizmos;
        private int layerMaskObjects;
        private int layerGizmos;

        private Vector3 currentPlaneNormFw;
        private Vector3 currentPlaneNormRt;
        private Vector3 currentPlaneNormUp;
        private Vector3 currentPlanePt;

        private bool snapDrag;
        private Vector3 currentPos;
        private Vector3 startPos;

        public void Start()
        {
            // Set up input testing layers.
            string[] gizmoLayer = new string[] { 
                EditorConstants.GIZMO_LAYER,
            };

            string[] objectLayer = new string[] { 
                EditorConstants.OBJECT_LAYER,
            };
            
            layerMaskGizmos = LayerMask.GetMask(gizmoLayer);
            layerMaskObjects = LayerMask.GetMask(objectLayer);

            layerGizmos = LayerMask.NameToLayer(EditorConstants.GIZMO_LAYER);

			Service.Events.AddListener(EventId.InputTransformUpdate, OnTransformUpdate);

            GameObject gizmoBase = Resources.Load<GameObject>("TranslateGizmo");
            gizmo = GameObject.Instantiate<GameObject>(gizmoBase).transform;
            gizmo.gameObject.SetActive(false);
        }

        /// Called when a finger touches the screen.
        public void Press(int id, Vector2 screenPosition)
        {
            Transform target = GetTargetObject(screenPosition);

			if (target != null && target.gameObject.layer == layerGizmos)
            {
                isDraggingGizmo = true;
                // Moving the objects around breaks if these values aren't cached beforehand.
                currentPlanePt = currentPos = currentSelected.position;
                currentPlaneNormFw = currentSelected.forward;
                currentPlaneNormRt = currentSelected.right;
                currentPlaneNormUp = currentSelected.up;
                currentAxis = (TranslationAxes)Enum.Parse(typeof(TranslationAxes), target.name);
                SendTransformUpdate(target.position);
                startPos = currentSelected.position;
            }
        }
        
        /// Called when a previously-pressed finger moves or remains stationary.
        public void Drag(int id, Vector2 screenPositionLast, Vector2 screenPosition)
        {
            if (isDraggingGizmo)
            {
                Vector3 normUp = snapDrag ? Vector3.up : currentPlaneNormUp;
                Vector3 xzIntersect = GetAxisPlaneIntersect(screenPosition, normUp);
                Vector3 xzLastIntersect = GetAxisPlaneIntersect(screenPositionLast, normUp);
                Vector3 xzDiff = xzIntersect - xzLastIntersect;

                Vector3 normFw = snapDrag ? Vector3.forward : currentPlaneNormFw;
                Vector3 xyIntersect = GetAxisPlaneIntersect(screenPosition, normFw);
                Vector3 xyLastIntersect = GetAxisPlaneIntersect(screenPositionLast, normFw);
                Vector3 xyDiff = xyIntersect - xyLastIntersect;

                Vector3 normRt = snapDrag ? Vector3.right : currentPlaneNormRt;
                Vector3 yzIntersect = GetAxisPlaneIntersect(screenPosition, normRt);
                Vector3 yzLastIntersect = GetAxisPlaneIntersect(screenPositionLast, normRt);
                Vector3 yzDiff = yzIntersect - yzLastIntersect;

                switch (currentAxis)
                {
                    case TranslationAxes.XAxis:
                        if (snapDrag)
                        {
                            currentPos += new Vector3(xzDiff.x, 0f, 0f);
                        }
                        else
                        {
                            Vector3 xAxisDiff = GetAxisDiff(xzIntersect, xzLastIntersect, gizmo.right, gizmo.forward);
                            currentPos += xAxisDiff;
                        }
                        break;
                    case TranslationAxes.ZAxis:
                        if (snapDrag)
                        {
                            currentPos += new Vector3(0f, 0f, xzDiff.z);
                        }
                        else
                        {
                            Vector3 zAxisDiff = GetAxisDiff(xzIntersect, xzLastIntersect, gizmo.forward, gizmo.right);
                            currentPos += zAxisDiff;
                        }
                        break;
                    case TranslationAxes.YAxis:
                        if (snapDrag)
                        {
                            currentPos += new Vector3(0f, xyDiff.y, 0f);
                        }
                        else
                        {
                            Vector3 yAxisDiff = GetAxisDiff(xyIntersect, xyLastIntersect, gizmo.up, gizmo.right);
                            currentPos += yAxisDiff;
                        }
                        break;
                    case TranslationAxes.XZAxis:
                        currentPos += new Vector3(xzDiff.x, xzDiff.y, xzDiff.z);
                        break;
                    case TranslationAxes.XYAxis:
                        currentPos += new Vector3(xyDiff.x, xyDiff.y, xyDiff.z);
                        break;
                    case TranslationAxes.YZAxis:
                        currentPos += new Vector3(yzDiff.x, yzDiff.y, yzDiff.z);
                        break;
                }

                currentSelected.position = snapDrag ? 
                                           MathEditorUtils.Vector3Round(currentPos) : 
                                           currentPos;

                gizmo.transform.position = currentSelected.transform.position;
                SendTransformUpdate(currentSelected.position);
            }
        }

        /// Called when a finger is released from the screen.
        public void Release(int id, Vector2 screenPositionLast, Vector2 screenPosition)
        {
            Transform target = GetTargetObject(screenPosition);

			if (target == null && !isDraggingGizmo && !isOrbiting)
            {
                gizmo.gameObject.SetActive(false);
            }

            if (isOrbiting)
            {
                isOrbiting = false;
            }

            if (isDraggingGizmo)
            {
                SendTransformUpdate(currentSelected.position, startPos);
                isDraggingGizmo = false;
            }
        }

        public void SetGizmoOnTarget(Transform target)
        {
			if (target != null)
            {
                currentSelected = target;
                gizmo.gameObject.SetActive(true);
                gizmo.transform.position = currentSelected.transform.position;
                if (!snapDrag)
                {
                    gizmo.transform.rotation = currentSelected.transform.rotation;
                }
                SendTransformUpdate(currentSelected.position);
            }
            else
            {
                gizmo.gameObject.SetActive(false);
            }
        }

        private void SendTransformUpdate(Vector3 newValues, Vector3 oldValues)
        {
            TransformUpdateModel transUpdate = 
                new TransformUpdateModel(currentSelected, newValues, oldValues);

			Service.Events.SendEvent (EventId.HistoryTransformUpdate, transUpdate);
            SendTransformUpdate(newValues);
        }

        private void SendTransformUpdate(Vector3 newValues)
        {
			Service.Events.SendEvent(EventId.GizmoTransformUpdate, newValues);
        }

        /// <summary>
        /// Gets the vector result of the difference between last frame's axis-aligned mouse drag
        /// position and the current frame's.
        /// </summary>
        /// <param name="mousePlanePos">Current position of the mouse on the translation 
        /// plane (xz or xy).</param>
        /// <param name="lastMousePlanePos">Last position of the mouse on the translation 
        /// plane (xz or xy)</param>
        /// <param name="axisVector">Vector pointing in the direction of the desired
        ///  translation axis.</param>
        /// <param name="perpVector">Vector pointing perpendicular to the direction of the 
        /// desired translation axis.</param>
        private Vector3 GetAxisDiff(Vector3 mousePlanePos, 
                                    Vector3 lastMousePlanePos, 
                                    Vector3 axisVector, 
                                    Vector3 perpVector)
        {
            Vector3 axisPos = GetAxisIntersect(mousePlanePos, axisVector, perpVector);
            Vector3 lastAxisPos = GetAxisIntersect(lastMousePlanePos, axisVector, perpVector);
            return axisPos - lastAxisPos;
        }

        private Vector3 GetAxisIntersect(Vector3 mousePlanePos, Vector3 axisVector, Vector3 perpVector)
        {
            Vector3 intersection1;
            Vector3 intersection2;
            MathEditorUtils.CalculateLineLineIntersection(gizmo.position, 
                gizmo.position + axisVector, 
                mousePlanePos, 
                mousePlanePos + perpVector, 
                out intersection1, 
                out intersection2);
            return intersection1;
        }

        private Vector3 GetAxisPlaneIntersect(Vector2 screenPosition, Vector3 planeNormal)
        {
            Vector3 intersect;
            Ray ray = Service.Cameras.Camera.ScreenPointToRay(screenPosition);
            MathEditorUtils.LinePlaneIntersection(out intersect, ray.origin, 
                                            ray.direction, planeNormal, 
                                            currentPlanePt);
            return intersect;
        }

        private Transform GetTargetObject(Vector2 screenPosition)
        {
            Transform returnTrans = null;
            RaycastHit hitDataGizmo;
            Ray ray = Service.Cameras.Camera.ScreenPointToRay(screenPosition);
            Physics.Raycast(ray, out hitDataGizmo, Mathf.Infinity, layerMaskGizmos);

            // Test against gizmos first. If no gizmos, select world objects
            if (hitDataGizmo.transform != null)
            {
                returnTrans = hitDataGizmo.transform;
            }
            else
            {
                RaycastHit hitDataObject;
                Physics.Raycast(ray, out hitDataObject, Mathf.Infinity, layerMaskObjects);
                returnTrans = hitDataObject.transform;
            }

            return returnTrans;
        }

        public bool IsActive()
        {
            return gizmo.gameObject.activeSelf;
        }

        public void Update(float dt)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                isOrbiting = true;
            }

            if (gizmo.gameObject.activeSelf)
            {
                ScaleGizmoForDistance();
            }
        }

		private void OnTransformUpdate(object cookie)
		{
			UpdateFromInput((Vector3)cookie);
		}

        private void UpdateFromInput(Vector3 value)
        {
            startPos = currentSelected.position;
            currentPos = value;
            if (snapDrag)
            {
                currentSelected.position = MathEditorUtils.Vector3Round(currentPos);
            }
            else
            {
                currentSelected.position = currentPos;
            }
            gizmo.transform.position = currentSelected.position;
        }

        public TransformGizmo GetGizmoType()
        {
            return TransformGizmo.Translation;
        }

        private void ScaleGizmoForDistance()
        {
			float fov = Service.Cameras.Camera.fieldOfView / 2f;
            float dst = Vector3.Distance(Service.Cameras.Camera.transform.position, gizmo.position);
            float defaultSize = Mathf.Sin(fov * Mathf.Deg2Rad) * EditorConstants.GIZMO_REFERENCE_DIST;
            float defaultGizmoSize = defaultSize * EditorConstants.GIZMO_RELATIVE_SCALE;
            float trueSize = Mathf.Sin(fov * Mathf.Deg2Rad) * dst;
            float scale = trueSize / defaultSize;
            float gizmoScale = (defaultGizmoSize * scale);
            gizmo.localScale = new Vector3(gizmoScale, gizmoScale, gizmoScale);
        }

        public void SetActive(bool active)
        {
            gizmo.gameObject.SetActive(active);
        }

        public void Unload()
        {
            GameObject.Destroy(gizmo.gameObject);
			Service.Events.RemoveListener (EventId.InputTransformUpdate, OnTransformUpdate);
        }
    }
}
