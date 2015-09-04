using UnityEngine;
using System.Collections;

public class ButtonsScript : MonoBehaviour {
	private AudioSource hoverSound, clickSound;

	// Use this for initialization
	void Start () {

		hoverSound = GetComponents<AudioSource> () [0];
		clickSound = GetComponents<AudioSource> () [1];

		//On cache le texte "choose a role" au debut
		hideChooseRole (true);

		//On deplace les boutons killer,survivor et back au debut
		hideKillerBtn (true);
		hideSurvivorBtn (true);
		hideBackBtn (true);


	}

	
	void OnMouseEnter()
	{
		hoverSound.Play ();
		gameObject.GetComponent<Renderer>().material.color = Color.red;
	}

	void OnMouseDown()
	{
		//Si on clique sur play : montrer les boutons de choix + bouton back, cacher le reste
		if (gameObject.name == "PlayButton") 
		{
			clickSound.Play ();
			hideKillerBtn (false);
			hideSurvivorBtn (false);
			hideBackBtn (false);
			hideChooseRole(false);

			hidePlayBtn(true);
			hideQuitBtn(true);
		}

		//Si on clique sur back : montrer les boutons play & quit , cacher le reste
		else if (gameObject.name == "BackButton") 
		{
			clickSound.Play ();
			hideKillerBtn (true);
			hideSurvivorBtn (true);
			hideBackBtn (true);
			hideChooseRole(true);
			
			hidePlayBtn(false);
			hideQuitBtn(false);
		}

		//Si on clique sur quit : quitter
		if (gameObject.name == "QuitButton") 
		{
			clickSound.Play ();
			Application.Quit();
		}

		//Jouer en tant que tueur
		if (gameObject.name == "KillerChoiceButton") 
		{
			clickSound.Play ();
			Application.LoadLevel("Level1");
		}

		//Jouer en tant que survivant
		if (gameObject.name == "SurvivorChoiceButton") 
		{
			clickSound.Play ();
			Application.LoadLevel("Level2");
		}
	}
	
	void OnMouseExit()
	{
		gameObject.GetComponent<Renderer>().material.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		
	}




	/**
	 * Methode pour cacher le bouton "play" (le deplacer hors du champ de vision)
	 * ou le remettre a sa position initiale
	 * */
	void hidePlayBtn(bool hide)
	{
			if(hide)
			{
				GameObject.Find ("PlayButton").GetComponent<Collider>().enabled = false;
				GameObject.Find ("PlayButton").GetComponent<Renderer>().enabled = false;
			}
			
			else
			{
				GameObject.Find ("PlayButton").GetComponent<Collider>().enabled = true;
				GameObject.Find ("PlayButton").GetComponent<Renderer>().enabled = true;
			}
	}

	/**
	 * Methode pour cacher le bouton "quit" (le deplacer hors du champ de vision)
	 * ou le remettre a sa position initiale
	 * */
	void hideQuitBtn(bool hide)
	{
			if(hide)
			{
				GameObject.Find ("QuitButton").GetComponent<Collider>().enabled = false;
				GameObject.Find ("QuitButton").GetComponent<Renderer>().enabled = false;
			}
			
			else
			{
				GameObject.Find ("QuitButton").GetComponent<Collider>().enabled = true;
				GameObject.Find ("QuitButton").GetComponent<Renderer>().enabled = true;
			}
	}

	/**
	 * Methode pour cacher le bouton "back" (le deplacer hors du champ de vision)
	 * ou le remettre a sa position initiale
	 * */
	void hideBackBtn(bool hide)
	{
			if(hide)
			{
				GameObject.Find ("BackButton").GetComponent<Collider>().enabled = false;
				GameObject.Find("BackButton").GetComponent<Renderer>().enabled = false;
			}
			
			else
			{
				GameObject.Find ("BackButton").GetComponent<Collider>().enabled = true;
				GameObject.Find ("BackButton").GetComponent<Renderer>().enabled = true;
			}

	}

	/**
	 * Methode pour cacher le bouton "killer" (le deplacer hors du champ de vision)
	 * ou le remettre a sa position initiale
	 * */
	void hideKillerBtn(bool hide)
	{
			if(hide)
			{
				GameObject.Find ("KillerChoiceButton").GetComponent<Collider>().enabled = false;
				GameObject.Find ("KillerChoiceButton").GetComponent<Renderer>().enabled = false;
			}

			else
			{
				GameObject.Find ("KillerChoiceButton").GetComponent<Collider>().enabled = true;
				GameObject.Find ("KillerChoiceButton").GetComponent<Renderer>().enabled = true;
			}
	}

	/**
	 * Methode pour cacher le bouton "survivor" (le deplacer hors du champ de vision)
	 * ou le remettre a sa position initiale
	 * */
	void hideSurvivorBtn(bool hide)
	{
			if(hide)
			{
				GameObject.Find ("SurvivorChoiceButton").gameObject.GetComponent<Collider>().enabled = false;
				GameObject.Find ("SurvivorChoiceButton").GetComponent<Renderer>().enabled = false;
			}
			
			else
			{
				GameObject.Find ("SurvivorChoiceButton").GetComponent<Collider>().enabled = true;
				GameObject.Find ("SurvivorChoiceButton").GetComponent<Renderer>().enabled = true;
			}
	}

	/**
	 * Methode pour masquer/demasquer le texte "choose a role"
	 * */
	void hideChooseRole(bool hide)
	{
		if (hide) 
		{
			GameObject.Find ("ChooseYourRoleText").GetComponent<Renderer>().enabled = false;
		} 
		else 
		{
			GameObject.Find ("ChooseYourRoleText").GetComponent<Renderer>().enabled = true;	
		}
	}

}
