using System.Collections;
using UnityEngine;

public class DollyBehavior : MonoBehaviour
{
	public Transform focus;
	public float rotateSpeedKeyboard;
	public float moveSpeedKeyboard;
	public float rotateSpeedTouch;
	public float moveSpeedTouch;
	bool _isFocussed = true;

	// Start is called before the first frame update
	void Start()
	{
		JumpToFocus();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.touchSupported)
		{
			switch (Input.touchCount)
			{
				case 1:
					_isFocussed = false;
					MoveWithTouch();
					break;
				case 2:
					_isFocussed = false;
					RotateWithTouch();
					break;
				case 3:
					_isFocussed = true;
					MoveAndRotateWithKeyboard();
					break;
				default:
					MoveAndRotateWithKeyboard();
					break;
			}
		}
		else
		{
			MoveAndRotateWithKeyboard();
		}
	}

	void MoveWithTouch()
	{
		Touch touch = Input.GetTouch(0);
		Vector2 translation2d = -touch.deltaPosition * touch.deltaTime * moveSpeedTouch;
		Vector3 translation3d = new Vector3(translation2d.x, 0.0F, translation2d.y);
		transform.Translate(translation3d);
	}

	void RotateWithTouch()
	{
		Touch touchZero = Input.GetTouch(0);
		Touch touchOne = Input.GetTouch(1);

		Vector2 touchDifference = touchOne.position - touchZero.position;
		Vector2 touchDifferencePrev = touchDifference - touchOne.deltaPosition + touchZero.deltaPosition;
		float angle = Vector2.SignedAngle(touchDifferencePrev, touchDifference);
		Vector3 rotation = new Vector3(0.0F, angle, 0.0F) * rotateSpeedTouch * Time.deltaTime;
		transform.Rotate(rotation);
	}
	
	void MoveAndRotateWithKeyboard()
	{
		float rotateInput = Input.GetAxis("Rotate");
		float xInput = Input.GetAxis("Horizontal");
		float zInput = Input.GetAxis("Vertical");

		// Set Focus based on input
		if (Input.GetButtonDown("Focus"))
		{
			_isFocussed = true;
		}
		else if (rotateInput != 0.0F || xInput != 0.0F || zInput != 0.0F)
		{
			_isFocussed = false;
		}

		if (_isFocussed) {
			JumpToFocus();
		}
		else
		{
			// Rotate around the Y-Axis based on input and rotateSpeedKeyboard
			Vector3 rotation = new Vector3(0.0F, rotateInput, 0.0F) * rotateSpeedKeyboard * Time.deltaTime;
			transform.Rotate(rotation);
			// Move on the X-Z-Plane on input and moveSpeed
			Vector3 translation = new Vector3(xInput, 0.0F, zInput);
			translation = Vector3.ClampMagnitude(translation, 1.0F) * moveSpeedKeyboard * Time.deltaTime;
			transform.Translate(translation);
		}
	}

	void JumpToFocus()
	{
		transform.rotation = focus.rotation;
		transform.position = focus.position;
	}
}
