using System;
using System.Collections;
using UnityEngine;

public abstract class GameState : ScriptableObject
{
    public abstract IEnumerator Load();
    public abstract void Unload();
}

public abstract class GameState<T> : GameState
{
    protected T Value;
    public Action<T> OnChange;
    
    public virtual void Set(T newValue) { Value = newValue; OnChange?.Invoke(Value); }
    
}