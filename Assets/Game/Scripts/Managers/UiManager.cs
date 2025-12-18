using System.Collections;
using UnityEngine;

public class UiManager : SceneManager
{
    [SerializeField] private GameUiData uiData;
    private GameUi _uiEntity;

    public override IEnumerator Initialize()
    {
        Eventer.Subscribe<ManagersInitialized>(SpawnUi);
        yield break;
    }

    private void SpawnUi(ManagersInitialized _)
    {
        _uiEntity = E.NewEntity<GameUi>(uiData);
        _uiEntity.Load(() => {});
    }

    public override void Deinitialize()
    {
        Eventer.Unsubscribe<ManagersInitialized>(SpawnUi);
        E.DeleteEntity(_uiEntity);
    }
}