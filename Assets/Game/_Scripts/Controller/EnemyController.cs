using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{

	public GunController  thisGunController;
	void Start ()
	{
		thisGunController = GetComponent<GunController> ();

        InvokeRepeating("FireBullets", 1.0f, 2.0f);
	}

	void FireBullets ()
	{
		thisGunController.FireBullets ();
	}

    public void StopFiring()
    {
        CancelInvoke("FireBullets");
        this.enabled = false;
    }
}
