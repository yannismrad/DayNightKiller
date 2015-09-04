using UnityEngine;
using System.Collections;

/**
 * Script permettant de jouer des "beats" audio en fonction de la distance entre le joueur villageois et le démon
 * */
public class BeatScript : MonoBehaviour {

	private AudioSource beat1, beat2, beat3, beat4, currentBeat;
	private float distanceToDemon; //distance joueur-démon
	private float beatDist1 = 60, beatDist2 = 40, beatDist3 = 30, beatDist4 = 20; //distances déclenchant un beat
	GUIText fearText;
	bool playBeat;

	// Use this for initialization
	void Start () {

		beat1 = GetComponents<AudioSource> () [0];
		beat2 = GetComponents<AudioSource> () [1];
		beat3 = GetComponents<AudioSource> () [2];
		beat4 = GetComponents<AudioSource> () [3];
		fearText = GameObject.Find ("FearMeter").GetComponent<GUIText>();
		currentBeat = beat1;
		playBeat = true;
	
	}
	
	// Update is called once per frame
	void Update () {

		//On stoppe la musique si on est dans l'un des états des suivants
		if (GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().gameOver
				|| GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().huntState
				|| GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().lastHumanState
				|| GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().unlifeState) 
		{
			playBeat = false;
		}

		distanceToDemon = Vector3.Distance (transform.position, GameObject.Find ("demon").transform.position);

		if (distanceToDemon > beatDist1) 
		{
			fearText.text = "Nothing around";

		}

		if (distanceToDemon <= beatDist1 && distanceToDemon > beatDist2) 
		{
			Debug.Log ("DISTANCE 1");
			if(currentBeat != null)
			{
				//Un beat est déjà en cours de lecture, on attend qu'il s'arrete
				if(currentBeat.isPlaying)
				{
					currentBeat.loop = false;
				}

				//S'il est arreté on joue le beat1
				else
				{
					Debug.Log("PLAYING BEAT 1");
					currentBeat = beat1;
					currentBeat.loop = true;
					if(playBeat && !currentBeat.isPlaying)
						currentBeat.Play ();

					fearText.text = "He is roaming";
					fearText.color = new Color(100,100,100);
				}
			}
		}

		else if (distanceToDemon <= beatDist2 && distanceToDemon > beatDist3) 
		{

			if(currentBeat != null)
			{
				if(currentBeat.isPlaying)
				{
					currentBeat.loop = false;
				}

				else
				{
					Debug.Log("PLAYING BEAT 2");
					currentBeat = beat2;
					currentBeat.loop = true;

					if(playBeat && !currentBeat.isPlaying)
						currentBeat.Play ();

					fearText.text = "He is near";
					fearText.color = new Color(100,100,100);
				}
			}
		}

		else if (distanceToDemon <= beatDist3 && distanceToDemon > beatDist4) 
		{
			if(currentBeat != null)
			{
				if(currentBeat.isPlaying)
				{
					currentBeat.loop = false;
				}

				else
				{
					Debug.Log("PLAYING BEAT 3");
					currentBeat = beat3;
					currentBeat.loop = true;
					if(playBeat && !currentBeat.isPlaying)
						currentBeat.Play ();

					fearText.text = "He is close";
					fearText.color = new Color(255,60,10);
				}
			}
		}

		else if (distanceToDemon <= beatDist4) 
		{
			if(currentBeat != null)
			{
				if(currentBeat.isPlaying)
				{
					currentBeat.loop = false;
				}
				
				else
				{
					Debug.Log("PLAYING BEAT 4");
					currentBeat = beat4;
					currentBeat.loop = true;
					if(playBeat && !currentBeat.isPlaying)
						currentBeat.Play ();

					fearText.text = "Run !";
					fearText.color = new Color(255,0,0);
				}
			}
		}



	
	}

	/**
	 * Methode permettant de passer d'un beat à un autre
	 * */
	public void switchToBeat(AudioSource oldBeat, AudioSource newBeat)
	{

	}
}
