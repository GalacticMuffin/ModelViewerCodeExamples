using UnityEngine;
using System.Collections;

public class CircularSpawner : MonoBehaviour {

//▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ Variables ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀
	private int arraySize; 
	private int objectNumber = 0;

//Unity Editor tool variables and layout
	[Header("↻ Generate Circle With Multiple Different Prefabs ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━")]

	public GameObject[] PrefabsToUse;
	[Header("↻ Generate Circle With 1 Type of Prefab ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━")]

	public GameObject Prefab;
	public int NumberOfSpawns;
	[Header("☑ Circle Perameters ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━")]


	public Vector3 center;
	public Vector3 LocalObjectRotation = Vector3.zero; 
	public float radius;
	//private Quaternion rot;
	//private Vector3 pos;
	public bool EvenSpacing = true; 


//---------------------- START ----------------------
	void Start() 
	{
		arraySize = 0;
		if(PrefabsToUse.Length > 0 && NumberOfSpawns > 0 || PrefabsToUse.Length > 0 && Prefab) 
		{
			throw new System.ArgumentException("Edit only one method for generating a Circle (you may have prefabs or values in both Generate methods)"); 
		}
		else if(PrefabsToUse.Length > 0)
		{
			arraySize = PrefabsToUse.Length;
			NumberOfSpawns = 0;
		}
		else if(NumberOfSpawns > 0)
		{
			arraySize = NumberOfSpawns;
		}

		GameObject[] objects = new GameObject[arraySize]; 

		//call this anywhere you want to spawn a circle of objects;
		CreateCircleBegin(objects);

	}

	//CREATE CIRCLE BEGIN
	public void CreateCircleBegin(GameObject[] objects)
	{

		
		int i = 0;
		if(PrefabsToUse.Length > 0)
		{
			
			foreach (var x in PrefabsToUse)
			{
				objects[i] = x; 
				i++;
			}
		}
		else
		{
			for (int j = 0; j < NumberOfSpawns; j++)
			{
				
				objects[j] = (GameObject)Instantiate(Prefab);
				objects[j].transform.SetParent(this.transform); 
				Debug.Log(objects[j].name); 
			}
		}
		center = transform.position;
		foreach (var x in objects)
		{
			objectNumber ++;
			SpawnObjectsInCircle(x); 
		}
	}

//ANGLE CALCULATOR
	private float AngleCalculator()
	{
		float angle = (360f / arraySize) * objectNumber; 
		Debug.Log(angle);
		return angle; 
	}

//RANDOM CIRCLE
	private Vector3 RandomCircle(Vector3 center, float radius) 
	{ 
		// create random angle between 0 to 360 degrees 
		var ang = Random.value * 360; 
		Vector3 pos; 
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad); 
		pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad); 
		pos.z = center.z; 
		return pos; 
	}

//POINT GENERATOR
	private Vector2 CircularPointGenerator(float angle)
	{ 	
		Vector3 pos;
		// the Euler rotates the vector along the center of the "circle" by the degrese the 'angle' specifies
		pos = (Quaternion.Euler(0,0,angle) * Vector2.right) * radius; 
		Debug.Log(pos); 
		return pos;
	}

//RANDOM POINT GENERATOR
	private Vector3 RandomCircularPointGenerator()
	{
		var pos = (Vector3)(Random.insideUnitCircle.normalized * radius);   
		return pos;
	}

//SPAWN OBJECTS
	private void SpawnObjectsInCircle(GameObject x)
	{
		float angle = AngleCalculator();
		Vector3 spawnPosition; 
		if(EvenSpacing)
			spawnPosition = Quaternion.Euler(LocalObjectRotation) * CircularPointGenerator(angle);
		else
			spawnPosition = Quaternion.Euler(LocalObjectRotation) * RandomCircularPointGenerator();

			// make the object face the center
			//Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center);
			//GameObject cube = (GameObject)Instantiate(x, spawnPosition, Quaternion.Euler(0,0,0));
			x.transform.localPosition = spawnPosition; 
			x.transform.LookAt(center, Vector3.up);	  
	}
}