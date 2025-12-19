using System;
using UnityEngine;

public class SpaceCraftWeaponSpot : DerivedEntity, IEntityAsset
{
    public SpaceCraftSpotType spotType;
    public SpaceCraftWeapon weapon;
    
    [HideInInspector] public FrameStructureType spotStructureType;
    private SpaceCraftWeaponSpotLoader _loader;
    
    public override void Load(Action onLoad) { _loader = new SpaceCraftWeaponSpotLoader(Owner, this, onLoad, data); }

    public override void OnDisappear() { _loader.Dispose(); }
}