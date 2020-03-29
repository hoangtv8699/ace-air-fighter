using UnityEngine;
using System.Collections;

public class GroundMover : MonoBehaviour {

	public Vector3  startPosition;
	public Vector3 EndPosition;
	public Vector3 MoveSpeed;
	// Use this for initializatio
	  Transform thisTranfrom;
	void Start () {

		thisTranfrom = transform;

		 
	
	}

	void Update()
	{
		thisTranfrom.Translate (MoveSpeed * Time.deltaTime);
		checkGroundPosition ();
	}
	// Update is called once per frame
	void checkGroundPosition () {


	 
		if ( thisTranfrom.position.z <= EndPosition.z ) {

			thisTranfrom.position = startPosition;
		}
	
	}
}
