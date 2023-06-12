using System.Collections;
using UnityEngine;

public class MoveWithInput : MonoBehaviour
{
	public float maxSpeed;

	// Update is called once per frame
	void Update()
	{
		Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0F, Input.GetAxis("Vertical"));
		Vector3 translation = Vector3.ClampMagnitude(input, 1.0F) * maxSpeed * Time.deltaTime;
		transform.Translate(translation);
	}
}
 