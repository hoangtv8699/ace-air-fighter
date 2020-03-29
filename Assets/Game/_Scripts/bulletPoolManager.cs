using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bulletPoolManager : MonoBehaviour {

	[Range(15,50)]
	public int NoOfBulletToBeCreated = 30;
	public List<GameObject> AllBullets;
	public GameObject bulletPrefab;

	  int SlectedIndex = 0;
	int currentBulletGenerattionCount = 0;
	public string NameToAppend;
	GameObject selectedBullet;




	public GameObject GetBulletObject () {
	
		//if there are no bullets in list array
		if (currentBulletGenerattionCount <= NoOfBulletToBeCreated) {
			selectedBullet = GameObject.Instantiate (bulletPrefab) as GameObject;
			AllBullets.Add (selectedBullet);
			currentBulletGenerattionCount++;
			selectedBullet.transform.parent = this.transform;
		} else {//after filling the array
			selectedBullet = AllBullets [SlectedIndex];
        
			SlectedIndex++;
			if (SlectedIndex > AllBullets.Count - 1)
				SlectedIndex = 0;
		}

		selectedBullet.name = NameToAppend;
        selectedBullet.SetActive(true);
		return selectedBullet;
	}

 
}
