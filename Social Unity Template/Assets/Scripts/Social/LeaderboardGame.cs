
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LeaderboardGame : MonoBehaviour
{
	class Player : IComparable
	{
		public string name;
		public int score;
		public Player(string name, int score)
		{
			this.name = name;
			this.score = score;
		}
		
    public int CompareTo(object obj) {
        if (obj == null) return 1;

        Player otherPlayer = obj as Player;
        if (otherPlayer != null)
            return otherPlayer.score.CompareTo(this.score);
        else
           throw new ArgumentException("Object is not a Player");
    }
	}

	public string pageNumberText = "Page {0} / {1}";
	public string getScoresUrl = "get_scores/";
	public string editScoreUrl = "edit_score/";
	public TMP_Text rankColumn;
	public TMP_Text playerColumn;
	public TMP_Text scoreColumn;
	public TMP_Text pageNumberField;
	public Button previousButton;
	public Button nextButton;

	Player[] players;

	int myRank;
	int myScore;

	const int linesPerPage = 10;
	int currentPage = 1;
	int maxPage;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(UpdatePlayers());
	}

	IEnumerator UpdatePlayers()
	{
		using (WWW www = new WWW(Client.BASE_URL + getScoresUrl))
		{
			yield return www;
			string wwwText = www.text.TrimStart();
			Debug.Log(wwwText);
			if (wwwText.StartsWith("0"))
			{
				string[] wwwTextLines = wwwText.Split(":")[1].TrimStart().Split(",");
				players = new Player[wwwTextLines.Length];
				for (int i = 0; i < wwwTextLines.Length; i++)
				{
					string[] lineFields = wwwTextLines[i].Split(" ");
					string playerName = lineFields[0];
					int playerScore = Int32.Parse(lineFields[1]);
					players[i] = new Player(playerName, playerScore);
				}
				Array.Sort(players);
				
				for (int i = 0; i < players.Length; i++)
				{
					if (players[i].name.Equals(Client.username))
					{
						myScore = players[i].score;
						myRank = i + 1;
					}
				}
				maxPage = (int)Math.Ceiling((double)players.Length / (double)linesPerPage);
				Render();
			}
		}
	}

	void Render()
	{
		// Render page number text field
		pageNumberField.text = string.Format(pageNumberText, currentPage, maxPage);
		// Render page turn buttons
		previousButton.interactable = currentPage > 1;
		nextButton.interactable = currentPage < maxPage;
		// Render leaderboard columns
		string rankText = myRank.ToString();
		string playerText = Client.username + " (You)";
		string scoreText = myScore.ToString();
		int minRank = (currentPage - 1) * linesPerPage;
		int maxRank = minRank + linesPerPage - 1;
		if (maxRank >= players.Length)
		{
			maxRank = players.Length - 1;
		}
		for (int i = minRank; i <= maxRank; i++)
		{
			rankText += "\n" + (i + 1);
			playerText += "\n" + players[i].name;
			scoreText += "\n" + players[i].score;
		}
		rankColumn.text = rankText;
		playerColumn.text = playerText;
		scoreColumn.text = scoreText;
	}

	public void NextPage()
	{
		if (currentPage < maxPage)
		{
			currentPage++;
			Render();
		}
	}

	public void PreviousPage()
	{
		if (currentPage > 1)
		{
			currentPage--;
			Render();
		}
	}

	public void ScorePoints(int scoreToGain = 5)
	{
		myScore += scoreToGain;
		StartCoroutine(EditScore(myScore));
	}

	IEnumerator EditScore(int newScore)
	{
		WWWForm form = new WWWForm();
		form.AddField("score", newScore);
		using (WWW www = new WWW(Client.BASE_URL + editScoreUrl, form))
		{
			yield return www;
			string wwwText = www.text.TrimStart();
			Debug.Log(wwwText);
			if (wwwText.StartsWith("0"))
			{
				StartCoroutine(UpdatePlayers());
			}
		}
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

}
