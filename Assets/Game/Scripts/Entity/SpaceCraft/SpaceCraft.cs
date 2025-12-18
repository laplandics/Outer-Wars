using System;
using System.Collections;
using UnityEngine;

public class SpaceCraft : GameEntity
{
    [SerializeField] private SpaceCraftState state;
    private SpaceCraftData _data;
    private Action _onLoad;
    private Coroutine _loadRoutine;
    
    
    public override void Load(Action onLoad)
    {
        if (data is not SpaceCraftData spaceCraftData) return;
        _data = spaceCraftData;
        _onLoad = onLoad;
        state = new SpaceCraftState();
        _loadRoutine = G.GetManager<RoutineManager>().StartRoutine(LoadComponents());
    }

    private bool _participantHandlerLoaded;
    private bool _classLoaded;
    private IEnumerator LoadComponents()
    {
        LoadClass();
        yield return new WaitUntil(() => _classLoaded);
        _onLoad.Invoke();
    }

    private void LoadClass()
    {
        Action<SpaceCraftClass> callBack = spaceCraftClass =>
            { state.spaceCraftClass = spaceCraftClass; _classLoaded = true; };
        LoadAsset(callBack, nameof(SpaceCraft), _data.classType.ToString());
    }

    public override void OnDisappear()
    {
        if (_loadRoutine != null) G.GetManager<RoutineManager>().EndRoutine(_loadRoutine);
    }
}

[Serializable]
public class SpaceCraftData : EntityData
{
    public ClassType classType;
    public ParticipantType participantType;
    public FractionType fractionType;
    public SpaceCraftStructureData[] structures;
}
public enum ClassType { None, Corvette };
public enum ParticipantType { None, Enemy, Neutral, Ally, FleetMember }
public enum FractionType { None, Humanity, Ashy }

[Serializable]
public class SpaceCraftStructureData : EntityData
{
    public StructureType structureType;
    public SpaceCraftWeaponData structureWeapon;
    public StructureStatsData stats;
}
public enum StructureType { Bow, Stern, Starboard, Port }

[Serializable]
public class StructureStatsData : EntityData 
{ public float shield; public float armor; public float health; }

[Serializable]
public class SpaceCraftWeaponData : EntityData { public StarCraftWeaponType weaponType; }
public enum StarCraftWeaponType { None, Laser, Hangar, Artillery, Torpedo, Missile, Nuclear }

[Serializable]
public class SpaceCraftState : EntityState
{
    public SpaceCraftClass spaceCraftClass;
    public SpaceCraftParticipantHandler participantHandler;
    public SpaceCraftFractionHandler fractionHandler;
}

public abstract class SpaceCraftClass : EntityComponent {}
public abstract class SpaceCraftParticipantHandler : EntityComponent {}
public abstract class SpaceCraftFractionHandler : EntityComponent {}