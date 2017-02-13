using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Script makes sure you can read the text even if the head tilts (Ctrl + mouse for editor preview)

public class CopyZ : MonoBehaviour
{
	public Transform copyZAxisFrom;

	void Awake()
	{
		if (copyZAxisFrom == null)
			copyZAxisFrom = GetComponentInParent<Camera> ().transform;
	}

		private void LateUpdate()
	{
		transform.rotation = Quaternion.Euler (transform.eulerAngles.x, transform.eulerAngles.y, copyZAxisFrom.rotation.eulerAngles.z);
	}
}
