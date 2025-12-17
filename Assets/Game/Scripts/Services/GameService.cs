using UnityEngine;

public abstract class GameService : ScriptableObject
{
    public abstract void Run();
    public abstract void Stop();
}