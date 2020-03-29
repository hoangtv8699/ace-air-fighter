using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class getPathPoisition : MonoBehaviour {

	// Use this for initialization

	public List<Transform>  wayPoints = new List<Transform> () ;
	void Start () {
	
		Transform[] children =  gameObject.GetComponentsInChildren<Transform> ();

		foreach (Transform t in children) {

			if( t != gameObject.transform && t.name.Contains("wayPoint"))
			{
                wayPoints.Add(t);
				t.GetComponent<Renderer>().enabled = false;
			}

		}


		//Debug.Log ("Found children " + wayPoints.Count);
	}
	//bullet 
	void OnBecameInvisible()
	{
		Destroy (gameObject);
		Debug.Log ("started on object " + gameObject.name);
	}
}
