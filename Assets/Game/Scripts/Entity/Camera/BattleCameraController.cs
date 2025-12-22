using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleCameraController : CameraController
{
    [SerializeField] private CinemachineCamera cmCamera;
    [SerializeField] private Transform target;
    [SerializeField] private BattleCameraPreset cPreset;
    [SerializeField] private CinemachineInputAxisController cmController;
    [SerializeField] private CinemachineOrbitalFollow cmOrbitalFollow;
    private LensSettings.OverrideModes _mode;
    private Transform _cameraTransform;
    private Vector3 _inputDir;
    private GameInputs _inputs;
    
    public override void Load(Action onLoad)
    {
        if (data is not GameCameraData cameraData) throw new Exception("Camera controller has no gameCameraData");
        _cameraTransform = cmCamera.transform;
        G.GetManager<RoutineManager>().StartLateUpdateAction(UpdateTarget);
        ChangeCameraMode(cameraData.cameraMode);
        _inputs = G.GetService<InputsService>().GetGameInputs();
        _inputs.Camera.Enable();
        _inputs.Camera.Position.performed += ReadMoveVector;
        _inputs.Camera.Position.canceled += ReadMoveVector;
        _inputs.Camera.RotationSwitcher.performed += SwitchRotation;
        _inputs.Camera.RotationSwitcher.canceled += SwitchRotation;
        _inputs.Camera.Zoom.performed += UpdateZoom;
        _inputs.Camera.Zoom.canceled += UpdateZoom;
        _inputs.Camera.ChangeMode.performed += ChangeModeOnPerformed;
        onLoad.Invoke();
    }

    private void ChangeModeOnPerformed(InputAction.CallbackContext input)
    {
        ChangeCameraMode(_mode == LensSettings.OverrideModes.Perspective
            ? LensSettings.OverrideModes.Orthographic
            : LensSettings.OverrideModes.Perspective);
    }

    private void ChangeCameraMode(LensSettings.OverrideModes mode)
    {
        _mode = mode;
        cmCamera.Lens.ModeOverride = _mode;
        if (_mode == LensSettings.OverrideModes.Perspective)
            cmCamera.Lens.FieldOfView = cPreset.perspectiveFoV;
    }

    private void ReadMoveVector(InputAction.CallbackContext ctx)
    {
        _inputDir = Vector2.zero;
        _inputDir.x = ctx.ReadValue<Vector2>().x;
        _inputDir.z = ctx.ReadValue<Vector2>().y;
    }

    private void SwitchRotation(InputAction.CallbackContext input) { cmController.enabled = input.performed; }

    private void UpdateZoom(InputAction.CallbackContext input)
    {
        var zoomValue = input.ReadValue<float>();
        switch (_mode)
        {
            case LensSettings.OverrideModes.None:
                float currentZoom;
                throw new Exception("Camera should have override camera mode in gameCameraData");
            case LensSettings.OverrideModes.Orthographic:
                currentZoom = cmCamera.Lens.OrthographicSize;
                cmCamera.Lens.OrthographicSize = Mathf.Clamp(currentZoom + zoomValue * cPreset.orthographicZoomSpeed, 0.5f, cPreset.maxOrthographicZoom);
                cmOrbitalFollow.Radius = Mathf.Clamp(cmCamera.Lens.OrthographicSize, cPreset.minCameraRadius, cPreset.maxPerspectiveZoom);
                break;
            case LensSettings.OverrideModes.Perspective:
                currentZoom = cmOrbitalFollow.Radius;
                cmOrbitalFollow.Radius = Mathf.Clamp(currentZoom + zoomValue * cPreset.perspectiveZoomSpeed, cPreset.minCameraRadius, cPreset.maxPerspectiveZoom);
                cmCamera.Lens.OrthographicSize = cmOrbitalFollow.Radius;
                break;
            case LensSettings.OverrideModes.Physical:
                throw new Exception("Physical camera mode is not supported");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateTarget()
    {
        target.rotation = _cameraTransform.rotation;
        var moveDir = target.up * _inputDir.z + target.right * _inputDir.x;
        if (target.position.y > cPreset.maxYOffset && moveDir.y > 0 ||
            target.position.y < -cPreset.maxYOffset && moveDir.y < 0) moveDir.y = 0;
        if (target.position.x > cPreset.maxXOffset && moveDir.x > 0 ||
            target.position.x < -cPreset.maxXOffset && moveDir.x < 0) moveDir.x = 0;
        if (target.position.z > cPreset.maxZOffset && moveDir.z > 0 ||
            target.position.z < -cPreset.maxZOffset && moveDir.z < 0) moveDir.z = 0;
        target.position += moveDir * (cPreset.moveSpeed * Time.deltaTime);
    }

    public override void OnDisappear()
    {
        _inputs.Camera.Position.performed -= ReadMoveVector;
        _inputs.Camera.Position.canceled -= ReadMoveVector;
        _inputs.Camera.RotationSwitcher.performed -= SwitchRotation;
        _inputs.Camera.RotationSwitcher.canceled -= SwitchRotation;
        _inputs.Camera.Zoom.performed -= UpdateZoom;
        _inputs.Camera.Zoom.canceled -= UpdateZoom;
    }
}