using System;
using UnityEngine;

public class GameCamera : GameEntity
{
    [SerializeField] private GameCameraState state;
    private GameCameraData _data;

    public override void Load(Action onLoad)
    {
        if (data is not GameCameraData gameCameraData) return;
        _data = gameCameraData;
        state = new GameCameraState();
        Action<CameraController> callback = controller =>
            { state.cameraController = controller; onLoad?.Invoke(); };
        LoadAsset(callback, nameof(GameCamera), _data.cameraController.ToString());
    }
}

[Serializable]
public class GameCameraData : EntityData
{
    public CameraController cameraController;
    public enum  CameraController { None, BattleCameraController }
}

[Serializable]
public class GameCameraState : EntityState
{
    public CameraController cameraController;
}

public abstract class CameraController : EntityComponent {}