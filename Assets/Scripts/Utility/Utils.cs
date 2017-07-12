using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
using UnityEngine.SceneManagement;
#endif

public static class Utils{

	public static void ReloadScene()
	{
		#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		Application.LoadLevel (GetSceneName());
		#else
		SceneManager.LoadScene(GetSceneName());
		#endif
	}

	public static string GetSceneName()
	{
		#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		return Application.loadedLevelName;
		#else
		return SceneManager.GetActiveScene().name;
		#endif
	}
}
