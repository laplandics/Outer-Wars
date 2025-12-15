using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "IsGameStarted", menuName = "States/IsGameStarted")]
public class IsGameStarted : GameState<bool>
{
    public override IEnumerator Load()
    {
        Value = false;
        yield break;
    }

    public override void Unload()
    {
        Value = false;
    }
}