using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class ComponentHandler : MonoBehaviour {

	private Button backButton; 
	private Vector3 origionalPosition; 
	public Vector3 origionalRotation; 

	private HotSpotScript[] hotspots;  
	// Use this for initialization
	void Awake () 
	{
		backButton = ModelComponentViewerManager.vrUI.transform.GetChild(0).gameObject.GetComponent<Button>();
		backButton.onClick.AddListener(MoveIntoPlace); 
		origionalRotation = transform.eulerAngles; 
	}

	public void IsolateModelComponent(Vector3 position)
	{
		origionalPosition = position; 
		ModelComponentViewerManager.vrControlls.enabled = false;
		this.gameObject.transform.SetParent(ModelComponentViewerManager.componentOffsetPosition.transform);  

		iTween.MoveTo (this.gameObject, iTween.Hash (
			"id","moveTween",
			//move to x
			"position", ModelComponentViewerManager.componentOffsetPosition.transform, 
			//duration from start to finish
			"time", 1,
			//"fadein and fadeout" of the speed
			"easetype", iTween.EaseType.easeInOutSine, 
			//deletes object. iTween uses the Destroy function to destroy objects after animation is completed.
			"oncomplete", (Action<object>)(x => {VRModeOn();}) 
			)); 
		foreach(HotSpotScript hotspotScript in ModelComponentViewerManager.hotspotScripts)
		{
			//hotspotScript.GetComponent<CanvasGroup>().alpha = 0f;
			hotspotScript.StopCoroutine("RaycastCorutine");
			hotspotScript.StopCoroutine("RubberbandCorutine");

			hotspotScript.UIInactive();
		}
	}
	public void VRModeOn()
	{
		transform.localPosition = new Vector3(0,0,0); 

		ModelComponentViewerManager.vrUI.SetActive(true); 
		ModelComponentViewerManager.vrControlls.VRControlledObject = this.gameObject;
		ModelComponentViewerManager.vrControlls.autoRotateObject = transform.parent.gameObject;  
		ModelComponentViewerManager.vrControlls.enabled = true;

	}

	public void MoveIntoPlace()
	{
		ModelComponentViewerManager.vrUI.SetActive(false); 
		ModelComponentViewerManager.vrControlls.enabled = false;
		this.gameObject.transform.SetParent(ModelComponentViewerManager.ModelContainer.transform);  

		iTween.RotateTo (this.gameObject, iTween.Hash (
			"rotation", origionalRotation,
			"islocal", true,
			//duration from start to finish
			"time", 1,
			//"fadein and fadeout" of the speed
			"easetype", iTween.EaseType.easeInOutSine 
			));

		iTween.MoveTo (this.gameObject, iTween.Hash (
			"id","moveTween",
			//move to x
			"position", origionalPosition,
			//duration from start to finish
			"time", 1,
			//"fadein and fadeout" of the speed
			"easetype", iTween.EaseType.easeInOutSine, 
			"oncomplete", (Action<object>)(x => {VRModeOff();}) 
			)); 
	}

	public void VRModeOff()
	{
		Destroy(rigidbody); 
		transform.eulerAngles = origionalRotation; 

		ModelComponentViewerManager.vrControlls.enabled = true;
		ModelComponentViewerManager.vrControlls.VRControlledObject = ModelComponentViewerManager.worldSpaceContainer;
		foreach(HotSpotScript hotspotScript in ModelComponentViewerManager.hotspotScripts)
		{
			hotspotScript.ResetCorutines();
		}
		Destroy (this); 

	}
	void OnDestroy() 
	{
		backButton.onClick.RemoveListener(MoveIntoPlace); 
	}
}
