using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleCameraController : CameraController
{
    [SerializeField] private CinemachineCamera cmCamera;
    [SerializeField] private Transform target;
    [SerializeField] private BattleCameraEntityPreset cEntityPreset;
    
    private GameInputs _gameInputs;
    private Transform _cameraTr;
    private CinemachineInputAxisController _inputAxis;
    private CinemachineOrbitalFollow _cmOrbitalFollow;
        
    private Vector3 _inputDir;
    
    public override void Load(Action onLoad)
    {
        _gameInputs = G.GetService<InputsService>().GetGameInputs();
        _cameraTr = cmCamera.transform;
        _inputAxis = cmCamera.GetComponent<CinemachineInputAxisController>();
        _cmOrbitalFollow = cmCamera.GetComponent<CinemachineOrbitalFollow>();
        _inputAxis.enabled = false;
        UpdateOrbitsHeight();
        G.GetManager<RoutineManager>().StartLateUpdateAction(UpdateCamera);
        _gameInputs.Camera.Enable();
        _gameInputs.Camera.Position.performed += ReadMoveInput;
        _gameInputs.Camera.Position.canceled += ReadMoveInput;
        _gameInputs.Camera.RotationSwitcher.performed += UnlockRotation;
        _gameInputs.Camera.RotationSwitcher.canceled += LockRotation;
        _gameInputs.Camera.Zoom.performed += ChangeZoom;
        onLoad.Invoke();
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
        var currentZoom = cmCamera.Lens.OrthographicSize;
        cmCamera.Lens.OrthographicSize = Mathf.Clamp(currentZoom + zoomValue * cEntityPreset.zoomSpeed, 0.5f, cEntityPreset.maxZoom);
        UpdateOrbitsHeight();
    }

    private void UpdateOrbitsHeight()
    {
        _cmOrbitalFollow.Orbits.Top.Height = cEntityPreset.minTopOrbitHeight * cmCamera.Lens.OrthographicSize;
        _cmOrbitalFollow.Orbits.Center.Height = cEntityPreset.minCenterOrbitHeight * cmCamera.Lens.OrthographicSize;
        _cmOrbitalFollow.Orbits.Bottom.Height = cEntityPreset.minBottomOrbitHeight * cmCamera.Lens.OrthographicSize;
    }

    private void UpdateCamera()
    {
        target.rotation = _cameraTr.rotation;
        var moveDir = target.up * _inputDir.z + target.right * _inputDir.x;
        if (target.position.y >= 5f && moveDir.y > 0f) moveDir = new Vector3(moveDir.x, 0, moveDir.z);
        else if(target.position.y <= -5f && moveDir.y < 0f) moveDir = new Vector3(-moveDir.x, 0, moveDir.z);
        target.position += moveDir * (cEntityPreset.moveSpeed * Time.deltaTime);
        if (_cmOrbitalFollow.VerticalAxis.Value < cmCamera.Lens.OrthographicSize * 2)
            _cmOrbitalFollow.VerticalAxis.Value = cmCamera.Lens.OrthographicSize * 2;
    }

    public override void OnDisappear()
    {
        G.GetManager<RoutineManager>().StopLateUpdateAction(UpdateCamera);
        _gameInputs.Camera.Zoom.performed += ChangeZoom;
        _gameInputs.Camera.Position.performed -= ReadMoveInput;
        _gameInputs.Camera.Position.canceled -= ReadMoveInput;
        _gameInputs.Camera.RotationSwitcher.performed -= UnlockRotation;
        _gameInputs.Camera.RotationSwitcher.canceled -= LockRotation;
        _gameInputs.Camera.Disable();
        _gameInputs = null;
        cEntityPreset = null;
        _cameraTr = null;
        target = null;
        cmCamera = null;
        _inputAxis = null;
        _cmOrbitalFollow = null;
    }
}