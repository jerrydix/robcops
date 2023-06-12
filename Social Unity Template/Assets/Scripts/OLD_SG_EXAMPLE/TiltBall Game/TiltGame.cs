using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TiltGame : MonoBehaviour
{
  public TMP_Text infoField;
  public TMP_Text scoreField;
  public Button exitButton;
  public int score = 0;

  public string host = "";
  public bool isHost;
  public bool match_started = false;
  public bool matchOver = false;

  public GameObject ballPrefab;
  public GameObject buttonPrefab;
  public ObstacleSpawner obstacleSpawner;

  public float ballSpawnHeight = 8.6f;
  public GameObject ball = null;

  public float buttonMinX = -1.5f;
  public float buttonMaxX = 1.5f;
  public float buttonMinY = -1f;
  public float buttonMaxY = 5f;

  public float start_update_time = 1f;
  public float pass_update_time = 0.5f;
  float timer = 0f;

  public string getMatchUrl = "get_match/";
  public string endMatchUrl = "end_match/";

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (host == "" || matchOver)
    {
      return;
    }
    if (!match_started)
    {
      timer += Time.deltaTime;
      if (timer >= start_update_time)
      {
        timer = 0f;
        // Check for match start  
        StartCoroutine(GetMatchStatus());
      }
    }
    else if (!ball)
    {
      timer += Time.deltaTime;
      if (timer >= pass_update_time)
      {
        timer = 0f;
        // Check for ball pass
        StartCoroutine(GetMatchStatus());
      }
    }
  }
  IEnumerator GetMatchStatus()
  {
    WWWForm form = new WWWForm();
    form.AddField("host", host);
    using (WWW www = new WWW(Client.BASE_URL + getMatchUrl, form))
    {
      yield return www;
      string wwwText = www.text.TrimStart();
      Debug.Log(wwwText);
      if (wwwText.StartsWith("0"))
      {
        match_started = true;
        UpdateScore();
        // host has the ball
        if (isHost)
        {
          // I have the ball
          ReceiveBall(wwwText);
        }
      }
      else if (www.text.StartsWith("1"))
      {
        match_started = true;
        UpdateScore();
        // guest has the ball
        if (!isHost)
        {
          // I have the ball
          ReceiveBall(wwwText);
        }
      }
      else if (www.text.StartsWith("2"))
      {
        // The match was ended by the other player
        EndMatchHere();
      }
    }
  }
  void ReceiveBall(string wwwText)
  {
    SpawnBall(wwwText);
    SpawnButton();
    SpawnObstacles();
  }
  void SpawnBall(string wwwText)
  {
    var splitText = wwwText.Split();
    float xPos = float.Parse(splitText[1]);
    float xVel = float.Parse(splitText[2]);
    float yVel = -float.Parse(splitText[3]);
    ball = Instantiate(ballPrefab, new Vector3(xPos, ballSpawnHeight, 0), Quaternion.identity);
    ball.GetComponent<Rigidbody>().velocity = new Vector3(xVel, yVel, 0);
    TiltBallBehavior ballBehavior = ball.GetComponent<TiltBallBehavior>();
    ballBehavior.host = host;
    ballBehavior.host_has_ball = isHost;
    ballBehavior.tiltGame = this;
  }
  void SpawnButton()
  {
    var position = new Vector3(Random.Range(buttonMinX, buttonMaxX), Random.Range(buttonMinY, buttonMaxY), 0);
    Instantiate(buttonPrefab, position, Quaternion.identity);
  }
  void SpawnObstacles()
  {
    obstacleSpawner.Spawn(score);
  }
  void UpdateScore()
  {
    scoreField.text = "Score: " + score;
  }
  public void ScorePoints(int pointsToScore)
  {
    score += pointsToScore;
    UpdateScore();
  }
  public void EndMatch()
  {
    EndMatchHere();
    StartCoroutine(EndMatchOnServer());
  }
  void EndMatchHere()
  {
    matchOver = true;
    infoField.text = "GAME OVER";
    exitButton.gameObject.SetActive(true);
  }
  IEnumerator EndMatchOnServer()
  {
    WWWForm form = new WWWForm();
    form.AddField("host", host);
    using (WWW www = new WWW(Client.BASE_URL + endMatchUrl, form))
    {
      yield return www;
      Debug.Log(www.text);
    }
  }
}
