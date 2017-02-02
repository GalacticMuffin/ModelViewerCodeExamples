using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HotSpotScript : MonoBehaviour 
{
//▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ User Defined Variables ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀

	public string hotspotName; 
	public GameObject hotspotLinkedTo;
//▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ Variables ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀
	// Use this for initialization
	protected Vector3 flip;
	public bool hotspotActive;
	public GameObject line;
	public LineRenderer rendererForLine;
	private Line lineScript;
	//public List<GameObject> hotspotElements = new List<GameObject>();
	public GameObject test; 

	public Transform textRedRayShooterPosition;
	public Transform destAncher;
	public Animator hotspotAnim; 
	public bool expandHotspot;
	public BoxCollider[] hotspotColliders;  

	//---------------------- START ----------------------
	void Start ()
	{
	/*	Material m = test.GetComponent<CanvasRenderer>().GetMaterial;
		Debug.Log("tell me where you keep your renderer unity!" + m.renderQueue);*/

		/*foreach (CanvasRenderer mat in GetComponentsInChildren<CanvasRenderer>())
		{
			Material m = mat.GetMaterial(); 
			Debug.Log("tell me where you keep your renderer unity!" + m.renderQueue);
		}*/
	}
	void Awake () 
	{
		hotspotColliders = GetComponentsInChildren<BoxCollider>();
		rendererForLine = line.GetComponent<LineRenderer>();
		flip = transform.GetChild(0).localScale;
		lineScript = line.GetComponent<Line>();
		ResetCorutines();
	}

//FLIP METHOD
	public void FlipFunction()
	{
		Vector3 hotspotPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);

		if (hotspotPos.x > ModelComponentViewerManager.screenPos.x)
		{
			transform.GetChild(0).localScale = new Vector3(flip.x * 1f,flip.y,flip.z); 
		}
		if (hotspotPos.x < ModelComponentViewerManager.screenPos.x)
		{
			transform.GetChild(0).localScale = new Vector3(flip.x * -1f,flip.y,flip.z);
		}
	}
	public void ResetCorutines()
	{
		rendererForLine.gameObject.SetActive(false); 
		StartCoroutine("RaycastCorutine");
		StartCoroutine("RubberbandCorutine"); 
	}
//RAYCAST METHOD
	private IEnumerator RaycastCorutine() 
	{

		while(true)
		{

			RaycastFunction();
			if(hotspotActive == true)
			{
				FlipFunction();  
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void RaycastFunction()
	{
		if(CustomTrackableEventHandler.tracking == false) 
		{
			return;
		}
		Ray ray = new Ray(this.transform.position, transform.forward * -1);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 1f)) 
		{
			if(hit.rigidbody)
			{
				UIInactive();
			}
		}
		else
		{
			UIEngage();
		}

		Ray TextRedRay = new Ray(textRedRayShooterPosition.position, transform.forward);

		if(Physics.Raycast(TextRedRay, out hit, 1f)) 
		{
			if(hit.rigidbody)
			{
				expandHotspot = true;
			}

		}
		else
		{
			expandHotspot = false;
		}
		
		
		Debug.DrawRay(ray.origin, transform.forward * -1, Color.white); 
		Debug.DrawRay(TextRedRay.origin, transform.forward, Color.red);   

	}
//UI INACTIVE OFF
	public void UIInactive()
	{
		/*if(line != null)
		{
			Destroy(x); 
		}*/
			
		foreach(BoxCollider colider in hotspotColliders)
		{
			colider.enabled = false; 
		}
			
			hotspotActive = false; 
			hotspotAnim.SetBool("Engage",false);
			rendererForLine.gameObject.SetActive(false); 

	}
//UI ENGAGE ON
	public void UIEngage()
	{
		if(Input.touchCount == 0)
		{
			hotspotAnim.SetBool("Engage",true);
		}

		hotspotActive = true; 
		foreach(BoxCollider colider in hotspotColliders)
		{
			colider.enabled = true;
		}
	}
	public void LineEngageMethod()
	{
		rendererForLine.gameObject.SetActive(true);
		lineScript.StartLineMethod(); 

	}
//Rubberbanding

	private IEnumerator RubberbandCorutine() 
	{
		while(true)
		{
			if(destAncher.transform.localPosition.x > 0)
			{
				if(expandHotspot == true )
				{
					//Debug.Log ("Expand!");
					destAncher.transform.localPosition = new Vector3(destAncher.transform.localPosition.x + 0.001f, destAncher.transform.localPosition.y, destAncher.transform.localPosition.z);
				}
				else if(VRControlls.rotation == true)
				{
					//Debug.Log ("Contract!");
					destAncher.transform.localPosition = new Vector3(destAncher.transform.localPosition.x - 0.001f, destAncher.transform.localPosition.y, destAncher.transform.localPosition.z);
					
				}
			}
			while(destAncher.transform.localPosition.x <= .1f)
			{
				//Debug.Log ("It has reached 0!");
				
				destAncher.transform.localPosition = new Vector3(destAncher.transform.localPosition.x + 0.003f, destAncher.transform.localPosition.y, destAncher.transform.localPosition.z);
			}
			if(VRControlls.rotation == true)
			{
				yield return new WaitForSeconds(0.1f);
			}
			else
			{
				yield return new WaitForSeconds(0.01f);
			}
		}
	}
	public void button()
	{
		if(hotspotLinkedTo != null)
		{
			ComponentHandler componentHandler = hotspotLinkedTo.AddMissingComponent<ComponentHandler>();
			Rigidbody modelRigidbody = hotspotLinkedTo.AddMissingComponent<Rigidbody>();
			modelRigidbody.useGravity = false;
			modelRigidbody.isKinematic = false;
			modelRigidbody.mass = 200;
			modelRigidbody.drag = 1;
			modelRigidbody.angularDrag = 0.7f; 
			ModelComponentViewerManager.vrControlls.Halt();
			componentHandler.IsolateModelComponent(hotspotLinkedTo.transform.position);
		}
		else
		{
			throw new System.ArgumentException("Hotspot must be linked to something!");
		}
	}
	
}
