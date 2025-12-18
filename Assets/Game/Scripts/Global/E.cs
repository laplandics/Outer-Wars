using UnityEngine;

public static class E
{
    public static T NewEntity<T>(EntityData data) where T : GameEntity
    {
        var entity = new GameObject(data.entityName).AddComponent<T>();
        entity.data = data;
        return entity;
    }

    public static void DeleteEntity(GameEntity entity)
    {
        entity.OnDisappear();
        Spawner.Despawn(entity.gameObject);
    }
}