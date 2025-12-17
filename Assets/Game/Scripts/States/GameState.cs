using UnityEngine;

public abstract class GameState : ScriptableObject
{
    public virtual void Load() {}
    public virtual void Unload() {}
}