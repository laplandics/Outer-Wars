using UnityEngine;

public abstract class GameEntity : MonoBehaviour
{
    public virtual void OnSpawn() {}
    public virtual void OnDespawn() {}
}

public abstract class EntityObject : GameEntity
{
    
}

public abstract class EntityCondition : GameEntity
{
    
}

public abstract class EntityComponent : GameEntity
{
    
}