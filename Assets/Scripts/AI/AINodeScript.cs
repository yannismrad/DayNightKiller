using UnityEngine;
using System.Collections;

/**
 * Classe gérant les AINodes pour les villageois
 * */
public class AINodeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		Debug.Log ("Collision  enter!");

		//On vérifie si c'est un villageois qui est entré dans le node
		if (col.gameObject.tag == "Villager" && col.gameObject.name!="VillagerPlayer") 
		{
			if(col.gameObject.GetComponent<VillagerAIScript>() != null)
				col.gameObject.GetComponent<VillagerAIScript>().checkDestinationReached(transform);
		}
	}

	void OnTriggerExit(Collider col)
	{
		Debug.Log ("Collision exit!");
	}
}
