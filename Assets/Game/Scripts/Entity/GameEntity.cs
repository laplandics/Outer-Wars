using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class GameEntity : MonoBehaviour
{
    [SerializeReference] public EntityData data;
    
    public virtual void Load(Action onLoad) { onLoad.Invoke(); }
    public virtual void OnDisappear() {}

    protected void LoadAsset<T>(Action<T> callback, string label, string componentName, EntityData componentData = null) where T : EntityComponent
    {
        T instance;
        Addressables.LoadAssetsAsync<GameObject>(label, prefab =>
        {
            var component = prefab.GetComponent<T>();
            if (component == null) return;
            if (component.GetType().Name != componentName) return;
            instance = Spawner.Spawn(prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<T>();
            instance.name = prefab.name;
            instance.owner = this;
            if (componentData != null) instance.data = componentData;
            instance.Load(() => callback.Invoke(instance));
        }).Completed += h =>
        {
            if (h.Status != AsyncOperationStatus.Succeeded) throw h.OperationException;
            h.Release();
        };
    }
}

public abstract class EntityComponent : GameEntity
{
    [SerializeReference][ReadOnly] public GameEntity owner;
}

[Serializable]
public abstract class EntityData { public string entityName; }

[Serializable]
public abstract class EntityState {}