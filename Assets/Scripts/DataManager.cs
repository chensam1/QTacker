using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
class JsonBridge
{
	public int highScore;
}

public class DataManager : MonoBehaviour
{
	public static DataManager Instance { get; private set; }

	public int highScore;
	public bool soundOn;
	public int gameLevel;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		highScore = 0;
		soundOn = true;
		gameLevel = 0;
		strDataPath = Application.persistentDataPath + "/data.json";
		loadData();
		DontDestroyOnLoad(gameObject);
	}

	private string strDataPath;

	public void saveData()
	{
		if (Instance == null)
		{
			return;
		}
		JsonBridge jsonBridge = new JsonBridge();
		jsonBridge.highScore = Instance.highScore;
		string jsonString = JsonUtility.ToJson(jsonBridge);
		File.WriteAllText(strDataPath, jsonString);
		//Debug.Log("Save JSON: " + jsonString);
	}


	public void loadData()
	{
		if (Instance == null)
		{
			return;
		}
		if (!File.Exists(strDataPath))
		{
			return;
		}
		string jsonString = File.ReadAllText(strDataPath);
		//Debug.Log("Load JSON: " + jsonString);
		JsonBridge jsonBridge = JsonUtility.FromJson<JsonBridge>(jsonString);
		Instance.highScore = jsonBridge.highScore;
	}
}
