using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public abstract class EntityConfig
{
    public string id;
    protected EntityInstance Instance;
    protected List<AsyncOperationHandle<GameObject>> Handles = new();
    protected List<GameObject> ComponentsPrefabs = new();
    
    protected AsyncOperationHandle<T> LoadAsset<T>(string assetName)
    {
        var handle = Addressables.LoadAssetAsync<T>(assetName);
        handle.Completed += h => { if (handle.Status != AsyncOperationStatus.Succeeded) throw h.OperationException; };
        return handle;
    }

    protected IEnumerator WaitForLoad(Action onLoad)
    {
        foreach (var handle in Handles) {yield return handle; ComponentsPrefabs.Add(handle.Result);}
        IsLoad = true;
        onLoad.Invoke();
    }

    protected void SpawnInstance<T>(string name) where T : EntityInstance
    {
        var instance = new GameObject(name).AddComponent<T>();
        instance.config = this;
        instance.components = new List<EntityComponent>();
        Instance = instance;
        foreach (var componentPrefab in ComponentsPrefabs)
        {
            var pos = Vector3.zero;
            var rot = Quaternion.identity;
            var parent = Instance.transform;
            var component = Spawner.Spawn(componentPrefab, pos, rot, parent).GetComponent<EntityComponent>();
            component.name = componentPrefab.name;
            component.instance = Instance;
            Instance.components.Add(component);
        }
        foreach (var component in Instance.components) component.OnAppear();
        Instance.OnAppear();
        IsActive = true;
    }
    
    public virtual void Despawn()
    {
        if (!IsActive) return;
        IsActive = false;
        Instance.OnDisappear();
        foreach (var component in Instance.components) component.OnDisappear();
        Spawner.Despawn(Instance.gameObject);
        Instance = null;
        ComponentsPrefabs.Clear();
    }

    public void DeleteConfig()
    {
        IsLoad = false;
        Despawn();
        foreach (var handle in Handles) handle.Release();
        Handles.Clear();
    }
    
    private bool _isLoad;
    public bool IsLoad { get => _isLoad; protected set { _isLoad = value; G.GetState<GameEntities>().EntityLoadStatusChanged(this); } }
    private bool _isActive;
    public bool IsActive { get => _isActive; protected set { _isActive = value; G.GetState<GameEntities>().EntityActiveStatusChanged(this); } }
    public abstract void Spawn();
}