using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class HornScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	

	public void OnPointerDown(PointerEventData eventData){
		GameObject.Find ("HornSound").GetComponent<AudioSource> ().Play ();

	}
	
	public void OnPointerUp(PointerEventData eventData){
		
		GameObject.Find ("HornSound").GetComponent<AudioSource> ().Stop ();
		
		
	}
	

}