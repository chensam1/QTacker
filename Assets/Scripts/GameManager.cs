using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public static int GAME_STATE_PLAYING = 0;
    public static int GAME_STATE_MOVING = 1;
	public static int GAME_STATE_SPAWNING = 2;
	public static int GAME_STATE_CLEARING = 3;
	public static int GAME_STATE_OVER = 4;

	public int gameState = GAME_STATE_SPAWNING;
	public GameObject ballPrefab;

	int startQueueCount = 0;
	int leftStackCount = 0;
	int centerStackCount = 0;
	int rightStackCount = 0;
	int finalQueueCount = 0;

	// moving
	GameObject ballToMove = null;
	float xToMove = 0f;
	float yToMove = 0f;
	bool bSpawnAfterMoving = false;
	bool bTryClearBallsAfterMoving = false;
	bool bMovingUpPhase = true;

	// final queue
	GameObject[] finalBalls = new GameObject[6];
	bool clearingSearchPhase = true;
	int clearingStartIdx = -1;

	// score/gameover
	public int score = 0;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI hiscoreText;
	public TextMeshProUGUI gameOverText;

	// animation & sound
	public ParticleSystem explosionVFX;
	public AudioClip explosionAudioClip;
	public AudioClip gameOverAudioClip;
	public AudioClip moveAudioClip;
	private AudioSource audioSource;
	float totalDeltaTime = 0f;

	// disable list
	public Button buttonNotWorkWhenGameOver1;
	public Button buttonNotWorkWhenGameOver2;
	public Button buttonNotWorkWhenGameOver3;
	public Button buttonNotWorkWhenGameOver4;
	public Button buttonNotWorkWhenGameOver5;
	public Button buttonNotWorkWhenGameOver6;

	// Start is called before the first frame update
	void Start()
    {
		score = 0;
		audioSource = GetComponent<AudioSource>();
		hiscoreText.text = "Hiscore\n" + DataManager.Instance.highScore.ToString();
		switch (DataManager.Instance.gameLevel)
		{
			case 0:
				RandomColorManager.colorSize = 6;
				break;
			case 1:
				RandomColorManager.colorSize = 7;
				break;
			case 2:
				RandomColorManager.colorSize = 8;
				break;
		}
	}

	// Update is called once per frame
	void Update()
    {
		if (gameState == GAME_STATE_SPAWNING)
		{
			doGameStateSpawning(Time.deltaTime);
		}
		else if (gameState == GAME_STATE_MOVING)
		{
			doGameStateMoving(Time.deltaTime);
		}
		else if (gameState == GAME_STATE_CLEARING)
		{
			doGameClearing(Time.deltaTime);
		}
	}

	List<GameObject> getGameBallsByX(int x) // X is position of Queue/Stack
	{
		List<GameObject> ballObjects = new List<GameObject>();
		GameObject[] gameBalls = GameObject.FindGameObjectsWithTag("GameBall");
		foreach (GameObject ball in gameBalls)
		{
			if (Mathf.Abs(ball.transform.position.x - x) < 60)
			{
				ballObjects.Add(ball);
			}
		}
		return ballObjects;
	}

	GameObject getHighestGameBallsByX(int x) // X is position of Queue/Stack
	{
		List<GameObject> ballObjects = new List<GameObject>();
		GameObject[] gameBalls = GameObject.FindGameObjectsWithTag("GameBall");
		GameObject highestBall = null;
		float highestY = -100000f;
		foreach (GameObject ball in gameBalls)
		{
			if ((Mathf.Abs(ball.transform.position.x - x) < 60) && (ball.transform.position.y > highestY))
			{
				highestBall = ball;
				highestY = ball.transform.position.y;
			}
		}
		return highestBall;
	}

	GameObject getLowestGameBallsByX(int x) // X is position of Queue/Stack
	{
		List<GameObject> ballObjects = new List<GameObject>();
		GameObject[] gameBalls = GameObject.FindGameObjectsWithTag("GameBall");
		GameObject lowestBall = null;
		float lowestY = 100000f;
		foreach (GameObject ball in gameBalls)
		{
			if ((Mathf.Abs(ball.transform.position.x - x) < 60) && (ball.transform.position.y < lowestY))
			{
				lowestBall = ball;
				lowestY = ball.transform.position.y;
			}
		}
		return lowestBall;
	}

	void doGameStateSpawning(float deltaTime)
    {
		if (startQueueCount >= 6)
		{
			gameState = GAME_STATE_PLAYING;
			return;
		}
		int genY = -390;
		bool bMoveUp = false;
		float distanceLeft = 0f;
		GameObject lowestBall = getLowestGameBallsByX(-600);
		if (null != lowestBall)
		{
			distanceLeft = 118 - lowestBall.transform.position.y + genY; // 118 (ball diameter)
			if (distanceLeft > 0)
			{
				bMoveUp = true;
			}
		}
		if (bMoveUp)
		{
			float moveInThisStep = deltaTime * 360;
			if (moveInThisStep > distanceLeft)
			{
				moveInThisStep = distanceLeft + 1;
			}
			List<GameObject> gameBalls = getGameBallsByX(-600);
			foreach (GameObject ball in gameBalls)
			{
				Vector3 currentPosition = ball.transform.position;
				currentPosition.y += moveInThisStep;
				ball.transform.position = currentPosition;
			}
		}
		else
		{
			Instantiate(ballPrefab, new Vector3(-600, genY, -110), ballPrefab.transform.rotation);
			startQueueCount = startQueueCount + 1;
		}
	}

	void doGameStateMoving(float deltaTime)
	{
		if (ballToMove == null) { return; }
		Vector3 currentPosition = ballToMove.transform.position;
		if ((Mathf.Abs(currentPosition.x - xToMove) < 5f) && (Mathf.Abs(currentPosition.y - yToMove) < 5f)) // final position
		{
			Rigidbody rigidbody = ballToMove.GetComponent<Rigidbody>();
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			if (bSpawnAfterMoving)
			{
				gameState = GAME_STATE_SPAWNING;
			}
			else
			{
				if (bTryClearBallsAfterMoving)
				{
					doScoring();
					finalBalls[finalQueueCount - 1] = ballToMove;
					totalDeltaTime = 0f;
					clearingSearchPhase = true;
					gameState = GAME_STATE_CLEARING;
				}
				else
				{
					gameState = GAME_STATE_PLAYING;
				}
			}
			return;
		}
		float moveInThisStep = deltaTime * 1500;
		if (bMovingUpPhase)
		{
			currentPosition.y += moveInThisStep;
			ballToMove.transform.position = currentPosition;
			if (currentPosition.y > 700f)
			{
				currentPosition.x = xToMove;
				ballToMove.transform.position = currentPosition;
				bMovingUpPhase = false;
			}
		}
		else
		{
			currentPosition.y -= moveInThisStep;
			if (currentPosition.y < yToMove)
			{
				currentPosition.y = yToMove;
			}
			ballToMove.transform.position = currentPosition;
		}
	}

	private void moveBall(GameObject ball, float newX, float newY)
	{
		gameState = GAME_STATE_MOVING;
		ballToMove = ball;
		xToMove = newX;
		yToMove = newY;
		bSpawnAfterMoving = (Mathf.Abs(ballToMove.transform.position.x + 600) < 60);
		bTryClearBallsAfterMoving = (Mathf.Abs(xToMove - 600) < 60);
		bMovingUpPhase = true;
		audioSource.PlayOneShot(moveAudioClip, 1f);
	}

	private void doScoring()
	{
		score += (1 << DataManager.Instance.gameLevel);
		scoreText.text = "Score\n" + score.ToString();
		if (score > DataManager.Instance.highScore)
		{
			DataManager.Instance.highScore = score;
			hiscoreText.text = "Hiscore\n" + score.ToString();
		}
	}

	private void doGameClearing(float deltaTime)
	{
		if (checkGameOver())
		{
			return;
		}
		if (clearingSearchPhase)
		{
			if (finalQueueCount >= 3)
			{
				clearingStartIdx = finalQueueCount - 1;
				int colorIndex = getColorIndexOfFinalBall(clearingStartIdx);
				while (getColorIndexOfFinalBall(clearingStartIdx) == colorIndex)
				{
					clearingStartIdx--;
				}
				if (finalQueueCount - clearingStartIdx - 1 >= 3)
				{
					audioSource.PlayOneShot(explosionAudioClip, 1f);
					for (int i = clearingStartIdx + 1; i < finalQueueCount; i++)
					{
						Destroy(finalBalls[i]);
						finalBalls[i] = null;
					}
					finalQueueCount = clearingStartIdx + 1;
					clearingSearchPhase = false;
					Vector3 expolosionPos = explosionVFX.transform.position;
					expolosionPos.y = -260 + 118 * finalQueueCount;
					explosionVFX.transform.position = expolosionPos;
					explosionVFX.Play();
					return;
				}
			}
			gameState = GAME_STATE_PLAYING;
			return;
		}
		totalDeltaTime += deltaTime;
		if (totalDeltaTime > 1)
		{
			gameState = GAME_STATE_PLAYING;
		}
	}

	private int getColorIndexOfFinalBall(int finalIdx)
	{
		if ((finalIdx >= finalQueueCount) || (finalIdx < 0))
		{
			return 100000; // unlimited value
		}
		RandomColorManager randomColorManager = finalBalls[finalIdx].GetComponent<RandomColorManager>();
		return randomColorManager.colorIndex;
	}

	private void disableButtonWhenGameOver(Button button)
	{
		button.interactable = false;
		TextMeshProUGUI tmpText = button.GetComponentInChildren<TextMeshProUGUI>();
		tmpText.text = "";
	}

	private bool checkGameOver()
	{
		if ((finalQueueCount >= 5) && (getColorIndexOfFinalBall(finalQueueCount-1) != getColorIndexOfFinalBall(finalQueueCount-2)))
		{
			disableButtonWhenGameOver(buttonNotWorkWhenGameOver1);
			disableButtonWhenGameOver(buttonNotWorkWhenGameOver2);
			disableButtonWhenGameOver(buttonNotWorkWhenGameOver3);
			disableButtonWhenGameOver(buttonNotWorkWhenGameOver4);
			disableButtonWhenGameOver(buttonNotWorkWhenGameOver5);
			disableButtonWhenGameOver(buttonNotWorkWhenGameOver6);
			gameOverText.gameObject.SetActive(true);
			gameState = GAME_STATE_OVER;
			return true;
		}
		return false;
	}

	public void OnLeftPush()
	{
		if ((gameState != GAME_STATE_PLAYING) || (leftStackCount >= 5))
		{
			return;
		}
		GameObject highestBall = getHighestGameBallsByX(-600);
		if (highestBall == null)
		{
			return;
		}
		moveBall(highestBall, -300, -240 + leftStackCount * 118);
		startQueueCount--;
		leftStackCount++;
	}

	public void OnLeftPop()
	{
		if ((gameState != GAME_STATE_PLAYING) || (leftStackCount == 0) || (finalQueueCount >= 6))
		{
			return;
		}
		GameObject highestBall = getHighestGameBallsByX(-300);
		if (highestBall == null)
		{
			return;
		}
		moveBall(highestBall, 600, -390 + finalQueueCount * 118);
		leftStackCount--;
		finalQueueCount++;
	}

	public void OnCenterPush()
	{
		if ((gameState != GAME_STATE_PLAYING) || (centerStackCount >= 5))
		{
			return;
		}
		GameObject highestBall = getHighestGameBallsByX(-600);
		if (highestBall == null)
		{
			return;
		}
		moveBall(highestBall, 0, -240 + centerStackCount * 118);
		startQueueCount--;
		centerStackCount++;
	}

	public void OnCenterPop()
	{
		if ((gameState != GAME_STATE_PLAYING) || (centerStackCount == 0) || (finalQueueCount >= 6))
		{
			return;
		}
		GameObject highestBall = getHighestGameBallsByX(0);
		if (highestBall == null)
		{
			return;
		}
		moveBall(highestBall, 600, -390 + finalQueueCount * 118);
		centerStackCount--;
		finalQueueCount++;
	}

	public void OnRightPush()
	{
		if ((gameState != GAME_STATE_PLAYING) || (rightStackCount >= 5))
		{
			return;
		}
		GameObject highestBall = getHighestGameBallsByX(-600);
		if (highestBall == null)
		{
			return;
		}
		moveBall(highestBall, 300, -240 + rightStackCount * 118);
		startQueueCount--;
		rightStackCount++;
	}

	public void OnRightPop()
	{
		if ((gameState != GAME_STATE_PLAYING) || (rightStackCount == 0) || (finalQueueCount >= 6))
		{
			return;
		}
		GameObject highestBall = getHighestGameBallsByX(300);
		if (highestBall == null)
		{
			return;
		}
		moveBall(highestBall, 600, -390 + finalQueueCount * 118);
		rightStackCount--;
		finalQueueCount++;
	}

	public void OnBack()
	{
		DataManager.Instance.saveData();
		SceneManager.LoadScene("MenuScene");
	}

}
