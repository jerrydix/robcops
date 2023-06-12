using System.Collections;

using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
  public bool[] difficultyIncreases;
  public GameObject[] obstacles;
  int currentDifficulty = 0;

  // Start is called before the first frame update
  void Start()
  {
    // Shuffle the obstacle array
    GameObject tempObstacle;
    for (int i = 0; i < obstacles.Length - 1; i++)
    {
      int rnd = Random.Range(i, obstacles.Length);
      tempObstacle = obstacles[rnd];
      obstacles[rnd] = obstacles[i];
      obstacles[i] = tempObstacle;
    }
  }

  public void Spawn(int currentScore)
  {
    bool canRaiseDifficulty = currentDifficulty < obstacles.Length && currentScore < difficultyIncreases.Length;
    if (canRaiseDifficulty && difficultyIncreases[currentScore])
    {
      obstacles[currentDifficulty].SetActive(true);
      currentDifficulty++;
    }
  }
}
