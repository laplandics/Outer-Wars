using System.Collections.Generic;
using UnityEngine;

public abstract class GameEntity : MonoBehaviour
{
    public virtual void OnAppear() {}
    public virtual void OnDisappear() {}
}

public abstract class EntityInstance : GameEntity
{
    [SerializeReference] public EntityConfig config;
    [SerializeReference] public List<EntityComponent> components;

    public TComponent GetEntityComponent<TComponent>() where TComponent : EntityComponent
    {
        foreach (var component in components) {if(component is TComponent entityComponent) return entityComponent;}
        return null;
    }
}

public abstract class EntityComponent : GameEntity
{
    [SerializeReference] public EntityInstance instance;
}