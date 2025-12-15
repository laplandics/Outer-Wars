using System.Collections;
using UnityEngine;

public abstract class GameSettings : ScriptableObject
{
    public abstract IEnumerator Set();
    public abstract void Unset();
}