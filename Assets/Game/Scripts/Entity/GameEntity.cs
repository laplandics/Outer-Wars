using System;
using UnityEngine;

public abstract class GameEntity : MonoBehaviour
{
    [SerializeReference][ReadOnly] public EntityData data;
    
    public virtual void Load(Action onLoad) { onLoad.Invoke(); }
    public virtual void OnAppear() {}
    public virtual void OnDisappear() {}
}

public abstract class DerivedEntity : GameEntity
{ [ReadOnly] public IMainEntity Owner; }

[Serializable]
public abstract class EntityData {}

[Serializable]
public abstract class EntityState {}

public interface IMainEntity { public GameEntity Instance { get;} }
public interface IEntityAsset {}
public interface IEntityComponent {}

public abstract class EntityPreset : ScriptableObject {}