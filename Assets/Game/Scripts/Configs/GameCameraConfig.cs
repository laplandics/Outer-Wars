using System;
using UnityEngine;

[Serializable]
public class GameCameraConfig : EntityConfig
{
    [SerializeReference] public GameCameraData data;

    public GameCameraConfig(Action onLoad, GameCameraData newData)
    {
        data = newData;
        id = data.id;
        foreach (var component in data.components) { Handles.Add(LoadAsset<GameObject>(component.ToString())); }
        G.GetManager<RoutineManager>().StartRoutine(WaitForLoad(onLoad));
    }

    public override void Spawn() => SpawnInstance<GameCamera>(id);
}