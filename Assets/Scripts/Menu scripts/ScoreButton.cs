using UnityEngine;
using System.Collections;

public class ScoreButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	
	void OnMouseEnter()
	{
		if (gameObject.name == "QuitMenuButton") 
		{
			Debug.Log (" entered");
			gameObject.GetComponent<GUIText>().color = Color.red;
		}
		
		
	}
	
	void OnMouseDown()
	{
		//Si on clique sur main menu on retourne au menu principal
		if (gameObject.name == "QuitMenuButton") 
		{
			Application.LoadLevel("menu test 2");
		}
		
	}
	
	void OnMouseExit()
	{
		if (gameObject.name == "QuitMenuButton") 
		{
			gameObject.GetComponent<GUIText>().color = Color.white;
		}
		
	}
}
