using UnityEngine;
using System.Collections;

/**
 * Classe de gestion de l'état du jeu
 * */
public class StateCheckScript : MonoBehaviour {
	private int killerScore;
	private int nbSurvivorsAlive;
	public bool gameOver;
	GameObject nodeList, villagers;
	AudioSource unlifeMusic, lasthumanMusic, victoryMusic, huntMusic, loseMusic, startUpMusic ; //musiques
	public bool unlifeState, huntState, lastHumanState;
	public bool playAsVillager;
	public bool gamePaused;
	private float endFadeInTime = 5, endBgAlpha = 0;
	float timeElapsed, elapsedMins, elapsedSecs;

	// Use this for initialization
	void Start () {
		resumeGame();

		//Vérification de la partie jouée (survivant ou tueur)
		if (Application.loadedLevelName == "Level1") 
		{
			playAsVillager = false;
		}

		else if (Application.loadedLevelName == "Level2") 
		{
			playAsVillager = true;
		}

		killerScore = 0;
		unlifeMusic = GetComponents<AudioSource> () [0];
		lasthumanMusic = GetComponents<AudioSource> () [1];
		victoryMusic = GetComponents<AudioSource> () [2];
		huntMusic = GetComponents<AudioSource> () [3];
		loseMusic = GetComponents<AudioSource> () [4];
		startUpMusic = GetComponents<AudioSource> () [5];

		//On joue la musique de début de partie
		startUpMusic.Play ();

		//Etats
		unlifeState = false;
		lastHumanState = false;
		huntState = false;

		//Chargement des AINodes et des villageois
		nodeList = GameObject.Find ("AINodes");
		//Ajout de scripts à chaque AI Node
		foreach (Transform child in nodeList.transform) 
		{
			child.gameObject.AddComponent<AINodeScript>();
		}

		villagers = GameObject.Find ("Villagers");
		
		//Compter le nombre de villageois et régler leur vitesse 
		foreach (Transform villager in villagers.transform) 
		{
			nbSurvivorsAlive++;
			if(villager.gameObject.GetComponent<VillagerAIScript>() != null)
			{
				villager.gameObject.GetComponent<VillagerAIScript>().setSpeed(4.5f); //on règle leur vitesse
			}
		}

		//On cache l'overlay de défaite (on le montre seulement pour la défaite)
		GameObject.Find ("BloodBackground").GetComponent<GUITexture>().enabled = false;

		gamePaused = false;




	}
	
	// Update is called once per frame
	void Update () {

		timeElapsed += Time.deltaTime;
		elapsedMins = Mathf.Floor(timeElapsed / 60);
		elapsedSecs = (timeElapsed % 60);


		//Affichage du nombre de villageois encore en vie
		GameObject.Find ("VillagerCounter").GetComponent<GUIText>().text = nbSurvivorsAlive+"";

		//Si le timer vaut 3 minutes on joue la musique unlife 
		if (GameObject.Find ("TimerText").GetComponent<TimerScript> ().getCurrentMins () == 3
						&& GameObject.Find ("TimerText").GetComponent<TimerScript> ().getCurrentSecs () == 00) 
		{
			startUnlifeState();
		}
	}

	/**
	 * Passage en phase "unlife" (3 villageois encore en vie)
	 * */
	public void startUnlifeState()
	{
		if (!unlifeMusic.isPlaying && !lasthumanMusic.isPlaying && !huntMusic.isPlaying && !unlifeMusic.isPlaying && !lastHumanState && !huntState) 
		{
			unlifeMusic.Play ();
			unlifeState = true;
			
			foreach (Transform villager in villagers.transform) 
			{
				if(villager.gameObject.GetComponent<VillagerAIScript>() != null)
				{
					villager.gameObject.GetComponent<VillagerAIScript>().setSpeed(5.2f); //on augmente un peu la vitesse des derniers villageois
				}
			}
		}
	}

	/**
	 * Passage en phase "last human", dernier humain en vie
	 * */
	public void startLastHumanState()
	{
		if(unlifeMusic.isPlaying)
		{
			unlifeMusic.Stop();
		}
		
		if(huntMusic.isPlaying)
		{
			huntMusic.Stop();
		}

		if(playAsVillager)
			displayAlertMessage("You are the last human ! Run !");
		
		else
			displayAlertMessage("Kill the last human !");
		
		lasthumanMusic.Play ();
		lastHumanState = true;
	}
	
	/**
	 * Méthode appellée lorsqu'un villageois est tué
	 * */
	public void onVillagerKilled()
	{

		if (nbSurvivorsAlive > 0) 
		{
			nbSurvivorsAlive--;
			killerScore++;
		}

		//Message d'info (villageois tué)
		if (nbSurvivorsAlive > 1) 
		{
			displayInfoMessage ("A villager is dead !");
		}

		//3 humains en vie : jouer une musique lugubre
		if (nbSurvivorsAlive == 3)
		{
			startUnlifeState();
		} 

		//dernier humain : musique metal
		else if (nbSurvivorsAlive == 1) 
		{
			startLastHumanState();
		}
			
		//S'il ne reste plus de villageois victoire du démon
		else if (nbSurvivorsAlive == 0) 
		{
			if(!playAsVillager)
				endGame(true);

			else
				endGame (false);
		}
	}

	/**
	 * Methode déclenchée lorsque le compteur de temps est écoulé (aube)
	 * */
	public void onTimeOver()
	{
		displayInfoMessage ("The sun is rising");

		if (playAsVillager) 
		{
			displayAlertMessage ("Kill the demon !");
			GameObject.Find ("SurviveText").GetComponent<GUIText>().text = "Kill the demon";
		}

		else
			displayAlertMessage ("Villagers are looking for you !");



		setVillagersHuntState ();
	}


	/**
	 * Passage en phase hunt (villageois armés et chassent le demon)
	 * */
	public void setVillagersHuntState()
	{
		
		huntMusic.Play ();
		huntState = true;

		//On autorise le joueur à utiliser son attaque
		if (playAsVillager) 
		{
			GameObject.Find ("VillagerPlayer").GetComponent<VillagerScript>().setCanAttack(true);
		}

		//On autorise les IA à attaquer
		foreach (Transform villager in villagers.transform) 
		{
			if(villager.gameObject.name !="VillagerPlayer")
			{
				villager.gameObject.GetComponent<VillagerAIScript>().setCanAttack(true);
			}
		}


	}

	/**
	 * Methode appelée lorsque le joueur est tué
	 * */
	public void onPlayerKilled()
	{
		endGame (false);
	}

	/**
	 * Méthode appelée lorsque le démon IA meurt
	 * */
	public void onDemonAIKilled()
	{
		endGame(true);
	}

	/**
	 * Methode qui affiche l'ecran des scores et met fin à la partie
	 * */
	public void endGame(bool victory)
	{

		string title;

		pauseGame ();
		stopAllMusics ();
		
		
		if (!playAsVillager)
		{
			GameObject.Find ("demon").GetComponent<DemonScript>().pausePlayer();
		}
		
		//Titre du menu de fin de jeu et strings specifiques selon la victoire/défaite

		//Victoire
		if (victory) 
		{
			title = "Victory";
			victoryMusic.Play ();

			//En tant que villageois
			if(playAsVillager)
			{
				//Pas de texte "victims" pour le joueur villageois
				GameObject.Find ("VictimsText").GetComponent<GUIText>().text = "The demon is dead";
				GameObject.Find ("VictimsCount").GetComponent<GUIText>().text = "";
			}

			//En tant que démon
			else
			{
				//Affichage du score (victimes) du démon
				GameObject.Find ("VictimsText").GetComponent<GUIText>().text = "Victims";
				GameObject.Find ("VictimsCount").GetComponent<GUIText>().text = killerScore + "";
			}


		}

		//Défaite
		else 
		{
			GameObject.Find ("BloodBackground").GetComponent<GUITexture>().enabled = true;
			title = "Defeat";
			loseMusic.Play();
			gameOver = true;

			if(playAsVillager)
			{
				GameObject.Find ("VictimsText").GetComponent<GUIText>().text = "You are dead";
				GameObject.Find ("VictimsCount").GetComponent<GUIText>().text = "";
			}

			else
			{
				GameObject.Find ("VictimsText").GetComponent<GUIText>().text = "Victims";
				GameObject.Find ("VictimsCount").GetComponent<GUIText>().text = killerScore + "";
			}
		}


		GameObject.Find ("Title").GetComponent<GUIText>().text = title;

		//Affichage du temps écoulé
		float eMins = elapsedMins;

		GameObject.Find ("TimeElapsed").GetComponent<GUIText>().text = ""+elapsedMins.ToString("00") + ":" + elapsedSecs.ToString("00");

		GameObject.Find ("MenuBackground").GetComponent<ScoreWindowScript> ().showMenu (true);


	}

	/**
	 * Méthode pour arreter les musiques qui sont jouées
	 * */
	public void stopAllMusics()
	{
		if (lasthumanMusic.isPlaying)
			lasthumanMusic.Stop ();
		
		if (unlifeMusic.isPlaying)
			unlifeMusic.Stop ();

		if (huntMusic.isPlaying)
			huntMusic.Stop ();
	}

	/**
	 * Méthode pour mettre le jeu en pause
	 * */
	public void pauseGame()
	{
		Time.timeScale = 0;
		gamePaused = true;
	}

	/**
	 * Méthode pour reprendre la partie mise en pause
	 * */
	public void resumeGame()
	{
		Time.timeScale = 1;
		gamePaused = false;
	}

	/**
	 * Fonction qui va afficher un message d'info à l'écran
	 * */
	public void displayInfoMessage(string message)
	{
		GameObject.Find ("InfoMessage").AddComponent<MessageDisplayScript> ();
		GameObject.Find ("InfoMessage").GetComponent<MessageDisplayScript> ().displayMessage (message);
	}

	/**
	 * Fonction qui va afficher un message d'alerte (rouge) à l'écran
	 * */
	public void displayAlertMessage(string message)
	{
		GameObject.Find ("AlertMessage").AddComponent<MessageDisplayScript> ();
		GameObject.Find ("AlertMessage").GetComponent<MessageDisplayScript> ().displayMessage (message);
	}

	/**
	 * Methode qui retourne le score du tueur
	 * */
	public int getKillerScore()
	{
		return killerScore;
	}

}
