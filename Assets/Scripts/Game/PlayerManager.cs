using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviorHelper {

	/// <summary>
	/// true if Player can jump
	/// </summary>
	private bool canJump;

	/// <summary>
	/// true if game over
	/// </summary>
	private bool isGameOver;

	/// <summary>
	/// player Jumping forces acoding to the side 
	/// </summary>
	public float jumpForceX;
	public float jumpForceY;

	/// <summary>
	/// The rb2d refecrenc to the physics.
	/// </summary>
	private Rigidbody2D _rb2d;

	void Awake () {
		_rb2d = GetComponent<Rigidbody2D> ();
		_rb2d.isKinematic = true;
		transform.position = Vector2.zero;
		canJump = false;
	}

	/// <summary>
	/// Subscribe to OnTouchScreen from InputTouch
	/// </summary>
	void OnEnable () {
		InputTouch.OnTouchScreen += OnTouchScreen;

		GameManager.OnGameStarted += OnStarted;
	}

	void Start()
	{
		isGameOver = false;
	}

	/// <summary>
	/// Unsubscribe to OnTouchScreen from InputTouch
	/// </summary>
	void OnDisable()
	{
		InputTouch.OnTouchScreen -= OnTouchScreen;
		GameManager.OnGameStarted -= OnStarted;
		//decrease game on started and on finished
	}

	/// <summary>
	/// When game over, ridbody2d is kinematic so the player doesn't move anymore
	/// </summary>
	void OnFinished(){
		_rb2d.isKinematic = false;
	}

	void OnTouchScreen(){
		Jump(transform.position.x >= 0);
	}

	/// <summary>
	/// When started, the ridbody2d is not kinematic (to apply force to it) then we can continuously move up the player
	/// </summary>
	void OnStarted(){
		if(_rb2d.isKinematic){
			_rb2d.isKinematic = false;
		}

		_rb2d.velocity = new Vector2 (-jumpForceX, jumpForceY);

		StartCoroutine (OnStartDelay());
	}

	/// <summary>
	/// little delay to start the game. put samll delay befor start, let player to be ready
	/// </summary>
	IEnumerator OnStartDelay(){
		yield return new WaitForSeconds (0.3f);
		gameObject.SetActive (true);
		isGameOver = false;
		canJump = true;
		StartCoroutine (CoUpdate());
	}
		
	IEnumerator CoUpdate(){
		while (true) 
		{
			if (!canJump || isGameOver)
				break;

			PlayerMovement ();

			yield return 0;
		}
	}

	void PlayerMovement(){
		var v = _rb2d.velocity;
		v.y = jumpForceY;
		_rb2d.velocity = v;

		mainCameraManager.UpdatePos ();
	}

	/// <summary>
	/// Do a player jump, ie. a move on the X axis
	/// </summary>
	/// <param name="isLeft">If set to <c>true</c> is left.</param>
	void Jump(bool isLeft){
		if (!canJump || isGameOver)
			return;

		int direction = isLeft ? -1 : 1;

		_rb2d.velocity = new Vector2 (direction*jumpForceX , jumpForceY);
	}

	void OnCollisionEnter2D(Collision2D coll){
		OnCollision (coll.gameObject, coll);
	}

	/// <summary>
	/// Check who collide with player, if wall reduce health, if obstical Game over.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="coll">Coll.</param>
	void OnCollision(GameObject obj, Collision2D coll){
		if (isGameOver)
			return;

		_rb2d.velocity = new Vector2 (0, jumpForceY);

		if(coll.gameObject.tag == "LeftWall"){
			gameManager.SpawnParticleWallLeft (coll.contacts [0].point);
		}

		else if(coll.gameObject.tag == "RightWall"){
			gameManager.SpawnParticleWallRight (coll.contacts [0].point);
		}
		else if (coll.gameObject.tag == "Obstical") {
			//transform.position = coll.contacts [0].point;
			gameManager.SpawnParticleDeath(coll.contacts [0].point);
			LaunchGameOver ();
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Water") {
			Destroy (other.gameObject);
		}
	}

	/// <summary>
	/// the game over.
	/// </summary>
	void LaunchGameOver(){
		if (isGameOver)
			return;

		isGameOver = true;

		StartCoroutine (CoroutLaunchGameOver ());
	}

	/// <summary>
	/// Coroutine to turn rigidbody2d to kinematic = true (player can't move anymore), emit particles game over, and adds are available.
	/// </summary>
	IEnumerator CoroutLaunchGameOver(){
		_rb2d.velocity = Vector2.zero;

		_rb2d.isKinematic = true;

		GetComponent<Collider2D> ().enabled = false;

		//sounds

		//camera effects

		yield return new WaitForSeconds (1.5f);

		//continues popup
		/* 
			if (success) 
				{ 
					_rb2d.velocity = Vector2.zero;

					_rb2d.isKinematic = false;

					isGameOver = false;

					canJump = true;

					GetComponent<Collider2D> ().enabled = true;

					int direction = (transform.position.x >= 0) ? 1 : -1;


					_rb2d.velocity = new Vector2 (direction*jumpForceX, jumpForceY);

					StartCoroutine(CoUpdate());

				} 
				else 
				{ */
					gameManager.GameOver ();
					/*
				}
		*/


	}
}
