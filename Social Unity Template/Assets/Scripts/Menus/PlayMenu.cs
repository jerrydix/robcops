using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
	public void LoadGame(string gameName)
	{
		SceneManager.LoadScene(gameName + "Game");
	}
}
