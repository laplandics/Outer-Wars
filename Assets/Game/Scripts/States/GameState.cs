using System.Collections;
using UnityEngine;

public abstract class GameState : ScriptableObject
{
    public virtual IEnumerator Load() { yield break; }
    public virtual void Unload() {}
}