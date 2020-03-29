using UnityEngine;
using System.Collections;

public class parentNotiferInvisible : MonoBehaviour {




	void OnBecameInvisible()
	{
		transform.parent.GetComponent<BulletController> ().OnBecameInvisible ();
	}
}
