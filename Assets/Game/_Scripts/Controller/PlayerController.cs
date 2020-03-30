using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	public Transform playerTransform;
	public Vector3 Offset;
	public Vector3 targetPosition ;
	float bendValue ;
	bool canMove = false;
	public GunController gunInstance ;
	[Range(0.1f,0.3f)]
	public float
		planeMovementSpeed = 1.0f;
	public GameObject shiledRenderObject;
	public enum PlayerStates
	{

		start,
		PlaneAlive,
		death
	}
	 
	public PlayerStates currentState;

	public Transform PlayerMesh;
	  AnimationCurve PlayerRotationCurve = new AnimationCurve(new Keyframe(0, -4), new Keyframe(1, 4));

	 HealthController playerHealthScript;
	 public static GunController gunScript;
	void OnEnable ()
	{
	
		playerHealthScript = GetComponent<HealthController> ();
		gunScript = GetComponent<GunController> ();

	}
	 
	void Update ()
	{
		switch (currentState) {
		case PlayerStates.PlaneAlive:

			PlaneAlive ();
			gunInstance.FireBullets ();

			Camera.main.fieldOfView = playerTransform.position.z.Remap (-5,150,77,86);
			break;
		}

	}

	float lastGunpickUptime = 0;
	void OnTriggerEnter (Collider incoming)
	{
       // Debug.Log("player collided with "+ incoming.name);


        if (incoming.name.Contains("PickUp"))
        {
          //  Debug.Log("PARTICLE PICKUP IS CREATED");
               GameObject particleObj ;
             particleObj = GameController.Static.PlayerParticleEffects[0];
             GameObject Obj = GameObject.Instantiate(particleObj, playerTransform.position, Quaternion.identity) as GameObject;
             
             Destroy(Obj, 3);
        }


		if (incoming.name.Contains ("HealthPickup")) {

			Destroy (incoming.gameObject);
			SoundController.Static.playSoundFromName("Health_Pickup");

			playerHealthScript.HealthCount += 10 * (1 + PlayerPrefs.GetInt("Health",1));
			playerHealthScript.HealthCount = Mathf.Clamp (playerHealthScript.HealthCount, 0, 100);
			Ace_ingameUiControl.Static.UpdateHealthProgress (playerHealthScript.HealthCount);
			 
		} else if (incoming.name.Contains ("ShiledPickup")) {
			
			
			Destroy (incoming.gameObject);
			SoundController.Static.playSoundFromName("Health_Pickup");
			playerHealthScript.CollisionNameCheck = "NONE";
			CancelInvoke ("SwithOffShield");
			Invoke ("SwithOffShield", 20 * ( 1 + PlayerPrefs.GetInt("Shield",1) ) );
			shiledRenderObject.SetActive (true);
		} else if (incoming.name.Contains ("CoinPickUp")) {
		
			Destroy (incoming.gameObject);
			SoundController.Static.playSoundFromName("Pickup_Coin");
			Ace_ingameUiControl.Static.AddCurency(10);
           

		}
		else if (incoming.name.Contains ("GunPickup") && Time.timeSinceLevelLoad-lastGunpickUptime > 3.0f) {

			lastGunpickUptime = Time.timeSinceLevelLoad;
			incoming.gameObject.SetActive(false);
			Destroy (incoming.gameObject);
			SoundController.Static.playSoundFromName("Gun_Pickup");
			if( gunScript.currentGunType ==  GunType.single )
			{
				gunScript.currentGunType = GunType.Dual;
			}
			else if( gunScript.currentGunType ==  GunType.Dual )
			{
				gunScript.currentGunType = GunType.triple;
			}
			else if( gunScript.currentGunType ==  GunType.triple )
			{
				gunScript.currentGunType = GunType.Quad;
			}

		}

	}

	void SwithOffShield()
	{
		playerHealthScript.CollisionNameCheck ="EnemyBullet";
		shiledRenderObject.SetActive (false);
	}

	void PlaneAlive ()
	{
        targetPosition = playerTransform.position;

        if (Input.GetKey(KeyCode.W) && (playerTransform.position + Vector3.forward).z <= 50)
        {
            targetPosition += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S) && (playerTransform.position + Vector3.forward).z >= -10)
        {
            targetPosition -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D) && (playerTransform.position + Vector3.right).x <= 60)
        {
            targetPosition += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A) && (playerTransform.position + Vector3.right).x >= -60)
        {
            targetPosition -= Vector3.right;
        }
        

		playerTransform.position = Vector3.MoveTowards (playerTransform.position, targetPosition, Time.deltaTime * 50.1f);
		
 
		playerTransform.rotation = Quaternion.Euler (0, 0, PlayerRotationCurve.Evaluate( Mathf.PingPong(Time.time,1) ));

	}

	 
	 
}
