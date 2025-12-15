using System.Collections;
using UnityEngine;

public class CurrentBattleUi : GameState<Canvas>
{
    public override IEnumerator Load()
    {
        Value = null; 
        yield break;
    }

    public override void Unload()
    {
        Value = null; 
    }
}