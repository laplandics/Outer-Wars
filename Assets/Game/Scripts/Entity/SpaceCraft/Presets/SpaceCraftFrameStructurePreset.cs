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

    public SpaceCraftFrameStructureData GetData(FrameStructureType frameStructureType)
    {
        var dataDict = new Dictionary<FrameStructureType, SpaceCraftFrameStructureData>
        {
            [FrameStructureType.Bow] = bow,
            [FrameStructureType.Deck] = deck,
            [FrameStructureType.Keel] = keel,
            [FrameStructureType.Port] = port,
            [FrameStructureType.Starboard] = starboard,
            [FrameStructureType.Stern] = stern
        };
        return dataDict[frameStructureType];
    }
}