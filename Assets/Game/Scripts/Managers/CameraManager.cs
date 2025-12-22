using System.Collections;
using UnityEngine;

public class CameraManager : SceneManager
{
    [SerializeField] private GameCameraData cameraData;
    private GameCamera _cameraEntity;
    
    public override IEnumerator OnStart()
    {
        _cameraEntity = E.NewEntity<GameCamera>(cameraData);
        _cameraEntity.Load(() => {});
        yield break;
    }

    public override void OnEnd() { E.DeleteEntity(_cameraEntity); }
}