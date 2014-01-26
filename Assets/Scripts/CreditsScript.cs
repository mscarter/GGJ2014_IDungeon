using UnityEngine;
using System.Collections;

public class CreditsScript : MonoBehaviour {
	public Camera mainCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 camPos = mainCamera.transform.position;
		if (camPos.y < -54) {
			Application.LoadLevel (0);	 		
		}
		mainCamera.transform.Translate (Vector3.down * Time.deltaTime);
	}
}
