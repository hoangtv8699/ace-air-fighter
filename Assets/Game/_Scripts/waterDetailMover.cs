using UnityEngine;
using System.Collections;

public class waterDetailMover : MonoBehaviour {

	public Vector2  Speed;
	public Material waterMaterial;
	private Vector2 normalOffset;
	void Start () {
	
	}
	
	 
	void Update () {
	
		waterMaterial.SetTextureOffset ("_MainTex", normalOffset);
		waterMaterial.SetTextureOffset ("_Detail", normalOffset * 1.5f);

		normalOffset += Speed * Time.deltaTime;

	
	}
}
