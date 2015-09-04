using UnityEngine;
using System.Collections;

/**
 * Classe permettant de vérifier si une collision a lieu pendant une attaque du démon
 * */
public class MeleeScript : MonoBehaviour {

	private AudioSource hitBody, hitFloor, hitWood; //sons pour les coups
	string woodBarriereName = "modelFence2";

	// Use this for initialization
	void Start () {
		hitBody = GetComponents<AudioSource> () [0];
		hitFloor = GetComponents<AudioSource> () [1];
		hitWood = GetComponents<AudioSource> () [2];
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/**
	 * On vérifie si on a touché un villageois en attaquant
	 * */
	void OnTriggerEnter(Collider col)
	{
		//partie survivant 
		if(GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().playAsVillager)
		{
			if (GameObject.Find ("demon").GetComponent<DemonAI> ().isAttacking()) 
			{
				Debug.Log("DEMON IS ATTACKING");
				if (col.gameObject.tag == "Villager") 
				{
					Debug.Log ("SPHERE COLLIDE");
					
					
					if(col.gameObject.name == "VillagerPlayer" && !col.gameObject.GetComponent<VillagerScript>().isDead)
					{
						//tuer le villageois (joueur)
						col.gameObject.GetComponent<VillagerScript>().kill ();
					}
					
					else if(col.gameObject.name != "VillagerPlayer" && !col.gameObject.GetComponent<VillagerAIScript>().isDead)
					{
						//tuer le villageois(IA)
						col.gameObject.GetComponent<VillagerAIScript>().kill ();
					}
					
					hitBody.Play();
					
				}
			}
		}


		else 
		{
			if (GameObject.Find ("demon").GetComponent<DemonScript> ().isAttacking ()) 
			{
				if (col.gameObject.tag == "Villager") 
				{
					//Debug.Log ("SPHERE COLLIDE");
					
					hitBody.Play();
					if(! col.gameObject.GetComponent<VillagerAIScript>().isDead)
					{
						//tuer le villageois
						col.gameObject.GetComponent<VillagerAIScript>().kill ();
					}
					
				}
			}
		}




	}
}
