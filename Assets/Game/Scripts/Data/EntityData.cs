using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NEW_ENTITY", menuName = "GameData/New entityData")]
public class EntityData : ScriptableObject
{
    public GameObject prefab;
    [Header("Entity Data Components")]
    public List<DataComponentIdentificator> addComponents = new();
    [SerializeReference] public List<EntityDataComponent> components = new();

    public T GetDataComponent<T>() where T : EntityDataComponent
    {
        foreach(var component in components) {if(component is T t) return t;}
        return null;
    }

    [Button]
    private void AddComponents()
    {
        components.Clear();
        foreach(var component in addComponents)
        {
            var classType = GetTypeByIdentificator(component);
            var instance = (EntityDataComponent)Activator.CreateInstance(classType);
            components.Add(instance);
        }
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    private static Type GetTypeByIdentificator(DataComponentIdentificator identificator)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t =>
                t.IsSubclassOf(typeof(EntityDataComponent)) &&
                t.GetCustomAttribute<ComponentKindAttribute>() != null)
            .ToArray();
        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<ComponentKindAttribute>();
            if(attr.DataComponentIdentificator == identificator) return type;
        }
        throw new Exception($"Type {identificator} not found");
    }
}

public enum DataComponentIdentificator
{
    EntityObject,
    EntityCondition,
    EntityComponent

}

