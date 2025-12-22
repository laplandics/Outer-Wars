using System.Collections;
using UnityEngine;

public class UiManager : SceneManager
{
    [SerializeField] private GameUiData uiData;
    private GameUi _uiEntity;

    public override IEnumerator OnStart()
    {
        _uiEntity = E.NewEntity<GameUi>(uiData);
        _uiEntity.Load(() => {});
        yield break;
    }

    public override void OnDeinitialize() { E.DeleteEntity(_uiEntity); }
}