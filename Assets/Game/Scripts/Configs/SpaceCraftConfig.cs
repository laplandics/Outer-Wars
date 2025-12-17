using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SpaceCraftConfig : EntityConfig
{
    [SerializeReference] public SpaceCraftData data;
    
    private Action _onLoad;
    
    public SpaceCraftConfig(Action onLoad, SpaceCraftData newData)
    {
        
    }

    private IEnumerator WaitForLoad()
    {
        yield break;
    }

    public override void Spawn()
    {
        
    }
}