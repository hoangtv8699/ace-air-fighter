using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyItem : MonoBehaviour {

    private Client client;

    private void OnDestroy()
    {
        Debug.Log("destroy item:" + this.name.Split('|')[1]);
        client.DestroyItemTimeOut(this.name.Split('|')[1]);
    }
    // Use this for initialization
    void Start () {
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
