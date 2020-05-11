using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimeOut : MonoBehaviour {
    public bool isEnemy;

    private Client client;

    private void TimeOut()
    {
        if (isEnemy)
        {
            Debug.Log("destroy enemy:" + this.name);
            client.DestroyTimeOut(this.name);
        }
        else
        {
            Debug.Log("destroy item:" + this.name.Split('|')[1]);
            client.DestroyItemTimeOut(this.name.Split('|')[1]);
        }
        
        Destroy(this.gameObject);
    }
    // Use this for initialization
    void Start()
    {
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
        Invoke("TimeOut", 18);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
