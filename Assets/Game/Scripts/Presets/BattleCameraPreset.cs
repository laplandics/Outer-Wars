using UnityEngine;

[CreateAssetMenu(fileName = "BattleCamera_Preset", menuName = "Presets/CameraPreset")]
public class BattleCameraPreset : Preset
{
    public float moveSpeed;
    public float zoomSpeed;
    public float maxZoom;
    public float minTopOrbitHeight;
    public float minCenterOrbitHeight;
    public float minBottomOrbitHeight;
}