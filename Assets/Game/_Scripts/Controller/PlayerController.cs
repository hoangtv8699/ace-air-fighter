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
    public bool isShieldOn;

    private Client client;
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
        isShieldOn = false;
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
        playerHealthScript = GetComponent<HealthController> ();
		gunScript = GetComponent<GunController> ();

	}
	 
	void Update ()
	{
		switch (currentState) {
		case PlayerStates.PlaneAlive:

			PlaneAlive ();

			Camera.main.fieldOfView = playerTransform.position.z.Remap (-5,150,77,86);
			break;
		}

	}

    public void IncreaseGun()
    {
        SoundController.Static.playSoundFromName("Gun_Pickup");
        if (gunScript.currentGunType == GunType.single)
        {
            gunScript.currentGunType = GunType.Dual;
        }
        else if (gunScript.currentGunType == GunType.Dual)
        {
            gunScript.currentGunType = GunType.triple;
        }
        else if (gunScript.currentGunType == GunType.triple)
        {
            gunScript.currentGunType = GunType.Quad;
        }
    }

	float lastGunpickUptime = 0;
	void OnTriggerEnter (Collider incoming)
	{
        // Debug.Log("player collided with "+ incoming.name);

        if (this.name.Equals(client.getString("PlayerName"))) {
            if (incoming.name.Contains("Pickup"))
            {
                //Debug.Log("pick up");
                //  Debug.Log("PARTICLE PICKUP IS CREATED");
                GameObject particleObj;
                particleObj = GameController.Static.PlayerParticleEffects[0];
                GameObject Obj = GameObject.Instantiate(particleObj, playerTransform.position, Quaternion.identity) as GameObject;

                Destroy(Obj, 3);
            }


            if (incoming.name.Contains("HealthPickup"))
            {
                //send pick up item
                string[] data = incoming.name.Split('|');
                client.Item("HEALTH", data[1]);
                //string[] data = incoming.name.Split('|');
                //client.Item(data[0], data[1]);
                //Destroy(incoming.gameObject);
                //SoundController.Static.playSoundFromName("Health_Pickup");

                //playerHealthScript.HealthCount += 10 * (1 + PlayerPrefs.GetInt("Health", 1));
                //playerHealthScript.HealthCount = Mathf.Clamp(playerHealthScript.HealthCount, 0, 100);
                //UpdateHealthProgress(playerHealthScript.HealthCount);

            }
            else if (incoming.name.Contains("ShiledPickup"))
            {

                //send pick up item
                string[] data = incoming.name.Split('|');
                client.Item("SHIELD", data[1]);
                //Destroy(incoming.gameObject);
                //SoundController.Static.playSoundFromName("Health_Pickup");
                //playerHealthScript.CollisionNameCheck = "NONE";
                //CancelInvoke("SwithOffShield");
                //Invoke("SwithOffShield", 20 * (1 + PlayerPrefs.GetInt("Shield", 1)));
                //shiledRenderObject.SetActive(true);
            }
            else if (incoming.name.Contains("CoinPickUp"))
            {

                //Destroy(incoming.gameObject);
                //SoundController.Static.playSoundFromName("Pickup_Coin");
                //Ace_ingameUiControl.Static.AddCurency(10);


            }
            else if (incoming.name.Contains("GunPickup") && Time.timeSinceLevelLoad - lastGunpickUptime > 3.0f)
            {
                //send pick up item
                string[] data = incoming.name.Split('|');
                client.Item("GUN", data[1]);
                //lastGunpickUptime = Time.timeSinceLevelLoad;
                //incoming.gameObject.SetActive(false);
                //Destroy(incoming.gameObject);
                //SoundController.Static.playSoundFromName("Gun_Pickup");
                //if (gunScript.currentGunType == GunType.single)
                //{
                //    gunScript.currentGunType = GunType.Dual;
                //}
                //else if (gunScript.currentGunType == GunType.Dual)
                //{
                //    gunScript.currentGunType = GunType.triple;
                //}
                //else if (gunScript.currentGunType == GunType.triple)
                //{
                //    gunScript.currentGunType = GunType.Quad;
                //}

            }
        }
    }

	void SwithOffShield()
	{
		playerHealthScript.CollisionNameCheck ="EnemyBullet";
		shiledRenderObject.SetActive (false);
        isShieldOn = false;
	}

    float currentFireTime;



    public void Fire()
    {
        if (Time.timeSinceLevelLoad - currentFireTime > 0.5)
        {
            client.Shot();
            currentFireTime = Time.timeSinceLevelLoad;

        }
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

        if (gameObject.name.Equals(client.getString("PlayerName")) && client.IsPlaying())
        {
            // send move
            client.Move(targetPosition);
            if (Input.GetKey(KeyCode.Space))
            {
                Fire();
            }
        }

        client.UpdateMove(gameObject);

        //gunInstance.FireBullets();

        playerTransform.rotation = Quaternion.Euler (0, 0, PlayerRotationCurve.Evaluate( Mathf.PingPong(Time.time,1) ));

	}	 
}
