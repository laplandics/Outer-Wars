using UnityEngine;
using GlobalEvents;

public enum SceneType { Menu, Battle, Explore }

[CreateAssetMenu(fileName = "SceneStatus", menuName = "States/SceneStatus")]
public class SceneStatus : GameState
{
    private bool _isSceneStarted;
    private bool _isScenePaused;

    public bool LifeStatus
    {
        get => _isSceneStarted;
        private set { _isSceneStarted = value; Eventer.Invoke(new SceneStarted()); }
    }

    public bool PauseStatus
    {
        get => _isScenePaused;
        private set { _isScenePaused = value; Eventer.Invoke(new SceneEnded()); }
    }
    
    public void StartScene() => LifeStatus = true;
    public void EndScene() => LifeStatus = false;
    
    public void PauseScene() => PauseStatus = true;
    public void ResumeScene() => PauseStatus = false;
}