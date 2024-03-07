using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;

public class Spatial_SCC : MonoBehaviour, IVehicleInputActionsListener
{
    public SCC_InputProcessor inputProcessor;
    public Transform seat;
    public Transform cameraTarget;

    private SpatialSyncedObject _syncedObject;
    private SCC_Inputs _inputs = new SCC_Inputs();
    private bool _isDriving = false;

    private const VehicleInputFlags INPUT_FLAGS = VehicleInputFlags.Steer1D | VehicleInputFlags.Throttle | VehicleInputFlags.Reverse | VehicleInputFlags.PrimaryAction;

    private void Awake()
    {
        _syncedObject = GetComponent<SpatialSyncedObject>();

        UpdateShouldBeDriving();
        _syncedObject.onOwnerChanged += HandleOwnerChanged;
        SpatialBridge.spaceContentService.onSceneInitialized += HandleSceneInitialized;
    }

    private void OnDestroy()
    {
        SpatialBridge.inputService.ReleaseInputCapture(this);
        if (_isDriving)
            StoppedDriving();
    }

    private void Update()
    {
        if (_syncedObject.isLocallyOwned)
        {
            inputProcessor.OverrideInputs(_inputs);
        }
    }

    public void StartDriving()
    {
        SpatialBridge.inputService.StartVehicleInputCapture(INPUT_FLAGS, null, null, this);
    }

    public void StopDriving()
    {
        SpatialBridge.inputService.ReleaseInputCapture(this);
        Destroy(gameObject);
    }

    private void HandleOwnerChanged(int newOwner)
    {
        UpdateShouldBeDriving();
    }

    private void HandleSceneInitialized()
    {
        UpdateShouldBeDriving();
    }

    private void UpdateShouldBeDriving()
    {
        if (_syncedObject.isLocallyOwned)
        {
            if (!_isDriving)
            {
                StartDriving();
            }
        }
        else
        {
            if (_isDriving)
            {
                StopDriving();
            }
        }
    }

    private void StartedDriving()
    {
        SpatialBridge.cameraService.SetTargetOverride(cameraTarget, SpatialCameraMode.Vehicle);
        SpatialBridge.actorService.localActor.avatar.Sit(seat);
        _inputs.steerInput = 0;
        _inputs.handbrakeInput = 0;
        _inputs.throttleInput = 0;
        _inputs.brakeInput = 0;
        _isDriving = true;
    }

    private void StoppedDriving()
    {
        SpatialBridge.cameraService.ClearTargetOverride();
        SpatialBridge.actorService.localActor.avatar.Stand();
        _inputs.handbrakeInput = 1;
        _inputs.throttleInput = 0;
        _inputs.brakeInput = 0;
        inputProcessor.OverrideInputs(_inputs);
        _isDriving = false;
    }

#region IVehicleInputActionsListener

    public void OnInputCaptureStarted(InputCaptureType type)
    {
        StartedDriving();
    }

    public void OnInputCaptureStopped(InputCaptureType type)
    {
        StoppedDriving();
    }

    public void OnVehicleSteerInput(InputPhase inputPhase, Vector2 inputSteer)
    {
        _inputs.steerInput = inputSteer.x;
    }

    public void OnVehicleThrottleInput(InputPhase inputPhase, float inputThrottle)
    {
        _inputs.throttleInput = inputThrottle;
    }

    public void OnVehicleReverseInput(InputPhase inputPhase, float inputReverse)
    {
        _inputs.brakeInput = inputReverse;
    }

    public void OnVehiclePrimaryActionInput(InputPhase inputPhase)
    {
        _inputs.handbrakeInput = inputPhase != InputPhase.OnReleased ? 1 : 0;
    }

    public void OnVehicleSecondaryActionInput(InputPhase inputPhase)
    {
    }

    public void OnVehicleExitInput()
    {
        StopDriving();
    }

#endregion
}
