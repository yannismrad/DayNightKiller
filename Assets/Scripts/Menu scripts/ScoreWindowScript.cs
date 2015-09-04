using UnityEngine;
using System.Collections;

public class ScoreWindowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		//On cache le menu au debut
		showMenu (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/**
	 * Methode pour cacher le menu (le déplacer à un endroit non visible)
	 * */
	public void showMenu(bool hide)
	{
		//cacher le menu
		if(!hide)
		{
			GameObject.Find ("MenuBackground").GetComponent<GUITexture>().enabled = false;
			GameObject.Find ("Title").GetComponent<GUIText>().enabled = false;
			GameObject.Find ("VictimsText").GetComponent<GUIText>().enabled = false;
			GameObject.Find ("TimeElapsedText").GetComponent<GUIText>().enabled = false;
			GameObject.Find ("VictimsCount").GetComponent<GUIText>().enabled = false;
			GameObject.Find ("TimeElapsed").GetComponent<GUIText>().enabled = false;

			//Affichage du score
			/*GameObject.Find ("VictimsCount").guiText.text = 
				GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript>().getKillerScore()+""; */
			
			GameObject.Find ("QuitMenuButton").GetComponent<GUIText>().GetComponent<Collider>().enabled = false;
			GameObject.Find ("QuitMenuButton").GetComponent<GUIText>().enabled = false;
		}

		//l'afficher
		else
		{
			GameObject.Find ("MenuBackground").GetComponent<GUITexture>().enabled = true;
			GameObject.Find ("Title").GetComponent<GUIText>().enabled = true;
			GameObject.Find ("VictimsText").GetComponent<GUIText>().enabled = true;
			GameObject.Find ("TimeElapsedText").GetComponent<GUIText>().enabled = true;
			GameObject.Find ("VictimsCount").GetComponent<GUIText>().enabled = true;
			GameObject.Find ("TimeElapsed").GetComponent<GUIText>().enabled= true;
			
			GameObject.Find ("QuitMenuButton").GetComponent<GUIText>().GetComponent<Collider>().enabled = true;
			GameObject.Find ("QuitMenuButton").GetComponent<GUIText>().enabled = true;
		}
	}

}
