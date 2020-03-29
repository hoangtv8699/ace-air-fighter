using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour
{

	public float timeToDestroy = -1;
	void Start ()
	{
	
		if (timeToDestroy >= 0) {
			Destroy (gameObject, timeToDestroy);
		}
	}


	void OnBecameInvisible ()
	{
		Destroy (gameObject, 2);
	}
	
	 
}
