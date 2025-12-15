using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/CameraSettings")]
public class CameraSettings : GameSettings
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxZoom;
    [SerializeField] private float minTopOrbitHeight;
    [SerializeField] private float minCenterOrbitHeight;
    [SerializeField] private float minBottomOrbitHeight;
    
    public float MoveSpeed => moveSpeed;
    public float ZoomSpeed => zoomSpeed;
    public float MaxZoom => maxZoom;
    public float MinTopOrbitHeight => minTopOrbitHeight;
    public float MinCenterOrbitHeight => minCenterOrbitHeight;
    public float MinBottomOrbitHeight => minBottomOrbitHeight;
    
    public override IEnumerator Set() { yield break; }
    public override void Unset() { }
}