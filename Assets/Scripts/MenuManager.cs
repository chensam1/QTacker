using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameManager gameManager;
	public MainMenuManager mainMenuManager;
	public HelpSceneManager helpSceneManager;

	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnLeftPush()
	{
		gameManager.OnLeftPush();
	}

	public void OnLeftPop()
	{
		gameManager.OnLeftPop();
	}

	public void OnCenterPush()
	{
		gameManager.OnCenterPush();
	}

	public void OnCenterPop()
	{
		gameManager.OnCenterPop();
	}

	public void OnRightPush()
	{
		gameManager.OnRightPush();
	}

	public void OnRightPop()
	{
		gameManager.OnRightPop();
	}

	public void OnBack()
	{
		gameManager.OnBack();
	}

	public void OnNewGame()
	{
		mainMenuManager.OnNewGame();
	}

	public void OnSoundOnOff()
	{
		mainMenuManager.OnSoundOnOff();
	}

	public void OnDifficulty()
	{
		mainMenuManager.OnDifficulty();
	}

	public void OnHelp()
	{
		mainMenuManager.OnHelp();
	}

	public void OnExit()
	{
		mainMenuManager.OnExit();
	}

	public void OnBack4Help()
	{
		helpSceneManager.OnBack();
	}


}





