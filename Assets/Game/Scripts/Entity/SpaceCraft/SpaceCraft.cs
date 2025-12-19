using System;
using UnityEngine;

public class SpaceCraft : GameEntity, IMainEntity
{
    public const string Label = nameof(SpaceCraft);
    [SerializeField] private SpaceCraftState state;
    private SpaceCraftLoader _loader;
    
    public GameEntity Instance => this;

    public override void Load(Action onLoad)
    {
        state = new SpaceCraftState();
        _loader = new SpaceCraftLoader(this, onLoad, data, state);
    }

    public override void OnDisappear()
    {
        _loader.Dispose();
    }
}