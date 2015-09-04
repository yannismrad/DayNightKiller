using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Classe gérant l'IA des villageois
 * */
public class VillagerAIScript : MonoBehaviour {
	CharacterController controller;
	float changeDelay = 30; //délai entre deux changements de destination
	float idleTime = 8; //temps d'arret lorsque l'IA atteint sa destination
	private Transform destination;
	private NavMeshAgent agent;
	public NavMeshPath path;
	private List<GameObject> destinationNodes; //noeuds ou le villageois peut se rendre
	private bool hasReachedDestination, isWaiting;
	public bool isDead;
	Animation currentAnim;
	public GameObject killEffect;
	private float stuckTime = 4;
	private Vector3 lastPosition;
	private bool isStuck, canAttack, isAttacking;
	private float distanceToDemon, minDistance;
	List <AudioSource> deathSounds;

	// Use this for initialization
	void Start () {
		currentAnim = this.gameObject.GetComponent<Animation> ();
		currentAnim.Play ("idle");

		controller  = GetComponent<CharacterController>();
		agent = gameObject.GetComponent<NavMeshAgent>();
		initDestinationNodes ();
		destination = null;
		hasReachedDestination = true;
		isStuck = false;

		deathSounds = new List <AudioSource> ();

		AudioSource d1 = GetComponents<AudioSource> () [0];
		AudioSource d2 = GetComponents<AudioSource> () [1];
		AudioSource d3 = GetComponents<AudioSource> () [2];
		deathSounds.Add (d1);
		deathSounds.Add (d2);
		deathSounds.Add (d3);

		minDistance = 10.0f;

		//Dernière position du villageois (en cas de blocage pour le débloquer)
		lastPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		//Toutes les 30 secondes le villageois change de destination
		InvokeRepeating ("pickRandomDestination", idleTime/2, idleTime);
		//Vérification toutes les 4 secondes si le personnage est coincé
		InvokeRepeating ("checkCharacterStuck", stuckTime, stuckTime);

		//Verifier la position du démon
		InvokeRepeating ("checkDistanceDemon", 2, 2);
	}
	
	// Update is called once per frame
	void Update () {


		if (!isDead) 
		{
			if(canAttack)
			{
				Debug.Log ("I can attack");
				if(distanceToDemon <= minDistance)
				{
					Debug.Log (gameObject.name + "can attack demon");
					isAttacking = true;
				}

				else
				{
					//Si hors de portée et que le mode attaque était engagé, on l'annule
					if(isAttacking)
						isAttacking = false;
				}
			}

			//Si le villageois ne peut pas attaquer, il se déplace comme un villageois ordinaire (vers un point aléatoire)
			if(!isAttacking)
			{
				if (destination != null) 
				{
					if(hasReachedDestination)
					{
						isWaiting = false;
						// on attend puis on choisit une nouvelle destination
						//Debug.Log ("Waiting before moving !");
						currentAnim.Play ("idle");
						StartCoroutine(wait(idleTime));
						
						if(isWaiting)
							pickRandomDestination();
					}
					
					else
					{
						currentAnim.Play ("walk");
					}
					
					agent.SetDestination(destination.position);
					
				}
			}

			//Si on est en mode attaque, on joue l'animation d'attaque
			else
			{
				if (!currentAnim.IsPlaying("punch")) 
				{
					Debug.Log (gameObject.name + "is punching  demon");
					currentAnim.Play ("punch");
					gameObject.GetComponent<Animation>()["punch"].speed = 1.0f;

					//Se tourner vers le démon
					transform.LookAt (GameObject.Find("demon").transform.position);

					//Tirer un projectile
					foreach (Transform child in transform)
					{
						if (child.gameObject.name == "WeaponCube")
						{
							child.gameObject.GetComponent<MagicWeaponScript>().shoot ();
						}
						
					}
				}

			}
		}
	}

	/**
	 * Méthode pour recalculer la distance entre le villageois et le démon
	 * */
	public void checkDistanceDemon()
	{
		Debug.Log ("checking demon distance");
		distanceToDemon = Vector3.Distance (gameObject.transform.position, GameObject.Find ("demon").transform.position);
	}

	/**
	 * Méthode pour tuer le villageois
	 * */
	public void kill()
	{
		isDead = true;
		currentAnim.Play ("death");
		controller.enabled = false;
		BoxCollider col = GetComponent<BoxCollider> ();
		col.enabled = false;
		agent.Stop(); // on arrete le déplacement du villageois s'il bouge
		agent.enabled = false;

		//On signale au StateCheck qu'un villageois est mort
		GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().onVillagerKilled ();

		if (!GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().playAsVillager) 
		{
			//Enlever son point du radar (mode démon)
			GameObject.Find("demon").GetComponent<RadarScript>().removeRadarObject(gameObject);
		}

		playRandomSoundInList (deathSounds);


		//Particules à la mort du villageois
		GameObject particleSystem = (GameObject)Instantiate (killEffect);
		particleSystem.transform.position = this.transform.position;

		Destroy (particleSystem, 4);
	}

	/**
	 * Methode qui verifie si un transform donné correspond au transform destination
	 * */
	public void checkDestinationReached(Transform t)
	{
		//Si T = au node destination, alors c'est bon
		if (t == destination) 
		{
			hasReachedDestination = true;
			//Debug.Log ("DESTINATION REACHED");
		}
	}

	/**
	 * Méthode vérifiant si un personnage est coincé à un endroit
	 * */
	public void checkCharacterStuck()
	{
		Transform bipT = transform.Find ("Bip001").transform;
		//Si la dernière position du villageois au précédent check = sa position actuelle => il est bloqué
		if (transform.position == lastPosition) 
		{
			Debug.Log(gameObject.name+" is stuck !");
			isStuck = true;
			//agent.SetDestination(new Vector3(0,0,0));
			pickRandomDestination();
			//transform.rotation = Quaternion.Inverse(transform.rotation);

		}

		else
		{
			if(isStuck)
				isStuck = false;
			lastPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		}

	}

	/**
	 * Methode qui permet de faire un "wait" 
	 * */
	IEnumerator wait(float time)
	{
		yield return new WaitForSeconds(time);
		isWaiting = true;
	}

	/**
	 * Méthode qui initialise la liste des noeuds destination
	 * */
	void initDestinationNodes()
	{
		destinationNodes = new List<GameObject> ();
		GameObject nodeList = GameObject.Find ("AINodes");

		//Ajout des AINodes a la liste
		foreach (Transform child in nodeList.transform) 
		{
			destinationNodes.Add (child.gameObject);
		}

	}

	/**
	 * Methode pour choisir une destination aleatoire
	 * */
	void pickRandomDestination()
	{
		if (!isDead) 
		{
			//Debug.Log ("PickRandomDestination");
			bool chosen = false;
			//On choisit une nouvelle destination seulement si le villageois a atteint sa precedente destination
			//ou si le personnage est coincé
			if (hasReachedDestination || isStuck) 
			{
				while(!chosen)
				{
					//Debug.Log ("Picking destination...");
					int randIndex = Random.Range (0, destinationNodes.Count);
					
					//Si la destination choisie != de la destination actuelle alors OK
					if(destinationNodes[randIndex].transform != destination)
					{
						setDestination (destinationNodes[randIndex].transform);
						chosen = true;
						hasReachedDestination = false;
						//Debug.Log ("New destination picked : "+destination.position.x+","+destination.position.y);
						break;
					}
				}
			}
		}

	}

	/**
	 * Changer la vitesse du villageois
	 * */
	public void setSpeed(float speed)
	{
		if (agent != null)
			agent.speed = speed;
	}

	/**
	 * Méthode qui permet de jouer un son aléatoire dans une liste
	 * */
	public void playRandomSoundInList(List<AudioSource> list)
	{
		int randIndex = Random.Range (0, list.Count);
		list[randIndex].Play ();
	}

	
	public void setCanAttack(bool val)
	{
		canAttack = val;
	}

	public bool villagerDead()
	{
		return isDead;
	}

	void setDestination(Transform destination)
	{
		this.destination = destination;
	}

	public void destroyScript()
	{
		Destroy (this);
	}
}
