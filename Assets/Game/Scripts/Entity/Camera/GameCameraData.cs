using System;
using Unity.Cinemachine;

[Serializable]
public class GameCameraData : EntityData
{ public CameraControllerType cameraController; public LensSettings.OverrideModes cameraMode; }

public enum CameraControllerType { BattleCameraController }

[Serializable]
public class GameCameraState : EntityState
{ public CameraController cameraController; }

public abstract class CameraController : DerivedEntity, IEntityAsset {}