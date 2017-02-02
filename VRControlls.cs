using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRControlls : MonoBehaviour {

//▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ Variables ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀
	public static bool rotation;

	private Vector3 currentRotation;
	private Vector3 lastRotation;
	public GameObject VRControlledObject; 
	public GameObject autoRotateObject; 
	public float spinSpeed; 
	public float counter;
	public static bool autoRotationEngaged;
	public bool autoRotation; 
	public float autoRotationCooldown;
	private Cooldown cooldown;
	private HotSpotScript hotspotScript;
	public Camera spacialUICamera; 
//---------------------- START ----------------------
	// Use this for initialization
	void Start () 
	{
		//hotspots = FindObjectsOfType(typeof(HotSpotScript)) as HotSpotScript[];
		VRControlledObject = this.gameObject;
		autoRotateObject = this.gameObject; 
		cooldown = VRControlledObject.AddComponent<Cooldown>();
		currentRotation = VRControlledObject.transform.eulerAngles;
	}

//~~~~~~~~~~~~~~~~~~~~~~ UPDATE ~~~~~~~~~~~~~~~~~~~~~ 
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(ModelComponentViewerManager.globalAutoRotation == true)
		{
			if(cooldown.engage == true)
			{
				AutoRotateFunction(); 
			}
			if(autoRotation == true && Input.touchCount <= 0)
			{
				AutoRotateFunction();
			}
			else
			{autoRotationEngaged = false;}
		}

		if(Input.touchCount > 0) 
		{
			/*foreach(HotSpotScript hotspot in hotspots)
			{
				hotspot.UIAlphaFunctionOff(); 
			}*/
			TouchFunction (Input.GetTouch(0).position);
			counter ++;
			autoRotation = false;

		}
		cooldown.CooldownCounterFunction(); 
		DetectRotationFunction();
	}

//TOUCH FUNCTION
	public void TouchFunction(Vector3 position)
	{
		if(Input.GetTouch(0).phase == TouchPhase.Moved) 
		{
			Touch touch = Input.GetTouch(0); 
			float xRotation = touch.deltaPosition.x;
			float yRotation = touch.deltaPosition.y; 

			VRControlledObject.GetComponent<Rigidbody>().AddTorque(new Vector3(yRotation * spinSpeed,xRotation * -spinSpeed,0));   
		}
		if(Input.GetTouch(0).phase == TouchPhase.Stationary && counter > 15.0f)
		{
			Halt();
			counter = 0;
		}
		cooldown.SetTimeStampFunction(autoRotationCooldown);

		if(Input.GetTouch(0).phase == TouchPhase.Began)
		{
			Ray ray = spacialUICamera.ScreenPointToRay (position); 

			RaycastHit[] hits;
			hits = Physics.RaycastAll (ray, Mathf.Infinity);
			for (int i = 0; i < hits.Length; i++) 
			{
				if (hits [i].transform.tag == "UIHitbox") 
				{
				//checks to see if we hit a bug
					//converts thing that is hit into a game object under the "MyBug" GameObject Variable
					GameObject hotspot = hits [i].transform.gameObject;
					//calls the DamageBugFunction in the bug script 
					hotspotScript = hotspot.GetComponentInParent<HotSpotScript>(); 
					hotspotScript.button(); 
				}
				/*else
				if (hits [i].transform.tag == "GASSTANK") {
					//converts thing that is hit into a game object under the "MyBug" GameObject Variable
					RectTransform MyGassTank = hits [i].transform.gameObject;
					//calls the DamageBugFunction in the bug script 
					MyGassTank.GetComponentInParent<GassTankManager> ().FillGass ();
				}*/
			}
		}
	}
//AUTO ROTATION FUNCTION

	public void AutoRotateFunction()
	{
		autoRotateObject.transform.Rotate(new Vector3(0,0.1f,0));
		autoRotationEngaged = true; 
	}

//DETECT ROTATION
	
	public void DetectRotationFunction()
	{
		currentRotation = transform.eulerAngles;
		if(currentRotation == lastRotation)
		{
			rotation = false;
		}
		else
		{
			rotation = true;
			
		}
		lastRotation = transform.eulerAngles;
	}

	public void Halt()
	{
		VRControlledObject.GetComponent<Rigidbody>().AddTorque(new Vector3(0,0,0));
		VRControlledObject.rigidbody.velocity = Vector3.zero;
		VRControlledObject.rigidbody.angularVelocity = Vector3.zero;
	}
}
