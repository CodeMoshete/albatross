namespace Events
{
	public enum EventId
	{
		ApplicationExit,
		NetPlayerDisconnected,
		NetPlayerConnected,
		NetPlayerIdentified,
		PlayerAimed,
		EntitySpawned,
		EntityDestroyed,
		EntitySelected,
		EntityHealthUpdate,
		EntityMoved,
		EntityFired,
		EntityFiredLocal,
		EntityTransformUpdated,
		GizmoTransformUpdate,
		HistoryTransformUpdate,
		InputTransformUpdate,
		VRHandCollisionEnter,
		VRHandCollisionExit,
		VRControllerPulse,
		VRControllerTriggerPress,
		VRControllerTriggerRelease,
		VRControllerTouchpadPress,
		VRControllerTouchpadDrag,
		VRControllerTouchpadRelease,
		DebugCameraControlsActive,
		CreateButtonPressed,
		CreationComplete
	}
}
