using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public delegate void GameStart ();
	/// <summary>
	/// Delegate stored function subscribe by script who want to know when the game is started
	/// </summary>
	public static event GameStart OnGameStarted;

	public delegate void GameEnd ();
	/// <summary>
	/// Delegate stored function subscribe by script who want to know when the game is ended
	/// </summary>
	public static event GameEnd OnGameEnded;

	/// <summary>
	/// Reference to the start UI Button
	/// </summary>
	public Button buttonStart;

	/// <summary>
	/// Particle to emit when the player starts
	/// </summary>
	public ParticleEmitter particleExplosionStart;

	/// <summary>
	/// The particle explosion death.
	/// </summary>
	public ParticleEmitter particleExplosionDeath;

	/// <summary>
	/// Particle to emit when the player touches the left wall
	/// </summary>
	public ParticleEmitter particleExplosionWallLeftPrefab;

	/// <summary>
	/// Particle to emit when the player touches the right wall
	/// </summary>
	public ParticleEmitter particleExplosionWallRightPrefab;

	private void Awake()
	{
		ActivateButtonStart ();
	}

	/// <summary>
	/// To activate button start
	/// </summary>
	void ActivateButtonStart()
	{

		buttonStart.onClick.RemoveAllListeners ();
		buttonStart.onClick.AddListener (OnStart);
	}

	/// <summary>
	/// Desactivate start button (to avoid double click) and start the game
	/// </summary>
	public void OnStart()
	{
		buttonStart.onClick.RemoveAllListeners ();
		buttonStart.gameObject.SetActive (false);
		#if !UNITY_TVOS
		if(OnGameStarted!=null)
			OnGameStarted ();
		#endif

		SpawnParticleStart();
	}

	/// <summary>
	/// Game Over function, who called the OnFinished event
	/// </summary>
	public void GameOver()
	{
		if (OnGameEnded != null)
			OnGameEnded ();
		

		//add pint to the ScoreManager

		// add point to the leaderboard manager

		Utils.ReloadScene();
	}

	/// <summary>
	/// Emit the particle at start
	/// </summary>
	public void SpawnParticleStart()
	{
		var pe = Instantiate (particleExplosionStart) as ParticleEmitter;
		pe.transform.position = new Vector3(0,-2.30f,0);
		pe.transform.rotation = Quaternion.identity;

	}

	/// <summary>
	/// Emit the particle when touching left wall
	/// </summary>
	public void SpawnParticleWallLeft(Vector3 v)
	{
		var pe = Instantiate (particleExplosionWallLeftPrefab) as ParticleEmitter;
		pe.transform.position = v;
		pe.transform.rotation = Quaternion.identity;

	}

	/// <summary>
	/// Emit the particle when touching right wall
	/// </summary>
	public void SpawnParticleWallRight(Vector3 v)
	{
		var pe = Instantiate (particleExplosionWallRightPrefab) as ParticleEmitter;
		pe.transform.position = v;
		pe.transform.rotation = Quaternion.identity;

	}

	public void SpawnParticleDeath(Vector3 v)
	{
		var pe = Instantiate (particleExplosionDeath) as ParticleEmitter;
		pe.transform.position = v;
		pe.transform.rotation = Quaternion.identity;

	}
}
