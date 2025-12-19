using UnityEngine;

[CreateAssetMenu(fileName = "BattleCamera_Preset", menuName = "Presets/CameraPreset")]
public class BattleCameraEntityPreset : EntityPreset
{
    public float moveSpeed;
    public float zoomSpeed;
    public float maxZoom;
    public float minTopOrbitHeight;
    public float minCenterOrbitHeight;
    public float minBottomOrbitHeight;
}