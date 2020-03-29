using UnityEngine;
using System.Collections;

public class ObjectRotate : MonoBehaviour {
	 

	public Vector3 speedVector ;
	public Space space;
	
	// Update is called once per frame
	void Update () {

		transform.Rotate ( speedVector * Time.deltaTime, space);


	}
	 
}
