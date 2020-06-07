using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Loading : MonoBehaviour {
	
	void OnEnable () {
		Invoke ("loadLevel",5);
	}
	
	// Update is called once per frame
	void Update () {

	}


 	void loadLevel(){
		 Application.LoadLevelAsync("GamePlay");
	}
}
