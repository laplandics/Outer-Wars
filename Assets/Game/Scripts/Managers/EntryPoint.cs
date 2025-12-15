using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameSettings[] gameSettings;
    [SerializeField] private GameState[] gameStates;
    [SerializeField] private GameService[] gameServices;
    [SerializeField] private SceneManager[] sceneManagers;
    
    private IEnumerator Start()
    {
        Application.quitting += End;
        yield return StartCoroutine(SetGameSettings());
        yield return StartCoroutine(LoadGameStates());
        yield return StartCoroutine(RunServices());
        yield return StartCoroutine(InitializeManagers());
        G.GetState<IsGameStarted>().Set(true);
    }

    private IEnumerator SetGameSettings()
    {
        var setSettings = new List<GameSettings>();
        foreach (var setting in gameSettings)
        {
            yield return StartCoroutine(setting.Set());
            setSettings.Add(setting);
        }
        G.CacheGameSettings(setSettings);
    }

    private IEnumerator LoadGameStates()
    {
        var gamesStates = new List<GameState>();
        foreach (var gameState in gameStates)
        {
            yield return StartCoroutine(gameState.Load());
            gamesStates.Add(gameState);
        }
        G.CacheGameStates(gamesStates);
    }
    
    private IEnumerator RunServices()
    {
        var runningServices = new List<GameService>();
        foreach (var service in gameServices)
        {
            yield return StartCoroutine(service.Run());
            runningServices.Add(service);
        }
        G.CacheGameServices(runningServices);
    }
    
    private IEnumerator InitializeManagers()
    {
        var initializedManagers = new List<SceneManager>();
        foreach (var manager in sceneManagers)
        {
            yield return StartCoroutine(manager.Initialize());
            initializedManagers.Add(manager);
        }
        G.CacheSceneManagers(initializedManagers);
    }

    private void End()
    {
        Application.quitting -= End;
        G.GetState<IsGameStarted>().Set(false);
        DeinitializeManagers();
        StopServices();
        UnloadStates();
        UnsetSettings();
        G.ResetData();
    }
    
    private void DeinitializeManagers()
    {
        foreach (var manager in sceneManagers) { manager.Deinitialize(); }
    }
    
    private void StopServices()
    {
        foreach (var service in gameServices) { service.Stop(); }
    }

    private void UnloadStates()
    {
        foreach (var state in gameStates) { state.Unload(); }
    }

    private void UnsetSettings()
    {
        foreach (var setting in gameSettings) { setting.Unset(); }
    }
}