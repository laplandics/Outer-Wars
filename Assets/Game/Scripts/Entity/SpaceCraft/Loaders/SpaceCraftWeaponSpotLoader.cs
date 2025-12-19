using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraftWeaponSpotLoader : EntityLoader
{
    private SpaceCraftWeaponSpot _scWeaponSpot;
    private SpaceCraftWeaponSpotData _data;
    private Coroutine _loadRoutine;
    private Action _onLoad;
    private bool _weaponLoaded;
    
    public SpaceCraftWeaponSpotLoader
        (IMainEntity owner, SpaceCraftWeaponSpot scWs, Action onLoad, EntityData data) : base(owner, onLoad, data)
    {
        if (Data is not SpaceCraftWeaponSpotData spotData) return;
        _scWeaponSpot = scWs;
        _data = spotData;
        _loadRoutine = G.GetManager<RoutineManager>().StartRoutine(LoadSpot());
        _onLoad = onLoad;
    }

    private IEnumerator LoadSpot()
    {
        LoadWeapon();
        yield return new WaitUntil(() => _weaponLoaded);
        _onLoad.Invoke();
    }

    private void LoadWeapon()
    {
        if (Owner.Instance.data is not SpaceCraftData ownerData) throw new Exception("Owner is not SpaceCraft");
        _weaponLoaded = false;
        var weaponType = _data.weapon.weaponType;
        var weaponMark = _data.weapon.weaponMark;
        if (weaponType == SpaceCraftWeaponType.EmptySpot) { _weaponLoaded = true; return; }
        var allowedTypes = new List<SpaceCraftWeaponType>();
        var allowedMarks = new List<SpaceCraftWeaponMark>();
        foreach (var allowedWeaponTypeAndMark in _data.allowedWeapons) 
        { allowedTypes.Add(allowedWeaponTypeAndMark.type); allowedMarks.Add(allowedWeaponTypeAndMark.mark); }
        if (!allowedTypes.Contains(weaponType) || !allowedMarks.Contains(weaponMark)) { _weaponLoaded = true; return; }
        var fractionName = ownerData.fractionType.ToString();
        var className = ownerData.classType.ToString();
        var structureName = _scWeaponSpot.spotStructureType.ToString();
        var weaponName = weaponType.ToString();
        var weaponMarkName = weaponMark.ToString();
        var fullWeaponAssetName = $"{fractionName}{className}{structureName}{weaponName}{weaponMarkName}";
        Debug.Log(fullWeaponAssetName);
        Action<SpaceCraftWeapon> callback = loadedWeapon =>
        { loadedWeapon.transform.SetParent(_scWeaponSpot.transform, false); _scWeaponSpot.weapon = loadedWeapon; _weaponLoaded = true; };
        LoadEntityAsset(callback, SpaceCraft.Label, fullWeaponAssetName, _data.weapon);
    }

    public override void Dispose()
    {
        if (_loadRoutine != null) G.GetManager<RoutineManager>().EndRoutine(_loadRoutine);
        _loadRoutine = null;
        _onLoad = null;
        _data = null;
        _scWeaponSpot = null;
        base.Dispose();
    }
}