using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName =  "SceneStatus", menuName = "States/SceneStatus")]
public class SceneStatus : GameState
{
    [SerializeField] private bool isSceneStarted;
    public bool IsSceneStarted { get => isSceneStarted; private set => isSceneStarted = value; }

    public override IEnumerator Load()
    {
        Eventer.Subscribe<SceneStarted>(SetSceneStarted);
        Eventer.Subscribe<SceneEnded>(SetSceneEnded);
        yield break;
    }

    private void SetSceneStarted(SceneStarted _) { IsSceneStarted = true; }

    private void SetSceneEnded(SceneEnded obj) { IsSceneStarted = false; }

    public override void Unload()
    {
        Eventer.Unsubscribe<SceneStarted>(SetSceneStarted);
        Eventer.Unsubscribe<SceneEnded>(SetSceneEnded);
    }
}
