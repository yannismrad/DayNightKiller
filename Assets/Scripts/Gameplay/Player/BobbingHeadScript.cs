using UnityEngine;
using System.Collections;

/**
 * Script permettant d'agiter la caméra lors des déplacements
 * Repris de ce script : http://wiki.unity3d.com/index.php?title=Headbobber
 * */
public class BobbingHeadScript : MonoBehaviour {

	private float timer = 0.0f; 
	public float bobbingSpeed = 0.18f; 
	public float bobbingAmount = 0.2f; 
	public float midpoint = 2.0f; 
	
	void Update () { 

		if (!GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().gamePaused) 
		{
			float waveslice = 0.0f; 
			float horizontal = Input.GetAxis("Horizontal"); 
			float vertical = Input.GetAxis("Vertical"); 
			if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) { 
				timer = 0.0f; 
			} 
			else { 
				waveslice = Mathf.Sin(timer); 
				timer = timer + bobbingSpeed; 
				if (timer > Mathf.PI * 2) { 
					timer = timer - (Mathf.PI * 2); 
				} 
			} 
			if (waveslice != 0) { 
				float translateChange = waveslice * bobbingAmount; 
				float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical); 
				totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f); 
				translateChange = totalAxes * translateChange; 
				
				Vector3 localPos = new Vector3(0, midpoint + translateChange, 0);
				transform.localPosition = localPos; 
			} 
			else { 
				Vector3 localPos = new Vector3(0, midpoint, 0);
				transform.localPosition = localPos; 
			} 
		}

	}
}
