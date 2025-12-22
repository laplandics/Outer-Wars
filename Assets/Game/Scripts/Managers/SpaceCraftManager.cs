using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpaceCraftManager : SceneManager
{
    public override IEnumerator OnStart()
    {
        var spName = "SpaceCraft";
        var type = SpaceCraftClassType.Corvette;
        var fraction = SpaceCraftFractionType.Humanity;
        var controller = SpaceCraftControllerType.Neutral;
        G.GetManager<RoutineManager>().StartRoutine(BuildNewSpaceCraft(spName, type, fraction, controller));
        yield break;
    }

    private static IEnumerator BuildNewSpaceCraft
        (string spName, SpaceCraftClassType ct, SpaceCraftFractionType ft, SpaceCraftControllerType cnt, Action<SpaceCraft> onBuild = null)
    {
        var spaceCraftData = new SpaceCraftData();
        spaceCraftData.spaceCraftName = spName;
        spaceCraftData.spaceCraftClassType = ct;
        spaceCraftData.spaceCraftFractionType = ft;
        spaceCraftData.spaceCraftControllerType = cnt;
        spaceCraftData.frameStructures = new List<SpaceCraftFrameStructureData>();
        spaceCraftData.internalStructures = new List<SpaceCraftInternalStructureData>();

        var structurePresetName = $"{ft.ToString()}{ct.ToString()}";
        SpaceCraftFrameStructurePreset frameStructurePreset = null;
        yield return LoadDataPreset<SpaceCraftFrameStructurePreset>(structurePresetName, preset => frameStructurePreset = preset);
        yield return new WaitUntil(() => frameStructurePreset != null);
        
        var bowData = frameStructurePreset.GetData(SpaceCraftFrameStructureType.Bow);
        var deckData = frameStructurePreset.GetData(SpaceCraftFrameStructureType.Deck);
        var keelData = frameStructurePreset.GetData(SpaceCraftFrameStructureType.Keel);
        var portData = frameStructurePreset.GetData(SpaceCraftFrameStructureType.Port);
        var starboardData = frameStructurePreset.GetData(SpaceCraftFrameStructureType.Starboard);
        var sternData = frameStructurePreset.GetData(SpaceCraftFrameStructureType.Stern);
        var frameStructures = new List<SpaceCraftFrameStructureData> {bowData, deckData, keelData, portData, starboardData, sternData};
        if (frameStructurePreset != null) Destroy(frameStructurePreset);
        
        var weaponPresetName = $"{ft.ToString()}";
        SpaceCraftWeaponPreset weaponPreset = null;
        yield return LoadDataPreset<SpaceCraftWeaponPreset>(weaponPresetName, preset => weaponPreset = preset);
        yield return new WaitUntil(() => weaponPreset != null);

        foreach (var weaponSpotData in frameStructures.SelectMany(structure => structure.weaponSpots))
        { weaponSpotData.weapon = weaponPreset.GetData(weaponSpotData); }
        if (weaponPreset != null) Destroy(weaponPreset);
        
        spaceCraftData.frameStructures = frameStructures;
        var spaceCraft = E.NewEntity<SpaceCraft>(spaceCraftData);
        var spaceCraftLoaded = false;
        spaceCraft.Load(() => spaceCraftLoaded = true);
        yield return new WaitUntil(() => spaceCraftLoaded);
        onBuild?.Invoke(spaceCraft);
    }

    private static IEnumerator LoadDataPreset<T>(string presetName, Action<T> onLoad) where T : EntityPreset
    {
        T presetInstance = null;
        var handle = Addressables.LoadAssetsAsync<T>(nameof(SpaceCraft), preset =>
        { if (preset.name != presetName) return; presetInstance = Instantiate(preset); });
        yield return handle;
        if (handle.Status != AsyncOperationStatus.Succeeded) throw handle.OperationException;
        onLoad.Invoke(presetInstance);
        handle.Release();
    }
}