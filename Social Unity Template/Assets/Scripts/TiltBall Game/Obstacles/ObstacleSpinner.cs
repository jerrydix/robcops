using System.Collections;

using UnityEngine;

public class ObstacleSpinner : MonoBehaviour
{
  public float speed = 20;
  public bool clockwise = false;
  // Start is called before the first frame update
  void Start()
  {

  }

  void FixedUpdate()
  {
    transform.Rotate(0, 0, speed * Time.fixedDeltaTime * (clockwise ? -1 : 1));
  }
}
