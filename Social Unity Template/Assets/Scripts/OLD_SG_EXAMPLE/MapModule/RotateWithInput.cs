using System.Collections;
using UnityEngine;

public class RotateWithInput : MonoBehaviour
{
	public float maxSpeed;

	// Update is called once per frame
	void Update()
	{
		float input = Input.GetAxis("Rotate");
		Vector3 rotation = new Vector3(0.0F, input, 0.0F) * maxSpeed * Time.deltaTime;
		transform.Rotate(rotation);
	}
}
 