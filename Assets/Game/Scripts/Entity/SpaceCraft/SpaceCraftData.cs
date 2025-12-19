using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpaceCraftData : EntityData
{ public string spaceCraftName; public ClassType classType; public ControllerType controllerType; public FractionType fractionType;
    public List<SpaceCraftFrameStructureData> frameStructures; public List<SpaceCraftInternalStructureData> internalStructures; }
public enum ClassType { Corvette, Cruiser, Destroyer, Battleship, Titan };
public enum ControllerType { Enemy, Neutral, Ally, FleetMember }
public enum FractionType { Other, Humanity, Ashy }

[Serializable]
public class SpaceCraftFrameStructureData : EntityData
{ public FrameStructureType frameStructureType; public List<SpaceCraftWeaponSpotData> weaponSpots;
    public SpaceCraftFrameBaseStatsData baseStats; [ReadOnly] public SpaceCraftFrameCurrentStatsData currentStats; }
public enum FrameStructureType { Bow, Deck, Keel, Port, Starboard, Stern }

[Serializable]
public class SpaceCraftFrameBaseStatsData
{ public int baseArmor; public int baseShield; public int baseHealth; }

[Serializable]
public class SpaceCraftFrameCurrentStatsData
{ public int currentArmor; public int currentShield; public int currentHealth; }

[Serializable]
public class SpaceCraftWeaponSpotData : EntityData
{ public SpaceCraftSpotType spotType; public List<AllowedWeaponTypeAndMark> allowedWeapons; [ReadOnly] public SpaceCraftWeaponData weapon;
  [Serializable] public class AllowedWeaponTypeAndMark { public SpaceCraftWeaponType type; public SpaceCraftWeaponMark mark; }
}
public enum SpaceCraftSpotType { FirstSpot, SecondSpot, ThirdSpot }

[Serializable]
public class SpaceCraftWeaponData : EntityData
{ public SpaceCraftWeaponType weaponType; public SpaceCraftWeaponMark weaponMark;
    public List<SpaceCraftAmmoType> allowedAmmoTypes; [ReadOnly] public SpaceCraftAmmoType currentAmmoType; }
public enum SpaceCraftWeaponType { EmptySpot, Ram, Laser, Hangar, Artillery, Railgun, Mortar, TorpedoLauncher }
public enum SpaceCraftWeaponMark { None, Mk1, Mk2, Mk3, Mk4 }
public enum SpaceCraftAmmoType { NoAmmo }

[Serializable]
public class SpaceCraftInternalStructureData : EntityData
{ public InternalStructureType structureType; }
public enum InternalStructureType { FreeCell, Generator, Engine, Bridge }

[Serializable]
public class SpaceCraftState : EntityState
{
    public SpaceCraftClass scClass;
    public SpaceCraftController scController;
    public SpaceCraftFraction scFraction;
    public List<SpaceCraftFrameStructure> scFrameStructures;
    public List<SpaceCraftInternalStructure> scInternalStructures;
}

[Serializable]
public class SpaceCraftFrameStructureWeaponSpotPosition
{
    public SpaceCraftSpotType spotType;
    public Transform spotTr;
    [ReadOnly] public SpaceCraftWeaponSpot spot;
}