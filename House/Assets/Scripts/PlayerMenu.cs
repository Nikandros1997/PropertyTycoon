using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 1

public class PlayerMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {

	public GameObject Menu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//turn on the menu for the player when there is movement on top of the object.
	public void OnPointerEnter(PointerEventData eventData) {
		//Debug.Log ("Enter()");
		Menu.SetActive (true);
	}

	// turn of the menu when pointer is not on the object.
	public void OnPointerExit(PointerEventData eventData) {
		//Debug.Log ("Exit()");
		Menu.SetActive (false);
	}
}
