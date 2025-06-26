using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorManager : MonoBehaviour
{
	public static int colorSize = 6;

	static int spawnCounter = 0;

	public int colorIndex = 0;
	public int spawnIndex = 0;

	public void SetBallColor(Color colorToSet)
	{
		Renderer ballRenderer = GetComponent<Renderer>();
		ballRenderer.material.color = colorToSet;
	}
	
	// Start is called before the first frame update
	void Start()
    {
		spawnIndex = spawnCounter;
		spawnCounter = spawnCounter + 1;
		colorIndex = Random.Range(0, colorSize);
		switch (colorIndex)
		{
			case 0:
				SetBallColor(new Color(1f, 0, 0, 1f)); // red
				break;
			case 1:
				SetBallColor(new Color(0, 1f, 0, 1f)); // green
				break;
			case 2:
				SetBallColor(new Color(0, 0, 1f, 1f)); // blue
				break;
			case 3:
				SetBallColor(new Color(0, 0, 0, 1f)); // black
				break;
			case 4:
				SetBallColor(new Color(0.9f, 0.9f, 0.9f, 1f)); // white
				break;
			case 5:
				SetBallColor(new Color(1f, 0, 1f, 1f)); // light pink
				break;
			case 6:
				SetBallColor(new Color(0.9f, 0.9f, 0, 1f)); // yellow
				break;
			case 7:
				SetBallColor(new Color(0, 0.75f, 0.75f, 1f)); // light cyan
				break;
			case 8:
				SetBallColor(new Color(0.4f, 0.4f, 0.4f, 1f)); // gray
				break;
			case 9:
				SetBallColor(new Color(0.65f, 0.65f, 0, 1f)); // ?
				break;
		}

	}

    // Update is called once per frame
    void Update()
    {        
    }

}
