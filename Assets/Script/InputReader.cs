using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputControls;

[CreateAssetMenu(fileName = "Input Reader", menuName = "Scriptable Objects/Input Reader")]
public class InputReader : ScriptableObject, IPlayerTouchGamepadActions, IPlayerKeyboardMouseActions
{
    public Action<bool> ShootEvent;
    public Vector2 MoveValue{get; private set;}
    public Vector2 LookValue{get; private set;}
    
    private InputControls controls;

    private bool _isJoystickMovement;
    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new InputControls();
            controls.PlayerKeyboardMouse.SetCallbacks(this);
            controls.PlayerTouchGamepad.SetCallbacks(this);
        }
        
        controls.PlayerKeyboardMouse.Enable();
    }

    private void OnDisable()
    {
        controls.PlayerKeyboardMouse.Disable();
        controls.PlayerTouchGamepad.Disable();
    }
    
    public void EnableGamePadControls()
    {
        controls.PlayerKeyboardMouse.Disable();
        controls.PlayerTouchGamepad.Enable();
        _isJoystickMovement = true;
    }
    
    public void DisableGamePadControls()
    {
        controls.PlayerKeyboardMouse.Enable();
        controls.PlayerTouchGamepad.Disable();
        _isJoystickMovement = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookValue = context.ReadValue<Vector2>();
        LookValue -= new Vector2(Screen.width/2, Screen.height/2);
        Debug.Log(LookValue);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShootEvent?.Invoke(true);
        }
        else if(context.canceled)
        {
            ShootEvent?.Invoke(false);
        }
    }
}
