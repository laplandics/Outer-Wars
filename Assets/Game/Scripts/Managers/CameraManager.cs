using GlobalEvents;
using UnityEngine;

public class CameraManager : SceneManager
{
    [SerializeField] private GameCameraData data;
    private GameCameraConfig _config;
    
    public override void Initialize()
    {
        Eventer.Subscribe<ManagersInitialized>(InitializeCamera);
    }

    private void InitializeCamera(ManagersInitialized _)
    {
        _config = new GameCameraConfig(OnLoad, data);
    }

    private void OnLoad() { _config.Spawn(); }

    public override void Deinitialize()
    {
        Eventer.Unsubscribe<ManagersInitialized>(InitializeCamera);
        _config.Despawn();
        _config.DeleteConfig();
        _config = null;
    }
}