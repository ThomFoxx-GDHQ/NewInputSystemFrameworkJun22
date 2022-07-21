using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.Player;
using Game.Scripts.LiveObjects;
using System;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions _input;
    [SerializeField] private Player _player;
    [SerializeField] private Laptop _laptop;
    [SerializeField] private Drone _drone;
    [SerializeField] private Forklift _lift;
    [SerializeField] private Crate _crate;
    

    private void Start()
    {
        _input = new PlayerInputActions();
        _input.Player.Enable();
        _input.Player.Rotate.performed += Rotate_performed;
        _input.Player.Rotate.canceled += Rotate_canceled;
        _input.Player.Walk.performed += Walk_performed;
        _input.Player.Walk.canceled += Walk_canceled;
        _input.Laptop.Exit.performed += Laptop_Exit_performed;
        _input.Laptop.Switch.performed += Switch_performed;
        _input.Drone.Climb.performed += Climb_performed;
        _input.Drone.Descend.performed += Descend_performed;
        _input.Drone.Rotate.performed += Drone_Rotate_performed;
        _input.Drone.Tilt.performed += Tilt_performed;
        _input.Drone.Climb.canceled += Climb_canceled;
        _input.Drone.Descend.canceled += Descend_canceled;
        _input.Drone.Rotate.canceled += Drone_Rotate_canceled;
        _input.Drone.Tilt.canceled += Tilt_canceled;
        _input.Drone.Exit.performed += Drone_Exit_performed;
        _input.ForkLift.Move.performed += Move_performed;
        _input.ForkLift.Move.canceled += Move_canceled;
        _input.ForkLift.Turn.performed += Turn_performed;
        _input.ForkLift.Turn.canceled += Turn_canceled;
        _input.ForkLift.Lift.performed += Lift_performed;
        _input.ForkLift.Lift.canceled += Lift_canceled;
        _input.ForkLift.Exit.performed += Exit_performed;
        _input.Destructible.Punch.started += Punch_started;
        _input.Destructible.Punch.canceled += Punch_canceled;
        _input.Destructible.Punch.performed += Punch_performed;
        _input.Destructible.Exit.performed += Destrucible_Exit_performed;
    }

    private void Punch_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _crate.PunchStart();
    }

    private void Punch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _crate.PowerPunch();
    }

    private void Punch_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _crate.ShortPunch();
    }

    private void Destrucible_Exit_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private void Exit_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _lift.ExitDriveMode();
        EnablePlayerMap();
    }

    private void Lift_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _lift.SetLift(obj.ReadValue<float>());
    }

    private void Lift_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _lift.SetLift(0f);
    }

    private void Turn_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _lift.SetTurn(obj.ReadValue<float>());
    }

    private void Turn_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _lift.SetTurn(0f);
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _lift.SetMove(obj.ReadValue<float>());
    }

    private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _lift.SetMove(0f);
    }

    private void Drone_Exit_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _drone.EndFlight();
        EnablePlayerMap();
    }

    private void Tilt_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _drone.SetTilt(obj.ReadValue<Vector2>());
    }

    private void Tilt_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _drone.SetTilt(Vector2.zero);
    }
    
    private void Drone_Rotate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _drone.SetRotation(obj.ReadValue<float>());
    }

    private void Drone_Rotate_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _drone.SetRotation(0f);
    }

    private void Descend_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _drone.SetLift(-1);
    }

    private void Descend_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _drone.SetLift(0);
    }

    private void Climb_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _drone.SetLift(1);
    }

    private void Climb_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _drone.SetLift(0);
    }

    private void Switch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _laptop.SwitchCameras();
    }

    private void Laptop_Exit_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _laptop.EndHack();
        EnablePlayerMap();
    }

    private void Walk_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _player.SetWalk(obj.ReadValue<float>());
    }

    private void Walk_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _player.SetWalk(0f);
    }

    private void Rotate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _player.SetRotation(obj.ReadValue<float>());
    }

    private void Rotate_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _player.SetRotation(0f);
    }

    public void EnablePlayerMap()
    {
        _input.Laptop.Disable();
        _input.Drone.Disable();
        _input.ForkLift.Disable();
        _input.Destructible.Disable();
        _input.Player.Enable();
    }

    public void EnableLaptopMap()
    {
        _input.Player.Disable();
        _input.Drone.Disable();
        _input.ForkLift.Disable();
        _input.Destructible.Disable();
        _input.Laptop.Enable();
    }

    public void EnableDroneMap()
    {
        _input.Player.Disable();
        _input.Laptop.Disable();
        _input.ForkLift.Disable();
        _input.Destructible.Disable();
        _input.Drone.Enable();
    }

    public void EnableForkliftMap()
    {
        _input.Player.Disable();
        _input.Laptop.Disable();
        _input.Drone.Disable();
        _input.Destructible.Disable();
        _input.ForkLift.Enable();
    }

    public void EnableCratemap()
    {
        _input.Player.Disable();
        _input.Laptop.Disable();
        _input.Drone.Disable();
        _input.ForkLift.Disable();
        _input.Destructible.Enable();
    }
}