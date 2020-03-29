using UnityEngine;
using System.Collections;

public class startExample : MonoBehaviour {

	public ObjectCreatorOnScene EnemyPlaneCreator;
	void Start () {
	
		EnemyPlaneCreator.StartCreatingObjects ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
