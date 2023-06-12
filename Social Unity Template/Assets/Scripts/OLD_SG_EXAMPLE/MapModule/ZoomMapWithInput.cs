using System.Collections;
using Mapbox.Unity.Map;
using UnityEngine;

public class ZoomMapWithInput : MonoBehaviour
{
	public float zoomSpeedMouse;
	public float zoomSpeedTouch;

	[SerializeField]
	AbstractMap _map;

	bool _isInitialized = false;

	// Start is called before the first frame update
	void Start()
	{
		_map.OnInitialized += () => _isInitialized = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (_isInitialized)
		{
			if (Input.touchSupported && Input.touchCount == 2)
			{
				ZoomWithTouch();
			}
			else
			{
				ZoomWithMouse();
			}
		}
	}
	
	void ZoomWithTouch()
	{
		Touch touchZero = Input.GetTouch(0);
		Touch touchOne = Input.GetTouch(1);

		Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
		Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

		float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
		float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

		float zoomFactor = 0.05F * (touchDeltaMag - prevTouchDeltaMag) * zoomSpeedTouch;
		ZoomMap(zoomFactor);
	}

	void ZoomWithMouse()
	{
		float zoomFactor = Input.GetAxis("Mouse ScrollWheel") * zoomSpeedMouse;
		ZoomMap(zoomFactor);
	}

	void ZoomMap(float zoomFactor)
	{
		float zoom = _map.Zoom + zoomFactor * Time.deltaTime;
		_map.UpdateMap(zoom);
	}
}

