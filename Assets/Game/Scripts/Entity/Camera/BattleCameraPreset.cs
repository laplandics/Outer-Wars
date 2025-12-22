using UnityEngine;

[CreateAssetMenu(fileName = "BattleCamera_Preset", menuName = "Presets/CameraPreset")]
public class BattleCameraPreset : EntityPreset
{
    public float moveSpeed;
    public float maxXOffset;
    public float maxYOffset;
    public float maxZOffset;
    
    public float orthographicZoomSpeed;
    public float maxOrthographicZoom;
    
    public float perspectiveZoomSpeed;
    public float maxPerspectiveZoom;
    public int perspectiveFoV;
    public float minCameraRadius;
}