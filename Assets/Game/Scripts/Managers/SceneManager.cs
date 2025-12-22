using System.Collections;
using UnityEngine;

public abstract class SceneManager : MonoBehaviour
{
    public virtual IEnumerator OnInitialize() { yield break; }
    public virtual IEnumerator OnStart() { yield break; }
    public virtual void OnEnd() { }
    public virtual void OnDeinitialize() { }
}