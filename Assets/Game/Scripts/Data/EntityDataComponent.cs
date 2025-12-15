using System;
using UnityEngine;

[Serializable]
public abstract class EntityDataComponent
{
    [SerializeField][ReadOnly] protected string componentName;

    public EntityDataComponent() { componentName = GetType().Name; }
}

[Serializable, ComponentKind(DataComponentIdentificator.EntityObject)]
public class EntityObjectDC : EntityDataComponent
{
    public EntityObject entityObject;
}

[Serializable, ComponentKind(DataComponentIdentificator.EntityCondition)]
public class EntityConditionDC : EntityDataComponent
{
    public EntityCondition entityCondition;
}

[Serializable, ComponentKind(DataComponentIdentificator.EntityComponent)]
public class EEntityComponentDC : EntityDataComponent
{
    public EntityComponent entityComponent;
}