public abstract class Event {}

public class StatesLoaded : Event {}
public class StatesUnloaded : Event {}

public class ServicesLaunched : Event {}
public class ServicesStopped : Event {}

public class ManagersInitialized : Event {}
public class ManagersDeinitialized : Event {}

public class SceneStarted : Event {}
public class SceneEnded : Event {}