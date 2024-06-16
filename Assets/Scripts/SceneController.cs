using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference reloadIAR;

	private static void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private InputAction ReloadInputAction
	{
		get
		{
			var action = reloadIAR.action;
			if (!action.enabled) action.Enable();
			return action;
		}
	}

	private void OnEnable()
	{
		ReloadInputAction.performed += OnReloadPerfomed;
	}

	private void OnDisable()
	{
		ReloadInputAction.performed -= OnReloadPerfomed;
	}

	private void OnReloadPerfomed(InputAction.CallbackContext context)
	{
		ReloadScene();
	}

	private void OnTriggerEnter(Collider other)
	{
		ReloadScene();
	}
}
