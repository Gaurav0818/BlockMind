using System;
using Unity.VisualScripting;
using UnityEngine;

public class ToggleJoystick : MonoBehaviour
{
    public GameObject joystick;

    public InputReader inputReader;

    public void OnEnable()
    {
        if (inputReader == null)
            inputReader = GetComponent<InputReader>();
    }

    public void ToggleJoystickButton()
    {
        joystick.SetActive(!joystick.activeSelf);
        if(joystick.activeSelf)
        {
            inputReader.EnableGamePadControls();
        }
        else
        {
            inputReader.DisableGamePadControls();
        }
    }
}
