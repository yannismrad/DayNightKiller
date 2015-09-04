using UnityEngine;
using System.Collections;

/**
 * Script gérant le temps d'une partie
 * */
public class TimerScript : MonoBehaviour {
	public float nbMinutes; //temps de base (minutes)
	private float minutes = 10, seconds = 0, milliseconds = 0; //temps de survie

	private float guiTimerDelay = 120; //toutes les deux mins, changer l'affichage du background du timer
	private Texture2D[] guiTimerTextures;
	private int NB_TEX = 5;
	private int curTexIndex = 0;
	private bool runTimer;
	private float skyColor = 0.175f;

	void Start () {
		nbMinutes = minutes;
		guiTimerTextures = new Texture2D[NB_TEX];
		loadTextures ();
		RenderSettings.skybox.SetColor("_Tint", new Color(skyColor, skyColor, skyColor));
		runTimer = true;
		//Mise à jour de l'affichage du background du timer et de la luminosité ambiante
		InvokeRepeating ("updateGuiTimer", guiTimerDelay, guiTimerDelay);
	}

	void Update(){

		//Temps écoulé
		if (runTimer && minutes <= 0 && seconds <= 0 && milliseconds <= 0) 
		{
			GetComponent<GUIText>().text= "Dawn";
			runTimer = false;
			onTimerEnd();
		}

			if (milliseconds <= 0) 
			{
				if (seconds <= 0) 
				{
					minutes--;
					seconds = 59;
					
				}
				
				else if(seconds >= 1)
				{
					seconds--;
				}
				
				milliseconds = 100;
				
			}
			
			milliseconds -= Time.deltaTime * 100;
			
		
		if (runTimer) 
		{
			if (seconds < 10) 
			{
				GetComponent<GUIText>().text = string.Format ("{0}:0{1}", minutes, seconds);
			}
			
			else
				GetComponent<GUIText>().text = string.Format ("{0}:{1}", minutes, seconds);
		}
	}

	/**
	 * Methode declenchee a la fin du timer
	 * */
	void onTimerEnd()
	{
		GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().onTimeOver();
	}

	/**
	 * Méthode qui charge les textures du background du timer
	 * */
	void loadTextures()
	{
		for (int i=0; i<NB_TEX; i++) 
		{
			string texName = "Assets/Resources/GUI/moon_"+i+".png";
			Texture2D tex = (Texture2D)Resources.LoadAssetAtPath(texName, typeof(Texture2D));
			guiTimerTextures[i] = tex;
		}
	}

	/**
	 * Méthode qui change l'affichage de l'image de fond du timer
	 * */
	void updateGuiTimer()
	{
		//On charge la texture suivante dans le tableau
		if (curTexIndex <= guiTimerTextures.Length -1) 
		{
			GameObject.Find("TimerBackground").GetComponent<GUITexture>().texture = guiTimerTextures[curTexIndex];
			curTexIndex++;

			//Changer le ton de la lumière ambiante
			GameObject.Find ("AmbientLight").GetComponent<AmbienLightScript>().incColor(50);
			GameObject.Find ("AmbientLight").GetComponent<AmbienLightScript>().incIntensity(0.60f);
			//RenderSettings.ambientLight.in

			skyColor+=0.08f;
			//Changer la couleur de la skybox
			RenderSettings.skybox.SetColor("_Tint", new Color(skyColor,skyColor,skyColor));

		} 

		else return;
	}

	public float getCurrentMins()
	{
		return minutes;
	}

	public float getCurrentSecs()
	{
		return seconds;
	}

}
