using UnityEngine;
using System.Collections;

public class MessageDisplayScript : MonoBehaviour {

	private float curAlpha;
	private bool doFadeIn, stopFade;
	
	// Use this for initialization
	void Start () {
		curAlpha = 0;
		doFadeIn = true;
		stopFade = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!stopFade) 
		{
			//On fait un fade in du texte de départ
			if (doFadeIn) 
			{
				fadeIn();
				if(curAlpha >= 1)
					doFadeIn = false;
			}
			
			//Quand le texte est affiché entièrement on fait un fade out
			if (!doFadeIn) 
			{
				fadeOut ();
				
				//On arrete le fade out quand le texte a disparu
				if(curAlpha < 0)
				{
					stopFade = true;
					Destroy(this);
				}
					
			}
		}
	}
	
	void fadeIn()
	{  
		curAlpha += Time.deltaTime * 0.5f;
		float r= GetComponent<GUIText>().color.r;
		float g= GetComponent<GUIText>().color.g;
		float b= GetComponent<GUIText>().color.b;
		GetComponent<GUIText>().color = new Color(r,g,b,curAlpha);
	}
	
	void fadeOut()
	{  
		curAlpha -= Time.deltaTime * 0.5f;
		float r= GetComponent<GUIText>().color.r;
		float g= GetComponent<GUIText>().color.g;
		float b= GetComponent<GUIText>().color.b;
		GetComponent<GUIText>().color = new Color(r,g,b,curAlpha);
	}

	/**
	 * Méthode qui affiche le message donné en paramètre
	 * */
	public void displayMessage(string message)
	{
		GetComponent<GUIText>().text = message;
		curAlpha = 0;
		doFadeIn = true;
		stopFade = false;
	}
}
