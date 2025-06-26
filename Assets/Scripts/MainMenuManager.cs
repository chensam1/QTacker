using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

	public TextMeshProUGUI soundText;
	public TextMeshProUGUI difficultText;

	// Start is called before the first frame update
	void Start()
    {
		updateMenuText();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnNewGame()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void OnSoundOnOff()
	{
		DataManager.Instance.soundOn = !(DataManager.Instance.soundOn);
		updateMenuText();
		if (DataManager.Instance.soundOn)
		{
			AudioListener.volume = 1f;
		}
		else
		{
			AudioListener.volume = 0f;
		}
	}

	public void OnDifficulty()
	{
		DataManager.Instance.gameLevel++;
		if (DataManager.Instance.gameLevel > 2)
		{
			DataManager.Instance.gameLevel = 0;
		}
		updateMenuText();
	}

	public void OnHelp()
	{
		SceneManager.LoadScene("HelpScene");
	}

	public void OnExit()
	{
#if (UNITY_EDITOR)
		UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
    	Application.Quit();
#endif
	}

	private void updateMenuText()
	{
		if (DataManager.Instance.soundOn)
		{
			soundText.text = "Sound On";
		}
		else
		{
			soundText.text = "Sound Off";
		}
		switch (DataManager.Instance.gameLevel)
		{
			case 0:
				difficultText.text = "Difficulty Easy";
				break;
			case 1:
				difficultText.text = "Difficulty Normal";
				break;
			case 2:
				difficultText.text = "Difficulty Hard";
				break;
		}
	}

}
