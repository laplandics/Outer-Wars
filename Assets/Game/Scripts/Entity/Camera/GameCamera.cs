using System;
using UnityEngine;

public class GameCamera : GameEntity, IMainEntity
{
    [SerializeField] private GameCameraState state;
    private GameCameraData _data;
    private GameCameraLoader _loader;

    public EntityState State => state;
    public EntityData Data => _data;
    public GameEntity Instance => this;

    public override void Load(Action onLoad)
    {
        state = new GameCameraState();
        _loader = new GameCameraLoader(this, onLoad, data, state);
    }
}