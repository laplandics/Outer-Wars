using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[FRACTION]", menuName = "Presets/Weapons")]
public class SpaceCraftWeaponPreset : EntityPreset
{
    [SerializeField] private List<SpaceCraftWeaponData> ram;
    [SerializeField] private List<SpaceCraftWeaponData> laser;
    [SerializeField] private List<SpaceCraftWeaponData> hangar;
    [SerializeField] private List<SpaceCraftWeaponData> artillery;
    [SerializeField] private List<SpaceCraftWeaponData> railgun;
    [SerializeField] private List<SpaceCraftWeaponData> mortar;
    [SerializeField] private List<SpaceCraftWeaponData> torpedoLauncher;

    public SpaceCraftWeaponData GetData(SpaceCraftWeaponSpotData data,
        SpaceCraftWeaponType typePriority = SpaceCraftWeaponType.EmptySpot, bool countTypePriority = false,
        SpaceCraftWeaponMark markPriority = SpaceCraftWeaponMark.None, bool countMarkPriority = false)
    {
        var typeDataDict = new Dictionary<SpaceCraftWeaponType, List<SpaceCraftWeaponData>>
        {
            [SpaceCraftWeaponType.EmptySpot] = new(),
            [SpaceCraftWeaponType.Ram] = ram,
            [SpaceCraftWeaponType.Laser] = laser,
            [SpaceCraftWeaponType.Hangar] = hangar,
            [SpaceCraftWeaponType.Artillery] = artillery,
            [SpaceCraftWeaponType.Railgun] = railgun,
            [SpaceCraftWeaponType.Mortar] = mortar,
            [SpaceCraftWeaponType.TorpedoLauncher] = torpedoLauncher
        };
        var allowedWeapons = new List<SpaceCraftWeaponData>();
        foreach (var allowedWeaponTypeAndMark in data.allowedWeapons)
        {
            foreach (var weaponData in typeDataDict[allowedWeaponTypeAndMark.type])
            { if (allowedWeaponTypeAndMark.mark != weaponData.weaponMark) continue; allowedWeapons.Add(weaponData); }
        }
        var randomWeaponTypeAndMark = allowedWeapons[Random.Range(0, allowedWeapons.Count)];
        if (!countTypePriority) return randomWeaponTypeAndMark;
        var priorityTypeWeapons = allowedWeapons.FindAll(w => w.weaponType == typePriority);
        if (priorityTypeWeapons.Count == 0) return randomWeaponTypeAndMark;
        var randomWeaponMark = priorityTypeWeapons[Random.Range(0, priorityTypeWeapons.Count)];
        if (!countMarkPriority) return randomWeaponMark;
        var priorityWeapon = priorityTypeWeapons.Find(w => w.weaponMark == markPriority);
        return priorityWeapon ?? randomWeaponMark;
    }
}