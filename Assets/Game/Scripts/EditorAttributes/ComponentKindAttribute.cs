using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ComponentKindAttribute : Attribute
{
    public DataComponentIdentificator DataComponentIdentificator { get; }

    public ComponentKindAttribute(DataComponentIdentificator dataComponentIdentificator)
    {
        DataComponentIdentificator = dataComponentIdentificator;
    }
}