using System;
using System.Collections;
using UnityEngine;

public class SpaceCraftWeaponLoader : EntityLoader
{
    private SpaceCraftWeaponData _data;
    private Action _onLoad;
    private SpaceCraftWeapon _spaceCraftWeapon;
    private Coroutine _loadRoutine;
    
    public SpaceCraftWeaponLoader
        (IMainEntity owner, Action onLoad, EntityData data, SpaceCraftWeapon scw) : base(owner, onLoad, data)
    {
        if (data is not SpaceCraftWeaponData weaponData) return;
        _spaceCraftWeapon = scw;
        _data = weaponData;
        G.GetManager<RoutineManager>().StartRoutine(LoadWeapon());
        _onLoad = onLoad;
    }

    private bool _ammoLoaded;
    private IEnumerator LoadWeapon()
    {
        LoadAmmo();
        yield return new WaitUntil(() => _ammoLoaded);
        _onLoad.Invoke();
    }

    private void LoadAmmo()
    {
        _ammoLoaded = false;
        if (!_data.allowedAmmoTypes.Contains(_data.currentAmmoType) ||
            _data.currentAmmoType == SpaceCraftAmmoType.NoAmmo)
        { _ammoLoaded = true; return; }
        Action<SpaceCraftAmmo> callback = ammo => { _spaceCraftWeapon.currentAmmo = ammo; _ammoLoaded = true; };
        LoadEntityAsset(callback, SpaceCraft.Label, _data.currentAmmoType.ToString());
    }

    public override void Dispose()
    {
        if (_loadRoutine != null) G.GetManager<RoutineManager>().EndRoutine(_loadRoutine);
        _loadRoutine = null;
        _data = null;
        _spaceCraftWeapon = null;
        _onLoad = null;
        base.Dispose();
    }
}