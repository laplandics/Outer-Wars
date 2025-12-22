using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraftFrameStructureLoader : EntityLoader
{
    private Dictionary<SpaceCraftSpotType, bool> _weaponsOnSpotsLoads;
    private SpaceCraftFrameStructureData _data;
    private Action _onLoad;
    private SpaceCraftFrameStructure _spaceCraftFrameStructure;
    private Coroutine _loadRoutine;
    
    public SpaceCraftFrameStructureLoader
        (IMainEntity owner, Action onLoad, EntityData data, SpaceCraftFrameStructure sfs) : base(owner, onLoad, data)
    {
        if (data is not SpaceCraftFrameStructureData structureData) return;
        _spaceCraftFrameStructure = sfs;
        _data = structureData;
        _onLoad = onLoad;
        _loadRoutine = G.GetManager<RoutineManager>().StartRoutine(LoadStructure());
    }

    private IEnumerator LoadStructure()
    {
        LoadStats();
        LoadWeaponsOnSpots();
        yield return new WaitUntil(() => !_weaponsOnSpotsLoads.ContainsValue(false));
        _onLoad?.Invoke();
    }

    private void LoadStats()
    {
        _spaceCraftFrameStructure.baseStats = new SpaceCraftFrameStructure.BaseStats
        {
            baseArmor = _data.baseStats.baseArmor,
            baseShield = _data.baseStats.baseShield,
            baseHealth = _data.baseStats.baseHealth
        };
        _spaceCraftFrameStructure.currentStats = new SpaceCraftFrameStructure.CurrentStats
        {
            currentArmor = _data.currentStats.currentArmor,
            currentShield = _data.currentStats.currentShield,
            currentHealth = _data.currentStats.currentHealth
        };
    }

    private void LoadWeaponsOnSpots()
    {
        _weaponsOnSpotsLoads = new Dictionary<SpaceCraftSpotType, bool>();
        if (_spaceCraftFrameStructure.weaponSpotPositions.Count == 0) return;
        foreach (var spotPosition in _data.weaponSpots) { _weaponsOnSpotsLoads.Add(spotPosition.spotType, false); }
        foreach (var spotData in _data.weaponSpots)
        {
            var rightSpot = _spaceCraftFrameStructure.weaponSpotPositions.Find(s => s.spotType == spotData.spotType);
            if (rightSpot == null) { _weaponsOnSpotsLoads[spotData.spotType] = true; continue; }
            var newSpot = new GameObject(spotData.spotType.ToString()).AddComponent<SpaceCraftWeaponSpot>();
            newSpot.transform.SetParent(_spaceCraftFrameStructure.transform, false);
            newSpot.transform.localPosition = rightSpot.spotTr.localPosition;
            Spawner.Despawn(rightSpot.spotTr.gameObject);
            rightSpot.spotTr = newSpot.transform;
            newSpot.spotType = rightSpot.spotType;
            newSpot.data = spotData;
            newSpot.spotStructureType = _data.spaceCraftFrameStructureType;
            newSpot.Owner = Owner;
            rightSpot.spot = newSpot;
            Action onSpotLoaded = () => { _weaponsOnSpotsLoads[spotData.spotType] = true; };
            newSpot.Load(onSpotLoaded);
        }
    }

    public override void Dispose()
    {
        if (_loadRoutine != null) G.GetManager<RoutineManager>().EndRoutine(_loadRoutine);
        _weaponsOnSpotsLoads = null;
        _data = null;
        _onLoad = null;
        _spaceCraftFrameStructure = null;
        _loadRoutine = null;
        base.Dispose();
    }
}

