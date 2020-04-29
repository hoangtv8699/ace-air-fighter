using UnityEngine;
using System.Collections;


public enum GunType
{
	single,
	Dual,
	triple,
	Quad
}

public class GunController : MonoBehaviour
{
	public bulletPoolManager BulletPool;
	public Transform[] singleGunlocation;
	public Transform[] DoubleGunlocation;
	public Transform[] TripleGunlocation;
	public Transform[] QuadrapleGunlocation;
	[Range (0.1f, 4.0f)] 
	public float
		timeBetweenEachFiring;
	bool isPlayer;
	public GunType currentGunType;
	public string poolObjectName = "_EnemyBulletPoolManager";
	Color bulletColor;

    private Client client;

    private void OnEnable()
    {
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
    }

    void Start ()
	{
        
        GameObject bulletPoolObject = GameObject.Find (poolObjectName);

		BulletPool = bulletPoolObject.GetComponent<bulletPoolManager> ();

		if (gameObject.tag.Contains ("Player")) {
			isPlayer = true;
		}


		bulletColor = GameController.Static.enemyBulletColors [Random.Range (0, GameController.Static.enemyBulletColors.Length)];
		SoundController.Static.firingBullets.Play ();
	}

	Transform[] currentGunLocations;
	// Update is called once per frame
	//float currentFireTime;



	public void FireBullets ()
	{
		ChangeBulletScriptSpeed ();


        //if (Time.timeSinceLevelLoad - currentFireTime > timeBetweenEachFiring) {
        foreach (Transform t in currentGunLocations)
        {

            GameObject CreatedBullet = BulletPool.GetBulletObject();
            if (CreatedBullet == null)
                return;
            CreatedBullet.transform.position = t.position;
            CreatedBullet.transform.rotation = t.rotation;

            if (isPlayer)
            {
                CreatedBullet.name = this.name + CreatedBullet.name;
            }
            BulletController bulletScript = CreatedBullet.GetComponent<BulletController>();
            bulletScript.currentBulletState = BulletStates.movement;
            bulletScript.Speed = bulletSpeed;
            bulletScript.setColor(bulletColor);


            //currentFireTime = Time.timeSinceLevelLoad;

        }
		//}
	}

	float bulletSpeed;

	void ChangeBulletScriptSpeed ()
	{
		if (isPlayer) {
			switch (currentGunType) {
				
			case GunType.single:
				currentGunLocations = singleGunlocation;
				bulletSpeed = GameController.Static.PlayerSingleGunBulletSpeed;
				break;
				
			case GunType.Dual:
				currentGunLocations = DoubleGunlocation;
				bulletSpeed = GameController.Static.PlayerDualGunBulletSpeed;
				break;
				
			case GunType.triple:
				currentGunLocations = TripleGunlocation;
				bulletSpeed = GameController.Static.PlayerTripleGunBulletSpeed;
				break;
				
			case GunType.Quad:
				currentGunLocations = QuadrapleGunlocation;
				bulletSpeed = GameController.Static.PlayerQuadGunBulletSpeed;
				break;
			}
		} else {
			switch (currentGunType) {
				
			case GunType.single:
				currentGunLocations = singleGunlocation;
				bulletSpeed = GameController.Static.SingleGunBulletSpeed;
				break;
				
			case GunType.Dual:
				currentGunLocations = DoubleGunlocation;
				bulletSpeed = GameController.Static.DualGunBulletSpeed;
				break;
				
			case GunType.triple:
				currentGunLocations = TripleGunlocation;
				bulletSpeed = GameController.Static.TripleGunBulletSpeed;
				break;
				
			case GunType.Quad:
				currentGunLocations = QuadrapleGunlocation;
				bulletSpeed = GameController.Static.QuadGunBulletSpeed;
				break;
			}
		}
		
		
		

	}
}
