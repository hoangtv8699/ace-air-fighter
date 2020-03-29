using UnityEngine;
using System.Collections;

public class pathTraveller : MonoBehaviour {

	// Use this for initialization

	getPathPoisition pathInstance ;
	void Start () {
	
		pathInstance = transform.parent.GetComponent<getPathPoisition> ();
	}
	
	// Update is called once per frame
	int currentWaypoint =1 ;
	public Vector3 targetPathPoint;
	public float movementSpeed = 1;
	void Update () {
	

		targetPathPoint = pathInstance.wayPoints [currentWaypoint].position;

		transform.position = Vector3.MoveTowards (transform.position, targetPathPoint, movementSpeed);
	    
		transform.rotation = Quaternion.Lerp(transform.rotation, pathInstance.wayPoints [currentWaypoint].rotation, Time.time * 1);
		if (Vector3.Distance (transform.position, targetPathPoint) < 1) {

			if( currentWaypoint < pathInstance.wayPoints.Count -1) currentWaypoint++;
		}


	}
}
