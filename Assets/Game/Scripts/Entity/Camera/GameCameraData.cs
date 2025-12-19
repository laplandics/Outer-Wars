using System;

[Serializable]
public class GameCameraData : EntityData
{ public CameraController cameraController; public enum  CameraController { BattleCameraController } }

[Serializable]
public class GameCameraState : EntityState
{ public CameraController cameraController; }

public abstract class CameraController : DerivedEntity, IEntityAsset {}