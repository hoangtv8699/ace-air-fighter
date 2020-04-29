using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class  HealthController : MonoBehaviour
{

	public float HealthCount = 100;
	public float HealthDecrementCount = 100;
	public string CollisionNameCheck = "EnemyBullet";
	 
	public bool isPlayer = false;
	public bool isBigEnemy = false;
	public GameObject pickUp;
	public Image healthBar;
	Transform thisTransFrom;
	Rigidbody rigidBody;

    private Client client;

    void Start ()
	{
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
        if (!isPlayer)
        {
            CollisionNameCheck = client.getString("PlayerName") + "Bullet";
        }
       
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

		if (isPlayer)
        {
            //Ace_ingameUiControl.Static.startingHealth = HealthCount;
            //Ace_ingameUiControl.Static.UpdateHealthProgress (HealthCount);
            UpdateHealthProgress(HealthCount);
        }      
	}

    public void UpdateHealthProgress(float healthValue)
    {
        //float health = healthValue.Remap(0, HealthCount, 0, 100);
        healthBar.fillAmount = Mathf.RoundToInt(healthValue) * 0.01f;
        Debug.Log("health " + healthValue);
    }

    float lastParticleTime;

	void OnTriggerEnter (Collider incoming)
	{
        if (incoming.name.Contains (CollisionNameCheck)) {

			//SoundController.Static.playSoundFromName ("bulletImapact");
            // make particle
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

                if (this.name.Equals(client.getString("PlayerName")))
                {
                    //Debug.Log("player get shot: " + this.name);
                    client.PlayerGetShot();
                    //HealthCount -= HealthDecrementCount;
                    Camera.main.GetComponent<CameraShake>().enabled = true;
                    Invoke("CameraShakeReset", 0.5f);
                    UpdateHealthProgress(HealthCount);
                }
                

            } else {

                //Debug.Log("enemy get shot: " + this.name);
                if (HealthCount <= 20)
                {
                    client.Destroy(this.name);
                }
                else
                {
                    client.EnemyGetShot(this.name);
                }
                

				//if (isBigEnemy) {
				//	float collisionDistance = Vector3.Distance (thisTransFrom.position, incoming.transform.position);

				//	if (collisionDistance < 15) {
 
				//		HealthCount -= HealthDecrementCount * 2;
				//	}

				//}
                 
				//HealthCount -= HealthDecrementCount;
				////Ace_ingameUiControl.Static.AddScore (10);
				//if (healthBar != null) {
				//	healthBar.fillAmount = HealthCount * 0.01f;
				//}
			
			
			}
			CheckDeadOrAlive ();
        }
        //else if(incoming.tag.Contains("Enemy") && isPlayer)
        //{
        //    Debug.Log("Tigger enter: " + incoming.tag);
        //    HealthCount = 0;
        //    UpdateHealthProgress(HealthCount);
            
        //    Camera.main.GetComponent<CameraShake>().enabled = true;
        //    Invoke("CameraShakeReset", 0.5f);
        //    CheckDeadOrAlive();
        //}
        //else if (incoming.tag.Contains("Player") && this.tag.Contains("Enemy"))
        //{
        //    Debug.Log("Tigger enter: " + incoming.tag);
        //    HealthCount = 0;
        //    Ace_ingameUiControl.Static.UpdateHealthProgress(HealthCount);
            
        //    Camera.main.GetComponent<CameraShake>().enabled = true;
        //    Invoke("CameraShakeReset", 0.5f);
        //    CheckDeadOrAlive();
        //}



    }

    bool doOnce = false;

	public void CheckDeadOrAlive ()
	{

		if (HealthCount <= 0 && doOnce == false) {

			//Debug.Log (gameObject.name + " is dead");
			this.enabled = false;
			Destroy (gameObject, 15);

			if (this.tag.Contains ("Player")) {
				GameController.Static.OnPlayerDead ();
				SoundController.Static.playSoundFromName ("Blast");
				CameraShake shakeInstance = Camera.main.GetComponent<CameraShake> ();
				shakeInstance.enabled = true;
				shakeInstance.shake = 2f;

				gameObject.SetActive (false);
				UpdateHealthProgress (0);

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

                client.Destroy(this.name);

			}

			if (pickUp) {
				GameObject pick = GameObject.Instantiate (pickUp, thisTransFrom.position, Quaternion.identity) as GameObject;
                pick.name = pickUp.name + "|" + this.name;
                Debug.Log("create item: " + pick.name);
				Destroy (pick, 15);

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
