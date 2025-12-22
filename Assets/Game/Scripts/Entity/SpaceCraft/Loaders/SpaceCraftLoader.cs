using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraftLoader : EntityLoader
{
    private Dictionary<InternalStructureType, bool> _internalStructuresLoads;
    private Dictionary<SpaceCraftFrameStructureType, bool> _frameStructuresLoads;
    private bool _fractionLoaded;
    private bool _controllerLoaded;
    private bool _classLoaded;
    private Coroutine _loadRoutine;
    private SpaceCraftData _data;
    private SpaceCraftState _state;
    
    public SpaceCraftLoader(IMainEntity owner, Action onLoad, EntityData data, SpaceCraftState state) : base(owner, onLoad, data)
    {
        if (data is not SpaceCraftData spaceCraftData) return;
        _data = spaceCraftData;
        _state = state;
        _loadRoutine = G.GetManager<RoutineManager>().StartRoutine(LoadComponents());
    }

    private IEnumerator LoadComponents()
    {
        LoadInternalStructures();
        yield return new WaitUntil(() => !_internalStructuresLoads.ContainsValue(false));
        LoadFrameStructures();
        yield return new WaitUntil(() => !_frameStructuresLoads.ContainsValue(false));
        LoadFraction();
        yield return new WaitUntil(() => _fractionLoaded);
        LoadController();
        yield return new WaitUntil(() => _controllerLoaded);
        LoadClass();
        yield return new WaitUntil(() => _classLoaded);
        OnEntityAppear();
        Debug.Log("On spacecraft appear");
        OnLoad.Invoke();
    }

    private void LoadInternalStructures()
    {
        _internalStructuresLoads = new Dictionary<InternalStructureType, bool>();
        _state.scInternalStructures = new List<SpaceCraftInternalStructure>();
        foreach (var structureData in _data.internalStructures) { _internalStructuresLoads.Add(structureData.structureType, false); }
        foreach (var structureData in _data.internalStructures)
        {
            var st = structureData.structureType;
            if (st == InternalStructureType.FreeCell) { _internalStructuresLoads[st] = true; continue; }
            var structureName = $"{_data.spaceCraftClassType.ToString()}" +
                                $"{st.ToString()}";
            Action<SpaceCraftInternalStructure> callback = structure =>
                { _state.scInternalStructures.Add(structure); _internalStructuresLoads[st] = true; };
            LoadEntityAsset(callback, SpaceCraft.Label, structureName, structureData);
        }
    }
    
    private void LoadFrameStructures()
    {
        _frameStructuresLoads = new Dictionary<SpaceCraftFrameStructureType, bool>();
        _state.scFrameStructures = new List<SpaceCraftFrameStructure>();
        foreach (var structureData in _data.frameStructures) { _frameStructuresLoads.Add(structureData.spaceCraftFrameStructureType, false); }
        foreach (var structureData in _data.frameStructures)
        {
            var structureName = $"{_data.spaceCraftFractionType.ToString()}" +
                                $"{_data.spaceCraftClassType.ToString()}" +
                                $"{structureData.spaceCraftFrameStructureType.ToString()}";
            Action<SpaceCraftFrameStructure> callback = structure =>
                { _state.scFrameStructures.Add(structure); _frameStructuresLoads[structureData.spaceCraftFrameStructureType] = true; };
            LoadEntityAsset(callback, SpaceCraft.Label, structureName, structureData);
        }
    }

    private void LoadFraction()
    {
        _fractionLoaded = false;
        Action<SpaceCraftFraction> callback = fraction =>
            { _state.scFraction = fraction; _fractionLoaded = true; };
        LoadEntityComponent(callback, _data.spaceCraftFractionType.ToString());
    }

    private void LoadController()
    {
        _controllerLoaded = false;
        Action<SpaceCraftController> callback = controller => 
            { _state.scController = controller; _controllerLoaded = true; };
        LoadEntityComponent(callback,_data.spaceCraftControllerType.ToString());
    }

    private void LoadClass()
    {
        _classLoaded = false;
        Action<SpaceCraftClass> callBack = spaceCraftClass =>
            { _state.scClass = spaceCraftClass; _classLoaded = true; };
        LoadEntityComponent(callBack, _data.spaceCraftClassType.ToString());
    }

    private void OnEntityAppear()
    {
        Owner.Instance.OnAppear();
        _state.scFraction.OnAppear();
        _state.scController.OnAppear();
        _state.scClass.OnAppear();
        foreach (var structure in _state.scInternalStructures) structure.OnAppear();
        foreach (var structure in _state.scFrameStructures) structure.OnAppear();
    }

    public override void Dispose()
    {
        if (_loadRoutine != null) G.GetManager<RoutineManager>().EndRoutine(_loadRoutine);
        _loadRoutine = null;
        _state.scFraction?.OnDisappear();
        _state.scController?.OnDisappear();
        _state.scClass?.OnDisappear();
        foreach (var structure in _state.scInternalStructures) structure.OnDisappear();
        foreach (var structure in _state.scFrameStructures) structure.OnDisappear();
        _state = null;
        _data = null;
        base.Dispose();
    }
}