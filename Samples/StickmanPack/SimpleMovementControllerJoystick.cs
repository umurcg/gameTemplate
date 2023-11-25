using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using Samples.JoystickUI;
using UnityEngine;

public class SimpleMovementControllerJoystick : MonoBehaviour
{
    public float movementSpeed = 1f;
    JoystickController joystickController;

    public void Start()
    {
        joystickController = FindObjectOfType<JoystickController>();
        if (joystickController == null)
        {
            Debug.LogError("No JoystickController found in scene");
            return;
        }

        joystickController.OnJoystickUpdate += OnJoystickUpdate;
    }

    private void OnJoystickUpdate(JoystickController.JoystickData obj)
    {
        var movement = obj.Direction;
        var movement3D = new Vector3(movement.x, 0, movement.y);
        transform.position += movement3D * movementSpeed * Time.deltaTime;
        if (movement3D.magnitude > 0.1f) transform.rotation = Quaternion.LookRotation(movement3D);
    }


    private void OnDestroy()
    {
        if (joystickController != null)
        {
            joystickController.OnJoystickUpdate -= OnJoystickUpdate;
        }
    }
}