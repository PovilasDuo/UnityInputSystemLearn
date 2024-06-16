using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference moveInputActionReference;

    [Min(0f)]
    [SerializeField]
    private float maxMoveAngle = 10f;

    [Min(0f)]
    [SerializeField]
    private float moveSpeed = 5f;

    private new Rigidbody rigidbody;

    private Vector3 targetAngles;
    private Vector3 currentAngles;
    private Vector2 inputAxis;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateTargetAngles();
        ClampTargetAngles();
        UpdateCurrentAngles();
        ApplyCurrentAngles();
    }

	private void OnEnable()
	{
        MoveInputAction.performed += OnMovePerformed;
        MoveInputAction.canceled += OnMoveCanceled;
	}

	private void OnDisable()
	{
		MoveInputAction.performed -= OnMovePerformed;
		MoveInputAction.canceled -= OnMoveCanceled;
	}

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        inputAxis = ctx.ReadValue<Vector2>();
    }

	private void OnMoveCanceled(InputAction.CallbackContext ctx)
	{
        targetAngles = Vector3.zero;
        inputAxis = Vector2.zero;
	}

	private void ApplyCurrentAngles()
	{
        rigidbody.MoveRotation(Quaternion.Euler(currentAngles));
	}

	private void UpdateCurrentAngles()
	{
        var time = Time.deltaTime * moveSpeed;
        currentAngles.x = Mathf.LerpAngle(currentAngles.x, targetAngles.x, time);
		currentAngles.z = Mathf.LerpAngle(currentAngles.z, targetAngles.z, time);
	}

	private void ClampTargetAngles()
	{
        targetAngles.x = Mathf.Clamp(targetAngles.x, -maxMoveAngle, maxMoveAngle);
		targetAngles.z = Mathf.Clamp(targetAngles.z, -maxMoveAngle, maxMoveAngle);
	}

	private void UpdateTargetAngles()
	{
        targetAngles.x += inputAxis.y;
        targetAngles.z -= inputAxis.x;
	}

	private InputAction MoveInputAction
    {
        get
        {
            var action = moveInputActionReference.action;
            if (!action.enabled)    action.Enable();
            return action;
        }
    }
}
