using System;
using System.Collections.Generic;

public abstract class SpaceCraftWeapon : DerivedEntity, IEntityAsset
{
    [ReadOnly] public SpaceCraftAmmo currentAmmo;

    private SpaceCraftWeaponLoader _loader;
    public override void Load(Action onLoad) { _loader = new SpaceCraftWeaponLoader(Owner, onLoad, data, this); }

    public override void OnDisappear() { _loader.Dispose(); }
}