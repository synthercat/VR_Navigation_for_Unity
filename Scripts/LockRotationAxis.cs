using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotationAxis : MonoBehaviour
{
	public Transform cameraRotationY;

	void Awake()
	{
		if (cameraRotationY == null)
			cameraRotationY = GetComponentInParent<Camera> ().transform;
	}

	private void LateUpdate()
	{
		transform.rotation = Quaternion.Euler (0, cameraRotationY.rotation.eulerAngles.y, 0);
	}
}
