using UnityEngine;

public class SpaceCraftAmmo : DerivedEntity, IEntityAsset
{
    [SerializeField] private SpaceCraftAmmoDamage[] ammoDamages;
    public float maxDamage;
    public float minDamage;

}

public class SpaceCraftAmmoDamage : DerivedEntity, IEntityComponent {}