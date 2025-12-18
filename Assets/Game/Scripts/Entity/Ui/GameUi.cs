using System;
using UnityEngine;

public class GameUi : GameEntity
{
    [SerializeField] private GameUiState state;
    private GameUiData _data;
    
    public override void Load(Action onLoad)
    {
        if (data is not GameUiData uiData) return;
        _data = uiData;
        state = new GameUiState();
        Action<UiHandler> callback = handler =>
            { state.uiHandler = handler; onLoad.Invoke(); };
        LoadAsset(callback, nameof(GameUi), _data.uiHandler.ToString());
    }
}

[Serializable]
public class GameUiData : EntityData
{
    public UiHandlerType uiHandler;
}
public enum UiHandlerType { None, BattleUiHandler }

[Serializable]
public class GameUiState : EntityState
{
    public UiHandler uiHandler;
}

public abstract class UiHandler : EntityComponent {}

