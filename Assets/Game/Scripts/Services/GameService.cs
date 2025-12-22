using System.Collections;
using UnityEngine;

public abstract class GameService : ScriptableObject
{
    public virtual IEnumerator Run() { yield break; }
    public virtual IEnumerator OnStart() { yield break; }
    public virtual void OnEnd() {}
    public virtual void Stop() {}
}