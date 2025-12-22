using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class EntityLoader : IDisposable
{
    protected Action OnLoad;
    protected EntityData Data;
    protected IMainEntity Owner;
    
    protected EntityLoader(IMainEntity owner, Action onLoad, EntityData data)
    { Owner = owner; OnLoad = onLoad; Data = data; }
    
    protected void LoadEntityAsset<T>(Action<T> callback, string label, string assetName, EntityData componentData = null)
    where T : DerivedEntity, IEntityAsset
    {
        T instance = null;
        var handle = Addressables.LoadAssetsAsync<GameObject>(label);
        handle.Completed += h =>
        {
            if (h.Status != AsyncOperationStatus.Succeeded) throw h.OperationException;
            var prefabs = handle.Result;
            foreach (var prefab in prefabs)
            {
                if (prefab.name != assetName) continue;
                instance = Spawner.Spawn(prefab, Vector3.zero, Quaternion.identity, Owner.Instance.transform).GetComponent<T>();
                if (instance == null) throw new Exception($"Prefab {prefab.name} has no component of type {typeof(T).Name}");
                instance.name = prefab.name;
                instance.Owner = Owner;
                if (componentData != null) instance.data = componentData;
                break;
            }

            if (instance == null) throw new Exception($"There is no prefab with tag {label} and name {assetName} of type {typeof(T).Name}");
            instance.Load(() => callback.Invoke(instance));
            h.Release();
        };
    }
    
    protected void LoadEntityComponent<T>(Action<T> callback, string componentName, EntityData componentData = null)
    where T : DerivedEntity, IEntityComponent
    {
        var componentType = Type.GetType(componentName);
        if (componentType == null) throw new TypeLoadException($"Cannot find component {componentName}");
        var component = (T)Owner.Instance.gameObject.AddComponent(componentType);
        component.Owner = Owner;
        if (componentData != null) component.data = componentData;
        component.Load(() => callback.Invoke(component));
    }

    public virtual void Dispose() { OnLoad = null; Data = null; Owner = null; }
}