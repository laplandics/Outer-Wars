using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[FRACTION][CLASS]", menuName = "Presets/SpaceCraftFrameStructure")]
public class SpaceCraftFrameStructurePreset : EntityPreset
{
    [SerializeField] private SpaceCraftFrameStructureData bow;
    [SerializeField] private SpaceCraftFrameStructureData deck;
    [SerializeField] private SpaceCraftFrameStructureData keel;
    [SerializeField] private SpaceCraftFrameStructureData port;
    [SerializeField] private SpaceCraftFrameStructureData starboard;
    [SerializeField] private SpaceCraftFrameStructureData stern;

    public SpaceCraftFrameStructureData GetData(SpaceCraftFrameStructureType spaceCraftFrameStructureType)
    {
        var dataDict = new Dictionary<SpaceCraftFrameStructureType, SpaceCraftFrameStructureData>
        {
            [SpaceCraftFrameStructureType.Bow] = bow,
            [SpaceCraftFrameStructureType.Deck] = deck,
            [SpaceCraftFrameStructureType.Keel] = keel,
            [SpaceCraftFrameStructureType.Port] = port,
            [SpaceCraftFrameStructureType.Starboard] = starboard,
            [SpaceCraftFrameStructureType.Stern] = stern
        };
        return dataDict[spaceCraftFrameStructureType];
    }
}