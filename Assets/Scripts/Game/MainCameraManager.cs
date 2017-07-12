using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraManager : MonoBehaviour {

	/// <summary>
	/// Reference to the player
	/// </summary>
	public Transform player;
	/// <summary>
	/// Reference to the left wall
	/// </summary>
	public Transform left;
	/// <summary>
	/// Reference to the right wall
	/// </summary>
	public Transform right;
	/// <summary>
	/// if true = stop followgind player. false at game over
	/// </summary>
	public bool stopFollow = false;
	/// <summary>
	/// True by default. If true, the left and right walls will have always the same space between them
	/// </summary>
	public bool useContantWidth = true;
	/// <summary>
	/// If useContantWidth = true, the space between the left and right walls
	/// </summary>
	public float constantWidth = 7f;


	void OnEnable(){
		GameManager.OnGameStarted += OnStarted;

		GameManager.OnGameEnded += OnFinished;
	}
	void OnDisable(){
		GameManager.OnGameStarted -= OnStarted;

		GameManager.OnGameEnded -= OnFinished;
	}

	void Start (){
		stopFollow = false;

		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;

		float camHalfHeight = height/2f;
		float camHalfWidth = width/2f; 

		float size = Mathf.Min(camHalfHeight, camHalfWidth);

		if(useContantWidth)
			size = constantWidth;

		float decal = Mathf.Min(size*0.15f, size*0.15f);

		left.position = new Vector2 (-size + decal, 0);   

		right.position = new Vector2 (+size - decal, 0);  
	}

	private void OnStarted(){
		stopFollow = false;
	}

	private void OnFinished()
	{
		stopFollow = true;
	}

	/// <summary>
	/// To update the Y position of the camera, y position always  player Y position (if the game is not at Game Over state)
	/// </summary>
	public void UpdatePos()
	{

		if (stopFollow)
			return;

		Vector3 pos = transform.position;

		if (player == null)
			return;
		pos.y = player.transform.position.y;

		transform.position = pos;

	}
}
