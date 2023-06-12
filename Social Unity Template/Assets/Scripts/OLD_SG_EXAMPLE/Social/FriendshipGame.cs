
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FriendshipGame : MonoBehaviour
{
	class Friend : IComparable
	{
		public string name;
		public int level;
		public Friend(string name, int level)
		{
			this.name = name;
			this.level = level;
		}
		
    public int CompareTo(object obj) {
        if (obj == null) return 1;

        Friend otherFriend = obj as Friend;
        if (otherFriend != null)
            return otherFriend.level.CompareTo(this.level);
        else
           throw new ArgumentException("Object is not a Friend");
    }
	}

	public string pageNumberText = "Page {0} / {1}";
	public string getFriendsUrl = "get_friends/";
	public string getNamesUrl = "get_names/";
	public string addFriendUrl = "add_friend/";
	public TMP_Text friendColumn;
	public TMP_Text levelColumn;
	public TMP_Text pageNumberField;
	public Button previousButton;
	public Button nextButton;
	public TMP_InputField nameInput;
	public Button friendButton;

	Friend[] friends;
	HashSet<string> usernames;

	const int linesPerPage = 8;
	int currentPage = 1;
	int maxPage;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(UpdateFriends());
		StartCoroutine(UpdateUsernames());
	}

	IEnumerator UpdateFriends()
	{
		using (WWW www = new WWW(Client.BASE_URL + getFriendsUrl))
		{
			yield return www;
			string wwwText = www.text.TrimStart();
			Debug.Log(wwwText);
			if (wwwText.StartsWith("0"))
			{
				string[] wwwTextLines = wwwText.Split(":")[1].TrimStart()
				.Split(",", StringSplitOptions.RemoveEmptyEntries);
				friends = new Friend[wwwTextLines.Length];
				for (int i = 0; i < wwwTextLines.Length; i++)
				{
					string[] lineFields = wwwTextLines[i].Split(" ");
					string friendName = lineFields[0];
					int friendLevel = Int32.Parse(lineFields[1]);
					friends[i] = new Friend(friendName, friendLevel);
				}
				Array.Sort(friends);
				maxPage = (int)Math.Ceiling((double)friends.Length / (double)linesPerPage);
				if (maxPage <= 0)
				{
					maxPage = 1;
				}
				Render();
			}
		}
	}

	IEnumerator UpdateUsernames()
	{
		using (WWW www = new WWW(Client.BASE_URL + getNamesUrl))
		{
			yield return www;
			string wwwText = www.text.TrimStart();
			Debug.Log(wwwText);
			if (wwwText.StartsWith("0"))
			{
				usernames = new HashSet<string>(wwwText.Split(":")[1].TrimStart().Split());
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
		string friendText = "";
		string levelText = "";
		int minRank = (currentPage - 1) * linesPerPage;
		int maxRank = minRank + linesPerPage - 1;
		if (maxRank >= friends.Length)
		{
			maxRank = friends.Length - 1;
		}
		for (int i = minRank; i <= maxRank; i++)
		{
			friendText += friends[i].name + "\n";
			levelText += friends[i].level + "\n";
		}
		friendColumn.text = friendText;
		levelColumn.text = levelText;
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

	public void VerifyInput()
	{
		if (nameInput.text.Equals(Client.username))
		{
			friendButton.interactable = false;
			return;
		}
		if (!usernames.Contains(nameInput.text))
		{
			friendButton.interactable = false;
			return;
		}
		foreach (Friend friend in friends)
		{
			if (friend.name.Equals(nameInput.text)) 
			{
				friendButton.interactable = false;
				return;
			}
		}
		friendButton.interactable = true;
	}

	public void CallAddFriend()
	{
		StartCoroutine(AddFriend(nameInput.text));
		nameInput.text = "";
	}

	IEnumerator AddFriend(string username)
	{
		WWWForm form = new WWWForm();
		form.AddField("name", username);
		using (WWW www = new WWW(Client.BASE_URL + addFriendUrl, form))
		{
			yield return www;
			string wwwText = www.text.TrimStart();
			Debug.Log(wwwText);
			if (wwwText.StartsWith("0"))
			{
				StartCoroutine(UpdateFriends());
			}
		}
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

}
