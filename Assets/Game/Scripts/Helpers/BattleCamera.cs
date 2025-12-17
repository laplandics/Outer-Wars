using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

[Preserve]
public class BattleCamera : EntityComponent
{
    [SerializeField] private CinemachineCamera cmCamera;
    [SerializeField] private Transform target;
    [SerializeField] private BattleCameraPreset cPreset;
    
    private GameInputs _gameInputs;
    private Transform _cameraTr;
    private CinemachineInputAxisController _inputAxis;
    private CinemachineOrbitalFollow _cmOrbitalFollow;
        
    private Vector3 _inputDir;

    public override void OnAppear()
    {
        _gameInputs = G.GetService<InputsService>().GetGameInputs();
        _cameraTr = instance.transform;
        _inputAxis = cmCamera.GetComponent<CinemachineInputAxisController>();
        _cmOrbitalFollow = cmCamera.GetComponent<CinemachineOrbitalFollow>();
        _inputAxis.enabled = false;
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
        var currentZoom = cmCamera.Lens.OrthographicSize;
        cmCamera.Lens.OrthographicSize = Mathf.Clamp(currentZoom + zoomValue * cPreset.zoomSpeed, 0.5f, cPreset.maxZoom);
        UpdateOrbitsHeight();
    }

    private void UpdateOrbitsHeight()
    {
        _cmOrbitalFollow.Orbits.Top.Height = cPreset.minTopOrbitHeight * cmCamera.Lens.OrthographicSize;
        _cmOrbitalFollow.Orbits.Center.Height = cPreset.minCenterOrbitHeight * cmCamera.Lens.OrthographicSize;
        _cmOrbitalFollow.Orbits.Bottom.Height = cPreset.minBottomOrbitHeight * cmCamera.Lens.OrthographicSize;
    }

    private void UpdateTargetPosition()
    {
        var moveDir = _cameraTr.up * _inputDir.z + _cameraTr.right * _inputDir.x;
        target.position += moveDir * (cPreset.moveSpeed * Time.deltaTime);
    }

    public override void OnDisappear()
    {
        G.GetManager<RoutineManager>().StopLateUpdateAction(UpdateTargetPosition);
        _gameInputs.Camera.Zoom.performed += ChangeZoom;
        _gameInputs.Camera.Position.performed -= ReadMoveInput;
        _gameInputs.Camera.Position.canceled -= ReadMoveInput;
        _gameInputs.Camera.RotationSwitcher.performed -= UnlockRotation;
        _gameInputs.Camera.RotationSwitcher.canceled -= LockRotation;
        _gameInputs.Camera.Disable();
        _gameInputs = null;
        cPreset = null;
        _cameraTr = null;
        target = null;
        cmCamera = null;
        _inputAxis = null;
        _cmOrbitalFollow = null;
    }
}