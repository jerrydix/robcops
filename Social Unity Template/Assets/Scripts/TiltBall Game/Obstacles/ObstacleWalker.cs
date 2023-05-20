using System.Collections;

using UnityEngine;

public class ObstacleWalker : MonoBehaviour
{
  public float speed = 1;
  public float minX = -2;
  public float maxX = 2;
  public float minY = -1.4f;
  public float maxY = 6;
  Vector3 direction;

  // Start is called before the first frame update
  void Start()
  {
    ChooseDirection();
  }

  void FixedUpdate()
  {
    if (IsOutOfBounds())
    {
      ChooseDirection();
    }
    transform.position += direction * speed * Time.fixedDeltaTime;
  }
  bool IsOutOfBounds()
  {
    float x = transform.position.x;
    float y = transform.position.y;
    return x < minX || x > maxX || y < minY || y > maxY;
  }
  void ChooseDirection()
  {
    var goal = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
    direction = (goal - transform.position).normalized;
  }
}
