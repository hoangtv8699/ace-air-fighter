using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class loadingText : MonoBehaviour {

	// Use this for initialization

	public Text loading;
	public string[] TextArray ;
	float index;
    public	float speed = 5;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		loading.text = TextArray[ Mathf.RoundToInt(index)];
		index+= speed*Time.deltaTime;
		if(index > TextArray.Length-1) index = 0;
	}
}
