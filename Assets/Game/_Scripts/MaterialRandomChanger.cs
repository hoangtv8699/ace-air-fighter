using UnityEngine;
using System.Collections;

public class MaterialRandomChanger : MonoBehaviour
{

	public Material[] materials;

	void Start ()
	{
	
		Renderer renderer = GetComponent<Renderer> ();
		if (renderer == null)
			return;

		Material mat = renderer.material;

		if (mat != null) {
			mat = materials [Random.Range (0, materials.Length)];
		}
	}
	
	 
}
