using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class  HealthController : MonoBehaviour
{

	public float HealthCount = 100;
	public float HealthDecrementCount = 100;
	public string CollisionNameCheck = "PlayerBullet";
	 
	public bool isPlayer = false;
	public bool isBigEnemy = false;
	public GameObject pickUp;
	public Image healthBar;
	Transform thisTransFrom;
	Rigidbody rigidBody;

	void Start ()
	{
		//force setting minimum healthCoung and decrement
		if (HealthCount < 50)
			HealthCount = 0;
		if (HealthDecrementCount < 5)
			HealthDecrementCount = 5;

		thisTransFrom = transform;

		if (PlayerController.gunScript.currentGunType == GunType.Dual) {
			HealthDecrementCount = HealthDecrementCount * 0.45f;
		} else if (PlayerController.gunScript.currentGunType == GunType.triple) {
			HealthDecrementCount = HealthDecrementCount * 0.55f;
		} else if (PlayerController.gunScript.currentGunType == GunType.Quad) {
			HealthDecrementCount = HealthDecrementCount * 0.60f;
		}



		if (healthBar != null && HealthCount != 100) {

			Debug.Log (gameObject.name + " has health != 100");
		}

		lastParticleTime = Time.timeSinceLevelLoad;

		rigidBody = GetComponent<Rigidbody> ();

		if (isPlayer) { 
			Ace_ingameUiControl.Static.startingHealth = HealthCount;
			Ace_ingameUiControl.Static.UpdateHealthProgress (HealthCount);
		}      
	}

	float lastParticleTime;

	void OnTriggerEnter (Collider incoming)
	{

 

		if (incoming.name.Contains (CollisionNameCheck)) {

			//SoundController.Static.playSoundFromName ("bulletImapact");

			if (Time.timeSinceLevelLoad - lastParticleTime > 1) {

				GameObject particleObj;
 
				if (Vector3.Distance (thisTransFrom.position, incoming.transform.position) < 3) {
					particleObj = GameController.Static.ExplosionParticles [1];
				} else {
					particleObj = GameController.Static.ExplosionParticles [0];
				}

				GameObject Obj = GameObject.Instantiate (particleObj, incoming.transform.position, Quaternion.identity) as GameObject;
				Obj.name = gameObject.name + "_created this Particle";
				Obj.transform.parent = thisTransFrom;
				Destroy (Obj, 3);
				lastParticleTime = Time.timeSinceLevelLoad;
			}
			BulletController controller = incoming.GetComponent<BulletController> ();

			if (controller != null) {
				controller.currentBulletState = BulletStates.Reset;
			}
		
          
			
			if (isPlayer) {
				Ace_ingameUiControl.Static.UpdateHealthProgress (HealthCount);
				HealthCount -= HealthDecrementCount;
				Camera.main.GetComponent<CameraShake> ().enabled = true;
				Invoke ("CameraShakeReset", 0.5f);

			} else {

				if (isBigEnemy) {
					float collisionDistance = Vector3.Distance (thisTransFrom.position, incoming.transform.position);

					if (collisionDistance < 15) {
 
						HealthCount -= HealthDecrementCount * 2;
					}

				}
                 
				HealthCount -= HealthDecrementCount;
				Ace_ingameUiControl.Static.AddScore (10);
				if (healthBar != null) {
					healthBar.fillAmount = HealthCount * 0.01f;
				}
			
			
			}
			CheckDeadOrAlive ();
		}
		  
	}

	bool doOnce = false;

	void CheckDeadOrAlive ()
	{

		if (HealthCount <= 0 && doOnce == false) {

			//Debug.Log (gameObject.name + " is dead");
			this.enabled = false;
			Destroy (gameObject, 15);

			if (this.name.Contains ("Player")) {
				GameController.Static.OnPlayerDead ();
				SoundController.Static.playSoundFromName ("Blast");
				CameraShake shakeInstance = Camera.main.GetComponent<CameraShake> ();
				shakeInstance.enabled = true;
				shakeInstance.shake = 2f;

				gameObject.SetActive (false);
				Ace_ingameUiControl.Static.UpdateHealthProgress (0);

			} else {//for enemies
				if (rigidBody != null) {
					rigidBody.useGravity = true;
					rigidBody.constraints = RigidbodyConstraints.None;
					rigidBody.AddRelativeTorque (new Vector3 (UnityEngine.Random.Range (-1, 1), UnityEngine.Random.Range (-3, 5), UnityEngine.Random.Range (-3, 3) * 10) * 10, ForceMode.Acceleration);
					rigidBody.AddRelativeForce (new Vector3 (UnityEngine.Random.Range (-3, 3), -80, 150) * 5, ForceMode.Acceleration);
                   
				}
				if (isBigEnemy) {
					GameObject Obj = GameObject.Instantiate (GameController.Static.ExplosionParticles [2], transform.position, Quaternion.identity) as GameObject;
					Obj.transform.parent = thisTransFrom;
					Destroy (Obj, 3);
				}
				gameObject.SendMessage ("StopFiring", SendMessageOptions.DontRequireReceiver);
				SoundController.Static.playSoundFromName ("Blast");
				GameObject smokeObj = GameController.Static.SmokeParticles [Random.Range (0, 3)];
				smokeObj = GameObject.Instantiate (smokeObj, thisTransFrom.position, Quaternion.identity) as GameObject;
				smokeObj.transform.parent = thisTransFrom;

			}

			if (pickUp) {
				pickUp = GameObject.Instantiate (pickUp, thisTransFrom.position, Quaternion.identity) as GameObject;

				Destroy (pickUp, 15);

			}

			SoundController.Static.PlayDeadSound ();
			doOnce = true;
		}
	}

	void CameraShakeReset ()
	{
		Camera.main.GetComponent<CameraShake> ().enabled = false;
		Camera.main.GetComponent<CameraShake> ().shake = 0.2f;
	}
	 
}
