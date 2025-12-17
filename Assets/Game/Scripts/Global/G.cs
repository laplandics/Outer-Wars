using System;
using System.Collections.Generic;

public static class G
{
    private static readonly Dictionary<Type, GameState> States = new();
    private static readonly Dictionary<Type, SceneManager> Managers =  new();
    private static readonly Dictionary<Type, GameService> Services = new();

    public static void CacheGameStates(List<GameState> states)
    {
        foreach (var state in states) { States.TryAdd(state.GetType(), state); }
    }
    
    public static void CacheGameServices(List<GameService> services)
    {
        foreach (var service in services) { Services.TryAdd(service.GetType(), service); }
    }
    
    public static void CacheSceneManagers(List<SceneManager> managers)
    {
        foreach (var manager in managers) { Managers.TryAdd(manager.GetType(), manager); }
    }
    public static T GetState<T>()
    {
        if (!States.TryGetValue(typeof(T), out var state)) return default;
        return state is not T searchingState ? default : searchingState;
    }

    public static T GetService<T>()
    {
        if (!Services.TryGetValue(typeof(T), out var service)) return default;
        return service is not T searchingService ? default : searchingService;
    }
    
    public static T GetManager<T>()
    {
        if (!Managers.TryGetValue(typeof(T), out var manager)) return default;
        return manager is not T searchingManager ? default : searchingManager;
    }

    public static void ResetData() { Managers.Clear(); Services.Clear(); States.Clear(); }
}