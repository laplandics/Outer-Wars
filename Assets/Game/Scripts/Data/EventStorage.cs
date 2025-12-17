public abstract class Event {}

namespace GlobalEvents
{
    public class StatesLoaded : Event {}
    public class StatesUnloaded : Event {}
    
    public class ServicesLaunched : Event {}
    public class ServicesStopped : Event {}
    
    public class ManagersInitialized : Event {}
    public class ManagersDeinitialized : Event {}
    
    public class SceneStarted : Event {}
    public class SceneEnded : Event {}
}

namespace BattleSceneEvents
{
    public class BattleParticipantSpawned : Event { public IBattleParticipant Participant; }
    public class BattleParticipantDespawned : Event { public IBattleParticipant Participant; }
    
    public class TurnBegan : Event { public IBattleParticipant Acting; }
    public class TurnEnded : Event { public IBattleParticipant Acting; }
}