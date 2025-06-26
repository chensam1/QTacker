using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{

	private float MoveSpeed = 5;
	private float BackgroundHalfHeight = 2250.549f;

	// Start is called before the first frame update
	void Start()
	{
	}

    // Update is called once per frame
    void Update()
    {
		//Renderer renderer = gameObject.GetComponent<Renderer>(); // ***debug***
		//BackgroundHalfHeight = renderer.bounds.size.y / 2;
		//Debug.Log(BackgroundHalfHeight);
		transform.position = transform.position + Vector3.down * Time.deltaTime * MoveSpeed;
		if (transform.position.y < -1000)
		{
			transform.position = transform.position + new Vector3(0, BackgroundHalfHeight, 0);
		}
	}

}
