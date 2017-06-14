using System;
using Enums;
using Models.History;
using Utils;
using UnityEngine;
using Services;
using Events;

namespace Components
{
    public class RotationHandle : IHandleGizmo
    {
        private enum RotationAxes
        {
            XAxis,
            YAxis,
            ZAxis,
        }

        private const float OBJECT_SELECT_TIME = 0.2f;
        private const float OBJECT_FOCUS_TIME = 0.2f;

        public Transform currentSelected { get; private set; }
        private Transform gizmo;
        private bool isDraggingGizmo;
        private bool isOrbiting;
        private RotationAxes currentAxis;

        private int layerMaskGizmos;
        private int layerMaskObjects;
        private int layerGizmos;

        private Vector3 currentPlaneNormFw;
        private Vector3 currentPlaneNormRt;
        private Vector3 currentPlaneNormUp;
        private Vector3 currentPlanePt;

        private Vector3 startMouseCoords;
        private Vector3 currentMouseCoords;

        private float previousRotation;

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

            gizmo = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("RotateGizmo")).transform;
            gizmo.gameObject.SetActive(false);
        }

        /// Called when a finger touches the screen.
        public void Press(int id, Vector2 screenPosition)
        {
            Transform target = GetTargetObject(screenPosition);

            if (!target != null && target.gameObject.layer == layerGizmos)
            {
                isDraggingGizmo = true;

                currentPlanePt = currentSelected.position;
                currentPlaneNormFw = currentSelected.forward;
                currentPlaneNormRt = currentSelected.right;
                currentPlaneNormUp = currentSelected.up;

                currentAxis = (RotationAxes)Enum.Parse(typeof(RotationAxes), target.name);
                switch (currentAxis)
                {
                    case RotationAxes.XAxis:
                        startMouseCoords = currentMouseCoords = 
                            GetAxisPlaneIntersect(screenPosition, currentPlaneNormRt);
                        previousRotation = 0f;
                        break;
                    case RotationAxes.ZAxis:
                        startMouseCoords = currentMouseCoords = 
                            GetAxisPlaneIntersect(screenPosition, currentPlaneNormFw);
                        previousRotation = 0f;
                        break;
                    case RotationAxes.YAxis:
                        startMouseCoords = currentMouseCoords = 
                            GetAxisPlaneIntersect(screenPosition, currentPlaneNormUp);
                        previousRotation = 0f;
                        break;
                }
                SendTransformUpdate(target.eulerAngles);
                startPos = currentSelected.eulerAngles;
            }
        }
        
        /// Called when a previously-pressed finger moves or remains stationary.
        public void Drag(int id, Vector2 screenPositionLast, Vector2 screenPosition)
        {
            if (isDraggingGizmo)
            {
                Vector3 frameRotAmt = Vector3.zero;
                float rotAmt;

                switch (currentAxis)
                {
                    case RotationAxes.XAxis:
                        rotAmt = GetAxisRotation(screenPosition, currentPlaneNormRt);
                        frameRotAmt.x = previousRotation - rotAmt;
                        currentSelected.transform.Rotate(frameRotAmt);
                        previousRotation = rotAmt;
                        break;
                    case RotationAxes.ZAxis:
                        rotAmt = GetAxisRotation(screenPosition, currentPlaneNormFw);
                        frameRotAmt.z = previousRotation - rotAmt;
                        currentSelected.transform.Rotate(frameRotAmt);
                        previousRotation = rotAmt;
                        break;
                    case RotationAxes.YAxis:
                        rotAmt = GetAxisRotation(screenPosition, currentPlaneNormUp);
                        frameRotAmt.y = previousRotation - rotAmt;
                        currentSelected.transform.Rotate(frameRotAmt);
                        previousRotation = rotAmt;
                        break;
                }

                gizmo.transform.position = currentSelected.transform.position;
                gizmo.transform.rotation = currentSelected.rotation;
                SendTransformUpdate(currentSelected.eulerAngles);
            }
        }

        private float GetAxisRotation(Vector2 screenPosition, Vector3 normal)
        {
            currentMouseCoords = GetAxisPlaneIntersect(screenPosition, normal);
            Vector3 newDir = currentMouseCoords - currentSelected.transform.position;
            Vector3 oldDir = startMouseCoords - currentSelected.transform.position;
            Vector3 cross = Vector3.Cross(newDir, oldDir);
            float crossSign = Vector3.Dot(cross, normal);
            if (crossSign != 0f)
            {
                crossSign = (crossSign / Mathf.Abs(crossSign));
            }
            return crossSign * Vector3.Angle(oldDir, newDir);
        }

        /// Called when a finger is released from the screen.
        public void Release(int id, Vector2 screenPositionLast, Vector2 screenPosition)
        {
            Transform target = GetTargetObject(screenPosition);

            if (target != null && !isDraggingGizmo && !isOrbiting)
            {
                gizmo.gameObject.SetActive(false);
            }

            if (isOrbiting)
            {
                isOrbiting = false;
            }

            if (isDraggingGizmo)
            {
                isDraggingGizmo = false;
                gizmo.transform.rotation = currentSelected.rotation;
                SendTransformUpdate(currentSelected.eulerAngles, startPos);
            }
        }

        public void SetGizmoOnTarget(Transform target)
        {
            if (!target != null)
            {
                currentSelected = target;
                gizmo.gameObject.SetActive(true);
                gizmo.transform.position = currentSelected.transform.position;
                gizmo.transform.rotation = currentSelected.transform.rotation;

                SendTransformUpdate(currentSelected.eulerAngles);
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
            
			Service.Events.SendEvent(EventId.HistoryTransformUpdate, transUpdate);

            SendTransformUpdate(newValues);
        }

        private void SendTransformUpdate(Vector3 newValues)
        {
			Vector3 value = newValues;
			Service.Events.SendEvent(EventId.GizmoTransformUpdate, value);
        }

        private Vector3 GetIntersect(Vector2 screenPosition, Vector3 planeNormal)
        {
            Vector3 intersect = Vector3.zero;
			Ray ray = Service.Cameras.Camera.ScreenPointToRay(screenPosition);
            MathEditorUtils.LinePlaneIntersection(out intersect, ray.origin, 
                                            ray.direction, planeNormal, 
                                            currentSelected.position);
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
            if (Input.GetKeyDown(KeyCode.LeftControl))
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
			Vector3 val = (Vector3)cookie;
			startPos = currentSelected.eulerAngles;
			currentSelected.eulerAngles = val;
			gizmo.transform.position = currentSelected.transform.position;
			gizmo.transform.rotation = currentSelected.rotation;
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

        public TransformGizmo GetGizmoType()
        {
            return TransformGizmo.Rotation;
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
			Service.Events.RemoveListener(EventId.InputTransformUpdate, OnTransformUpdate);
        }
    }
}