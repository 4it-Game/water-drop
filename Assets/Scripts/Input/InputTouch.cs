using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Class in charge to listen the touch or click, and send event to subscribers
/// </summary>
public class InputTouch : MonoBehaviorHelper
{
	/// <summary>
	/// Delegate to listen the touch or click, and send event to subscribers
	/// </summary>
	public delegate void TouchScreen();
	/// <summary>
	/// Event trigger when the player touch or click, send to all subscribers
	/// </summary>
	public static event TouchScreen OnTouchScreen;

	void Update () 
	{
		//Check if we are running either in the Unity editor or in a standalone build.
		#if (!UNITY_ANDROID && !UNITY_IOS && !UNITY_TVOS) || UNITY_EDITOR

		if(Input.GetMouseButtonDown(0))
		{
			if(OnTouchScreen != null)
				OnTouchScreen();
		}

		//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		if (Input.touchCount > 0){
			//Store the first touch detected.
			Touch myTouch = Input.touches[0];

			//Check if the phase of that touch equals Began
			if (myTouch.phase == TouchPhase.Began){
				if(OnTouchScreen != null)
					OnTouchScreen();
			}
		}

	
		#endif
	}


}