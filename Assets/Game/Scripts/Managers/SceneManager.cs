using System.Collections;
using UnityEngine;

public abstract class SceneManager : MonoBehaviour
{
    public abstract void Initialize();
    public abstract void Deinitialize();
}