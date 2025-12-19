using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpaceCraftManager : SceneManager
{
    public override IEnumerator Initialize()
    {
        Eventer.Subscribe<SceneStarted>(BuildNewSpaceCraft);
        yield break;
    }

    private void BuildNewSpaceCraft(SceneStarted _)
    {
        var spName = "SpaceCraft";
        var type = ClassType.Corvette;
        var fraction = FractionType.Humanity;
        var controller = ControllerType.Neutral;
        Action<SpaceCraft> onBuild = _ => { };
        G.GetManager<RoutineManager>().StartRoutine(BuildNewSpaceCraft(spName, type, fraction, controller, onBuild));
    }

    private static IEnumerator BuildNewSpaceCraft
        (string spName, ClassType ct, FractionType ft, ControllerType cnt, Action<SpaceCraft> onBuild)
    {
        var spaceCraftData = new SpaceCraftData();
        spaceCraftData.spaceCraftName = spName;
        spaceCraftData.classType = ct;
        spaceCraftData.fractionType = ft;
        spaceCraftData.controllerType = cnt;
        spaceCraftData.frameStructures = new List<SpaceCraftFrameStructureData>();
        spaceCraftData.internalStructures = new List<SpaceCraftInternalStructureData>();

        var structurePresetName = $"{ft.ToString()}{ct.ToString()}";
        SpaceCraftFrameStructurePreset frameStructurePreset = null;
        yield return LoadDataPreset<SpaceCraftFrameStructurePreset>(structurePresetName, preset => frameStructurePreset = preset);
        yield return new WaitUntil(() => frameStructurePreset != null);
        
        var bowData = frameStructurePreset.GetData(FrameStructureType.Bow);
        var deckData = frameStructurePreset.GetData(FrameStructureType.Deck);
        var keelData = frameStructurePreset.GetData(FrameStructureType.Keel);
        var portData = frameStructurePreset.GetData(FrameStructureType.Port);
        var starboardData = frameStructurePreset.GetData(FrameStructureType.Starboard);
        var sternData = frameStructurePreset.GetData(FrameStructureType.Stern);
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
        onBuild.Invoke(spaceCraft);
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

    public override void Deinitialize()
    {
        Eventer.Unsubscribe<SceneStarted>(BuildNewSpaceCraft);
    }
}