using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentEntities", menuName = "States/CurrentEntities")]
public class CurrentEntities: GameState<EntityConfig>
{
    [SerializeField] private List<EntityConfig> entities = new();

    public override IEnumerator Load()
    {
        Value = null;
        yield break;
    }

    public override void Set(EntityConfig newValue)
    {
        entities.Add(newValue);
        base.Set(newValue);
    }

    public override void Unload()
    {
        entities.Clear();
        Value = null;
    }
}