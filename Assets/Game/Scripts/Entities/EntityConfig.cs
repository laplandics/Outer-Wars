using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

[Serializable]
public class EntityConfig
{
    public string ID { get; private set; }
    public GameEntity Instance { get; private set; }
    public EntityData Data { get; private set; }
    
    public IEnumerator Spawn<T>(Action<EntityConfig> callback) where T : GameEntity
    {
        var type = typeof(T);
        var label = type.Name;
        var assetsLoaded = false;
        Addressables.LoadAssetsAsync<EntityData>(label).Completed += handle =>
        {
            Data = handle.Result[Random.Range(0, handle.Result.Count)];
            assetsLoaded = true;
        };
        yield return new WaitUntil(() => assetsLoaded);
        Instance = Spawner.Spawn(Data.prefab, Vector3.zero, Quaternion.identity).GetComponent<T>();
        G.GetState<CurrentEntities>().Set(this);
        Instance.OnSpawn();
        callback.Invoke(this);
    }

    public void Despawn()
    {
        if(Instance == null) return;
        Instance.OnDespawn();
        Spawner.Despawn(Instance.gameObject);
        Instance = null;
        Data = null;
    }
}