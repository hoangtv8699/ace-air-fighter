using UnityEngine;
using System.Collections;

public enum BulletStates
{

	Reset,
	start,
	movement,
	Ready,
	End}
;

public class BulletController : MonoBehaviour
{

	public Rigidbody rigidBody;
	public Transform thisTransform;
	public Renderer Bulletrender;
	public   float Speed = 90;
	private float startingSpeed;
	public BulletStates currentBulletState;

	void OnEnable ()
	{
		startingSpeed = Speed;
	}

	public	void setColor (Color C)
	{
		Bulletrender.material.color = C;

	}

	void Update ()
	{

		switch (currentBulletState) {
		case BulletStates.movement:
			thisTransform.Translate ((thisTransform.forward) * Speed * Time.deltaTime, Space.World);
			Speed += 1.0f;
			break;

		case BulletStates.Reset:

			ResetBullet ();
			break;
		}
	}

	public void OnBecameInvisible ()
	{
		currentBulletState = BulletStates.Reset;
	}


	void ResetBullet ()
	{
		Speed = startingSpeed;
		thisTransform.position = new Vector3 (0, 0, -500);
		currentBulletState = BulletStates.Ready;
		gameObject.SetActive (false);
	}
}



