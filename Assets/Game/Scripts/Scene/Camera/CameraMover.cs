using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMover : IDisposable
{
    private CinemachineInputAxisController _inputAxis;
    private CinemachineOrbitalFollow _cmOrbitalFollow;
    private CinemachineCamera _cmCamera;
    private GameInputs _gameInputs;
    private Transform _camera;
    private CameraSettings _cSettings;
    private Transform _target;
        
    private Vector3 _inputDir;
    
    
    public CameraMover(GameInputs inputs, CinemachineCamera camera, Transform target)
    {
        _inputAxis = camera.GetComponent<CinemachineInputAxisController>();
        _inputAxis.enabled = false;
        _cmCamera = camera;
        _cmOrbitalFollow = _cmCamera.GetComponent<CinemachineOrbitalFollow>();
        _gameInputs = inputs;
        _cSettings = G.GetSettings<CameraSettings>();
        _camera = camera.transform;
        _target = target;
        UpdateOrbitsHeight();
        G.GetManager<RoutineManager>().StartLateUpdateAction(UpdateTargetPosition);
        _gameInputs.Camera.Enable();
        _gameInputs.Camera.Position.performed += ReadMoveInput;
        _gameInputs.Camera.Position.canceled += ReadMoveInput;
        _gameInputs.Camera.RotationSwitcher.performed += UnlockRotation;
        _gameInputs.Camera.RotationSwitcher.canceled += LockRotation;
        _gameInputs.Camera.Zoom.performed += ChangeZoom;
    }

    private void ReadMoveInput(InputAction.CallbackContext input)
    {
        _inputDir = Vector3.zero;
        _inputDir.x = input.ReadValue<Vector2>().x;
        _inputDir.z = input.ReadValue<Vector2>().y;
    }

    private void LockRotation(InputAction.CallbackContext _) => _inputAxis.enabled = false;

    private void UnlockRotation(InputAction.CallbackContext _) => _inputAxis.enabled = true;

    private void ChangeZoom(InputAction.CallbackContext input)
    {
        var zoomValue = input.ReadValue<float>();
        var currentZoom = _cmCamera.Lens.OrthographicSize;
        _cmCamera.Lens.OrthographicSize = Mathf.Clamp(currentZoom + zoomValue * _cSettings.ZoomSpeed, 0.5f, _cSettings.MaxZoom);
        UpdateOrbitsHeight();
    }

    private void UpdateOrbitsHeight()
    {
        _cmOrbitalFollow.Orbits.Top.Height = _cSettings.MinTopOrbitHeight * _cmCamera.Lens.OrthographicSize;
        _cmOrbitalFollow.Orbits.Center.Height = _cSettings.MinCenterOrbitHeight * _cmCamera.Lens.OrthographicSize;
        _cmOrbitalFollow.Orbits.Bottom.Height = _cSettings.MinBottomOrbitHeight * _cmCamera.Lens.OrthographicSize;
    }

    private void UpdateTargetPosition()
    {
        var moveDir = _camera.up * _inputDir.z + _camera.right * _inputDir.x;
        _target.position += moveDir * (_cSettings.MoveSpeed * Time.deltaTime);
    }

    public void Dispose()
    {
        _gameInputs.Camera.Zoom.performed += ChangeZoom;
        _gameInputs.Camera.Position.performed -= ReadMoveInput;
        _gameInputs.Camera.Position.canceled -= ReadMoveInput;
        _gameInputs.Camera.RotationSwitcher.performed -= UnlockRotation;
        _gameInputs.Camera.RotationSwitcher.canceled -= LockRotation;
        _gameInputs.Camera.Disable();
        _gameInputs = null;
        _cSettings = null;
        _camera = null;
        _target = null;
        _cmCamera = null;
        _inputAxis = null;
        _cmOrbitalFollow = null;
    }
}