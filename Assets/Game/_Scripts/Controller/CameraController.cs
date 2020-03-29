using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	// Use this for initialization
	public enum CameraStates
	{

		move,
		soloMove}
	;

	public LayerMask maskedLayed;
	public static Vector3 touchPoint;
	 

	public CameraStates currentCamState;

	void Update ()
	{
	
		switch (currentCamState) {

		case CameraStates.move:
		
		 
			if (Input.GetKey (KeyCode.Mouse0)) {
				
				 
				DrawRayToWorld ();
			}



			break;
		}


	}

	void DrawRayToWorld ()
	{


		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, maskedLayed)) {
			Vector3 playerMoveposition = new Vector3 (hit.point.x, 0, hit.point.z + 10);	 
			touchPoint = playerMoveposition;
			
		 
		}
	}
}
