using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public static GameManager gm;

	public string levelAfterVictory;
	public string levelAfterGameOver;

	public int score = 0;
	public int highscore = 0;
	public int startLives = 3;
	public int lives = 3;

	public TMP_Text UIScore;
	public TMP_Text UIHighScore;
	
	public TMP_Text UILevel;
	public GameObject[] UIExtraLives;
	public GameObject UIGamePaused;

	private GameObject _player;
	private Vector3 _spawnLocation;

	private void Awake()
	{
		if (gm == null)
		{
			gm = this.GetComponent<GameManager>();
		}

		setupDefaults();
	}

	private void Update() 
	{

        if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			if (Time.timeScale > 0f) 
			{
				UIGamePaused.SetActive(true);
				Time.timeScale = 0f;
			} else 
			{
				Time.timeScale = 1f;
				UIGamePaused.SetActive(false);
			}
		}
	}

	private void setupDefaults()
	{ 
		_player = GameObject.FindGameObjectWithTag("Player");

        _spawnLocation = _player.transform.position;

		if (levelAfterVictory=="") 
		{
			Debug.LogWarning("levelAfterVictory not specified, defaulted to current level");
		    levelAfterVictory = SceneManager.GetActiveScene().name;
		}
		
		if (levelAfterGameOver=="") 
		{
			Debug.LogWarning("levelAfterGameOver not specified, defaulted to current level");
			levelAfterGameOver = SceneManager.GetActiveScene().name;
        }

		if (UIScore==null)
			Debug.LogError ("Need to set UIScore on Game Manager.");
		
		if (UIHighScore==null)
			Debug.LogError ("Need to set UIHighScore on Game Manager.");
		
		if (UILevel==null)
			Debug.LogError ("Need to set UILevel on Game Manager.");
		
		if (UIGamePaused==null)
			Debug.LogError ("Need to set UIGamePaused on Game Manager.");
		
		refreshPlayerState();

		refreshGUI();
	}
	private void refreshPlayerState() 
	{
		lives = PlayerPrefManager.GetLives();

		if (lives <= 0) 
		{
			PlayerPrefManager.ResetPlayerState(startLives,false);

			lives = PlayerPrefManager.GetLives();
		}

		score = PlayerPrefManager.GetScore();
		highscore = PlayerPrefManager.GetHighscore();

		PlayerPrefManager.UnlockLevel();
	}

	private void refreshGUI() 
	{
		UIScore.text = "score: "+score.ToString();
		UIHighScore.text = "highscore: "+highscore.ToString ();
		UILevel.text = SceneManager.GetActiveScene().name;

        for (int i=0; i<UIExtraLives.Length; i++) 
		{
			if (i<(lives-1)) 
			{
				UIExtraLives[i].SetActive(true);
			} 
			else 
			{
				UIExtraLives[i].SetActive(false);
			}
		}
	}

	public void AddPoints(int amount)
	{
		score += amount;

		UIScore.text = "score: "+score.ToString();

		if (score>highscore) 
		{
			highscore = score;
			UIHighScore.text = "highscore: "+score.ToString();
		}
	}
	public void ResetGame() 
	{
		lives--;

		refreshGUI();

		if (lives <= 0) 
		{
			PlayerPrefManager.SavePlayerState(score,highscore,lives);

			SceneManager.LoadScene(levelAfterGameOver);
		} 
		else 
		{
			_player.GetComponent<PlayerControler>().Respawn(_spawnLocation);
		}
	}

	public void LevelCompete() 
	{
		PlayerPrefManager.SavePlayerState(score,highscore,lives);
		StartCoroutine(LoadNextLevel());
	}

	IEnumerator LoadNextLevel() 
	{
		yield return new WaitForSeconds(3.5f);
		SceneManager.LoadScene(levelAfterVictory);
	}
}
