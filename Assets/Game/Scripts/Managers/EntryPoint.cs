using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private AssetLabelReference statesLabel;
    [SerializeField] private AssetLabelReference servicesLabel;
    [SerializeField] private AssetLabelReference managersLabel;
    [SerializeField] private Transform managersContainer;
    
    private AsyncOperationHandle<IList<GameState>> _statesHandle;
    private AsyncOperationHandle<IList<GameService>> _servicesHandle;
    private AsyncOperationHandle<IList<GameObject>> _managersHandle;
    
    private readonly List<GameState> _gameStates = new();
    private readonly List<GameService> _gameServices = new();
    private readonly List<SceneManager> _sceneManagers = new();
    
    private readonly List<GameObject> _managersPrefabs = new();

    private IEnumerator Start()
    {
        Application.quitting += End;
        LoadAssets();
        yield return _statesHandle;
        yield return _servicesHandle;
        yield return _managersHandle;
        yield return LoadGameStates();
        yield return RunServices();
        yield return InitializeManagers();
        Eventer.Invoke(new SceneStarted());
    }

    private void LoadAssets()
    {
        _statesHandle = Addressables.LoadAssetsAsync<GameState>(statesLabel.RuntimeKey, state => _gameStates.Add(state));
        _servicesHandle = Addressables.LoadAssetsAsync<GameService>(servicesLabel.RuntimeKey, service => _gameServices.Add(service));
        _managersHandle = Addressables.LoadAssetsAsync<GameObject>(managersLabel.RuntimeKey, manager => _managersPrefabs.Add(manager));
    }

    private IEnumerator LoadGameStates()
    {
        foreach (var gameState in _gameStates) { yield return gameState.Load(); }
        G.CacheGameStates(_gameStates);
        Eventer.Invoke(new StatesLoaded());
    }
    
    private IEnumerator RunServices()
    {
        foreach (var gameService in _gameServices) { yield return gameService.Run(); }
        G.CacheGameServices(_gameServices);
        Eventer.Invoke(new ServicesLaunched());
    }
    
    private IEnumerator InitializeManagers()
    {
        foreach (var prefab in _managersPrefabs)
        {
            _sceneManagers.Add(Spawner.Spawn(prefab, Vector3.zero, Quaternion.identity, managersContainer).GetComponent<SceneManager>());
            _sceneManagers[^1].gameObject.name = prefab.name;
            yield return _sceneManagers[^1].Initialize();
        }
        _managersPrefabs.Clear();
        G.CacheSceneManagers(_sceneManagers);
        Eventer.Invoke(new ManagersInitialized());
    }

    private void End()
    {
        Eventer.Invoke(new SceneEnded());
        Application.quitting -= End;
        DeinitializeManagers();
        StopServices();
        UnloadStates();
        Eventer.ClearSubscribers();
        G.ResetData();
    }
    
    private void DeinitializeManagers()
    {
        foreach (var manager in _sceneManagers) { manager.Deinitialize(); }
        _sceneManagers.Clear();
        _managersHandle.Release();
        Eventer.Invoke(new ManagersDeinitialized());
    }
    
    private void StopServices()
    {
        foreach (var service in _gameServices) { service.Stop(); }
        _gameServices.Clear();
        _servicesHandle.Release();
        Eventer.Invoke(new ServicesStopped());
    }

    private void UnloadStates()
    {
        foreach (var state in _gameStates) { state.Unload(); }
        _gameStates.Clear();
        _statesHandle.Release();
        Eventer.Invoke(new StatesUnloaded());
    }
}