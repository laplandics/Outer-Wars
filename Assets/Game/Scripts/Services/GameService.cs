using System.Collections;
using UnityEngine;

public abstract class GameService : ScriptableObject
{
    public abstract IEnumerator Run();
    public abstract void Stop();
}