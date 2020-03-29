using UnityEngine;
using System.Collections;

public class ObjectCreatorOnScene : MonoBehaviour
{
	//public string OwerTag = "Player";
	// or Object position Point
	public string ObjectName;
	//ex coins,drums,obstacles
	public GameObject[] PrefabToCreate;
	public Vector3 OffsetPosition;
	// distance from desired position,forexample ,Leave it zero if you dont neeed

	[Range (0, 120)]
	public float
		startAtCreationTime = 0;
	[Range (0, 120)]
	public float
		TimeDifferenceBetweenEachCreation = 1;

	public bool[] RandomizeOffsetAxis = new bool[]{ false, false, false };

	//Randomize value
	[Range (0, 60)]
	public float
		RandomizeLastValueForX = 0;
	[Range (1, 60)]
	public int
		X_multipler = 1;

	[Range (0, 60)]
	public float
		RandomizeLastValueForY = 0;
	[Range (1, 60)]
	public int
		Y_multipler = 1;

	[Range (0, 60)]
	public float
		RandomizeLastValueForZ = 0;
	[Range (1, 160)]
	public int
		Z_multipler = 1;
 
	public Transform Owner;

	public void Start ()
	{
		//Owner = GameObject.FindGameObjectWithTag (OwerTag).GetComponent<Transform> ();
		if (Owner == null)
			Debug.LogError ("UNABLE TO FIND PLAYER FOR OBJECT CREATOR" + ObjectName);
		 
		StartCreatingObjects ();
	}

	public void StartCreatingObjects ()
	{
	
		InvokeRepeating ("CreateObject", startAtCreationTime, TimeDifferenceBetweenEachCreation);
	}

	void OnDisable ()
	{
		CancelInvoke ("CreateObject");
	}
	// Update is called once per frame
	public void CreateObject ()
	{

		GameObject createdObject = GameObject.Instantiate (PrefabToCreate [Random.Range (0, PrefabToCreate.Length)]) as GameObject;
		Vector3 TargetPoint = Owner.position + OffsetPosition + GetRandomizeValues ();

		createdObject.transform.position = TargetPoint;

	}

	Vector3 GetRandomizeValues ()
	{
		Vector3 rectVector = Vector3.zero;

		if (RandomizeOffsetAxis [0])
			rectVector.x = Random.Range (-RandomizeLastValueForX, RandomizeLastValueForX) * X_multipler;

		if (RandomizeOffsetAxis [1])
			rectVector.y = Random.Range (-RandomizeLastValueForY, RandomizeLastValueForY) * Y_multipler;

		if (RandomizeOffsetAxis [2])
			rectVector.z = Random.Range (-RandomizeLastValueForZ, RandomizeLastValueForZ) * Z_multipler;


		return rectVector;
	}
}
