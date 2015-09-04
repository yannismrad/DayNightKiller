using UnityEngine;
using System.Collections;

/**
 * Script de detection des villageois proches du démon IA
 * */
public class VillagerDetection : MonoBehaviour {
	bool enemyAlreadyInRange;
	Transform rangedEnemy;
	float distanceToEnemy;

	// Use this for initialization
	void Start () {
		enemyAlreadyInRange = false;
	}
	
	// Update is called once per frame
	void Update () {

		//On vérifie si l'ennemi détecté est toujours vivant, sinon on l'enlève
		if (rangedEnemy != null) 
		{
			if(rangedEnemy.gameObject.name=="VillagerPlayer")
			{
				if(rangedEnemy.gameObject.GetComponent<VillagerScript>().isDead)
				{
					Debug.Log ("ranged enemy player is dead");
					rangedEnemy = null;
				}
			}

			else 
			{
				if(rangedEnemy.gameObject.GetComponent<VillagerAIScript>() != null)
				{
					if(rangedEnemy.gameObject.GetComponent<VillagerAIScript>().isDead)
					{
						Debug.Log ("ranged enemy is dead");
						rangedEnemy = null;
					}
				}

			}


		}
	
	}

	/* Collision avec la sphère de detection : verrouiller l'ennemi qui est entré */
	void OnTriggerEnter(Collider col)
	{
		//On ne verrouille qu'un seul nouvel ennemi dans la portée de détection
		//si sa distance est inférieure à celui de l'ennemi actuel
		//Les autres sont ignorés jusqu'à ce que celui-ci sorte
		if (!enemyAlreadyInRange) 
		{
			if (col.gameObject.tag == "Villager")
			{
				Debug.Log ("New Enemy in range");
				float distToNewEnemy = Vector3.Distance (GameObject.Find ("demon").transform.position,
				                                             col.gameObject.transform.position);

				float distToCurrentEnemy = Vector3.Distance (GameObject.Find ("demon").transform.position,
				                                             GameObject.Find ("demon").GetComponent<DemonAI>().getTarget().position);

				if(distToNewEnemy < distToCurrentEnemy)
				{
					if(GameObject.Find ("demon").GetComponent<DemonAI>().setTarget(col.gameObject.transform))
						rangedEnemy = col.gameObject.transform;
				}


			}
		}

		
	}

	void OnTriggerStay(Collider col)
	{

	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.transform == rangedEnemy) 
		{
			GameObject.Find ("demon").GetComponent<DemonAI>().setCanAttack(false);
			enemyAlreadyInRange = false;
			rangedEnemy = null;
		}
						
	}
}
