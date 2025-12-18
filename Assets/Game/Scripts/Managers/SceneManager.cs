using System.Collections;
using UnityEngine;

public abstract class SceneManager : MonoBehaviour
{
    public abstract IEnumerator Initialize();
    public abstract void Deinitialize();
}