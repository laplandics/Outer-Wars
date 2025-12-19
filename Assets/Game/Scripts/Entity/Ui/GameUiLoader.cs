using System;

public class GameUiLoader : EntityLoader
{
    public GameUiLoader
        (IMainEntity owner, Action onLoad, EntityData data, GameUiState state) : base(owner, onLoad, data)
    {
        var uiData = (GameUiData)data;
        Action<UiHandler> callback = handler => { state.uiHandler = handler; onLoad.Invoke(); };
        LoadEntityAsset(callback, nameof(GameUi), uiData.uiHandler.ToString());
    }
}

[Serializable]
public class GameUiData : EntityData
{ public UiHandlerType uiHandler; }
public enum UiHandlerType { BattleUiHandler }

[Serializable]
public class GameUiState : EntityState
{ public UiHandler uiHandler; }

public abstract class UiHandler : DerivedEntity, IEntityAsset {}