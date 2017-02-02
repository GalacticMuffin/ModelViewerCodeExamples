using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody))]
public class ModelComponentViewerManager : MonoBehaviour {
	public bool automaticModelRotation;
	public static bool globalAutoRotation;
	public static float n;
	public static Vector3 screenPos; 
	public static float dot;
	public Camera theCamera; 
	public List<GameObject> Hotspots = new List<GameObject>();
	public static GameObject componentOffsetPosition;  
	public static VRControlls vrControlls;
	public static string State; 
	public static GameObject hotspotContainer;
	public static HotSpotScript[] hotspotScripts;
	public static GameObject vrUI; 
	public static GameObject ModelContainer;
	public static GameObject worldSpaceContainer; 
	// Use this for initialization
	void Start () 
	{
		worldSpaceContainer = this.gameObject; 
		vrControlls = this.gameObject.GetComponent<VRControlls>();  
		componentOffsetPosition = GameObject.Find("ComponentOffsetPosition"); 
		vrUI = Camera.main.transform.GetChild(0).gameObject;
		vrUI.SetActive(false); 
		StartCoroutine(RandomCorutine());
		theCamera = Camera.main;
		globalAutoRotation = automaticModelRotation; 
		hotspotContainer = GameObject.Find("HotspotContainer"); 
		ModelContainer = transform.GetChild(0).gameObject; 

		GameObject[] hotspots = GameObject.FindGameObjectsWithTag("HotSpot");
		hotspotScripts = new HotSpotScript[hotspots.Length]; 
		for(int i = 0; i < hotspots.Length; i ++)
		{
			hotspotScripts[i] = hotspots[i].GetComponent<HotSpotScript>(); 
		}
		
		ModelContainer = transform.GetChild(0).gameObject; 
	}
	
	// Update is called once per frame
	void Update () 
	{
		dot = Vector3.Dot(transform.forward, theCamera.transform.forward); 
		screenPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
		//Debug.Log ("target is " + screenPos.x + " pixels from the left"); 
	}
	private IEnumerator RandomCorutine() 
	{
		while(true)
		{
			n = Random.Range(0.1f,0.5f);
			yield return new WaitForSeconds(0.1f);
		}
	}
}
