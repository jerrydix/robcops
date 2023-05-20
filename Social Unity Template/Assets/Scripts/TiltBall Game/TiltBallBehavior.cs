using System.Collections;

using UnityEngine;

public class TiltBallBehavior : MonoBehaviour
{
  Rigidbody m_Rigidbody;
  public float input_strength = 5f;
  Vector3 force = Vector3.zero;
  Vector3 input;

  public bool activated_portal = false;
  public float pass_height = 8f;

  public string host = "";
  public bool host_has_ball;

  public TiltGame tiltGame;

  public string passBallUrl = "pass_ball/";

  void Start()
  {
    //Fetch the Rigidbody from the GameObject with this script attached
    m_Rigidbody = GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
    if (activated_portal && transform.position.y >= pass_height)
    {
      activated_portal = false;
      // Pass
      host_has_ball = !host_has_ball;
      StartCoroutine(PassBall());
      return;
    }

#if UNITY_EDITOR
    input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
#else
    input = new Vector3(Input.acceleration.x, Input.acceleration.y, 0);
#endif

    force = input * input_strength;
    m_Rigidbody.velocity += force * Time.fixedDeltaTime;
  }
  IEnumerator PassBall()
  {
    WWWForm form = new WWWForm();
    form.AddField("host", host);
    form.AddField("host_has_ball", host_has_ball ? "true" : "false");
    form.AddField("position", transform.position.x.ToString("R"));
    form.AddField("velocity_x", m_Rigidbody.velocity.x.ToString("R"));
    form.AddField("velocity_y", m_Rigidbody.velocity.y.ToString("R"));
    using (WWW www = new WWW(Client.BASE_URL + passBallUrl, form))
    {
      yield return www;
      string wwwText = www.text.TrimStart();
      Debug.Log(wwwText);
      if (wwwText.StartsWith("0"))
      {
        // Successfully passed, ball is gone
        gameObject.Destroy();
      }
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "PortalButton")
    {
      // Hit the button that activates the portal
      activated_portal = true;
      other.gameObject.Destroy();
      tiltGame.ScorePoints(1);
    }
    else if (other.gameObject.tag == "Obstacle")
    {
      // An Obstacle was hit, so the match ends
      tiltGame.EndMatch();
      gameObject.Destroy();
    }
  }
}
