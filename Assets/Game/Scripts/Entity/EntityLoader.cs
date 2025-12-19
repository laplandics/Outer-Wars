using System;
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
    
    protected void LoadEntityAsset<T>
        (Action<T> callback, string label, string assetName, EntityData componentData = null) where T : DerivedEntity, IEntityAsset
    {
        T instance;
        Addressables.LoadAssetsAsync<GameObject>(label, prefab =>
        {
            if (prefab.name != assetName) return;
            instance = Spawner.Spawn(prefab, Vector3.zero, Quaternion.identity, Owner.Instance.transform).GetComponent<T>();
            instance.name = prefab.name;
            instance.Owner = Owner;
            if (componentData != null) instance.data = componentData;
            instance.Load(() => callback.Invoke(instance));
        }).Completed += h =>
        {
            if (h.Status != AsyncOperationStatus.Succeeded) throw h.OperationException;
            h.Release();
        };
    }
    
    protected void LoadEntityComponent<T>
        (Action<T> callback, string componentName, EntityData componentData = null) where T : DerivedEntity, IEntityComponent
    {
        var componentType = Type.GetType(componentName);
        if (componentType == null) return;
        var component = (T)Owner.Instance.gameObject.AddComponent(componentType);
        component.Owner = Owner;
        if (componentData != null) component.data = componentData;
        component.Load(() => callback.Invoke(component));
    }

    public virtual void Dispose() { OnLoad = null; Data = null; Owner = null; }
}