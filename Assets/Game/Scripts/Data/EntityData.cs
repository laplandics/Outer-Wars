using System;
using System.Collections.Generic;

[Serializable]
public abstract class EntityData { public string id; }

[Serializable]
public class GameCameraData : EntityData
{
    public List<CameraComponents> components;
}

public enum CameraComponents { NoCamera, DefaultCamera, BattleCamera, ExploreCamera, }

[Serializable]
public class SpaceCraftData : EntityData
{

}
