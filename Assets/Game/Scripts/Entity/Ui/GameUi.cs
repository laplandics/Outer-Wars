using System;
using UnityEngine;

public class GameUi : GameEntity, IMainEntity
{
    [SerializeField] private GameUiState state;
    private GameUiLoader _loader;
    
    public GameEntity Instance => this;
    
    public override void Load(Action onLoad)
    {
        state = new GameUiState();
        _loader = new GameUiLoader(this, onLoad, data, state);
    }
    
    public override void OnDisappear() { _loader.Dispose(); }
}

