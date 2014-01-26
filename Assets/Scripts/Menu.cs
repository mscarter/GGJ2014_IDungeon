using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public Camera mainCamera;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			var clickRay = mainCamera.ScreenPointToRay(Input.mousePosition);

			RaycastHit hitInfo;
			Debug.Log(clickRay.ToString());
			if( Physics.Raycast(clickRay, out hitInfo, 1000f))
			{
				var objectHit = hitInfo.collider.gameObject.name;

				if( objectHit == "StartQuest" )
				{
					Application.LoadLevel(1);
				}

				if( objectHit == "Credits" ) {
					Application.LoadLevel(3);
				}
			}
		}
	}	
}
