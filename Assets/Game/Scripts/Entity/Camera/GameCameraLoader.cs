using System;

public class GameCameraLoader : EntityLoader
{
    public GameCameraLoader(IMainEntity owner, Action onLoad, EntityData data, GameCameraState state) : base(owner, onLoad, data)
    {
        if (data is not GameCameraData gameCameraData) return;
        Action<CameraController> callback = controller =>
            { state.cameraController = controller; onLoad?.Invoke(); };
        LoadEntityAsset(callback, nameof(GameCamera), gameCameraData.cameraController.ToString());
    }
}