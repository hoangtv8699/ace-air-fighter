using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float RepeatTime = 2f;
    private Client client;

	public GunController  thisGunController;
	void Start ()
	{
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
        int level = client.getInt("LevelSelect");

        switch (level)
        {
            case 1:
                RepeatTime = 2f;
                break;
            case 2:
                RepeatTime = 1f;
                break;
            case 3:
                RepeatTime = 0.5f;
                break;
        }

        thisGunController = GetComponent<GunController> ();

        InvokeRepeating("FireBullets", 1.0f, RepeatTime);
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
