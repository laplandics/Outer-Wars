using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEntities", menuName = "States/GameEntities")]
public class GameEntities : GameState
{
    public Action<bool, EntityConfig> OnLoadStatusChanged;
    public Action<bool, EntityConfig> OnActiveStatusChanged;
    
    [SerializeReference] private List<EntityConfig> loadedEntities;

    public override void Load() { loadedEntities = new List<EntityConfig>(); }

    public void EntityLoadStatusChanged(EntityConfig entity)
    {
        if (entity.IsLoad) loadedEntities.Add(entity);
        else loadedEntities.Remove(entity);
        OnLoadStatusChanged?.Invoke(entity.IsLoad, entity);
    }
    public void EntityActiveStatusChanged(EntityConfig entity) => OnActiveStatusChanged?.Invoke(entity.IsActive, entity);
    
    public List<EntityConfig> GetEntities() => loadedEntities;
    public EntityConfig FindEntity(string id) { return loadedEntities.FirstOrDefault(entity => entity.id == id); }
    
    public override void Unload() {loadedEntities = null;}
}