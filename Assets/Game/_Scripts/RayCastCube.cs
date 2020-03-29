using UnityEngine;
using System.Collections;

public class RayCastCube : MonoBehaviour {

	// Use this for initialization

	public Transform Shadow;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		RaycastHit hit;

		Vector3 down = transform.TransformDirection(Vector3.down);

		Debug.DrawRay(transform.position, down*10);


		if (Physics.Raycast(transform.position, down,out hit, 10)) {


			print("Object below i !" + hit.collider.name);

			Shadow.position = hit.point;
		}
	}
}
