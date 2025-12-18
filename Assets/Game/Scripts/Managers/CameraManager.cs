using System.Collections;
using UnityEngine;

public class CameraManager : SceneManager
{
    [SerializeField] private GameCameraData cameraData;
    private GameCamera _cameraEntity;
    
    public override IEnumerator Initialize()
    {
        Eventer.Subscribe<ManagersInitialized>(SpawnCamera);
        yield break;
    }

    private void SpawnCamera(ManagersInitialized _)
    {
        _cameraEntity = E.NewEntity<GameCamera>(cameraData);
        _cameraEntity.Load(() => {});
    }


    public override void Deinitialize()
    {
        Eventer.Unsubscribe<ManagersInitialized>(SpawnCamera);
        E.DeleteEntity(_cameraEntity);
    }
}