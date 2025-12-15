using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : SceneManager
{
    [SerializeField] private Transform target;
    [SerializeField] private CinemachineCamera mainCamera;
    private CameraMover _cameraMover;
    
    public override IEnumerator Initialize()
    {
        G.GetState<IsGameStarted>().OnChange += OnGameStart;
        yield break;
    }

    private void OnGameStart(bool gameStarted)
    {
        if(!gameStarted) return;
        var gameInputs = new GameInputs();
        _cameraMover = new CameraMover(gameInputs, mainCamera, target);
        G.GetState<IsGameStarted>().OnChange -= OnGameStart;
    }

    public override void Deinitialize()
    {
        G.GetState<IsGameStarted>().OnChange -= OnGameStart;
        _cameraMover.Dispose();
        _cameraMover = null;
    }
}