using System;
using System.Collections.Generic;

public abstract class SpaceCraftFrameStructure : DerivedEntity, IEntityAsset
{
    public List<SpaceCraftFrameStructureWeaponSpotPosition> weaponSpotPositions = new(); 
    [ReadOnly] public BaseStats baseStats;
    [ReadOnly] public CurrentStats currentStats;

    private SpaceCraftFrameStructureLoader _loader;
    
    public override void Load(Action onLoad) { _loader = new SpaceCraftFrameStructureLoader(Owner, onLoad, data, this); }

    public override void OnDisappear() { _loader.Dispose(); }

    [Serializable]
    public class BaseStats { public int baseArmor; public int baseShield; public int baseHealth; }
    [Serializable]
    public class CurrentStats { public int currentArmor; public int currentShield; public int currentHealth; }
}