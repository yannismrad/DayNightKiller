using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Classe gérant l'IA du démon
 * */
public class DemonAI : MonoBehaviour {
	
	//------------Variables----------------//
	
	public int moveSpeed;
	public int rotationSpeed;
	public int maxdistance;
	private List<GameObject> villagers; //liste des villageois à poursuivre
	public float villagerDistance;
	
	private Transform myTransform;
	private NavMeshAgent agent;
	
	float changeDelay = 2; //délai entre deux changements de destination
	float idleTime = 1; //temps d'arret lorsque l'IA atteint sa destination
	private Transform destination;
	public NavMeshPath path;
	private bool hasReachedTarget, isWaiting;
	public bool canAttack;
	public bool isDead;
	Animation currentAnim;
	public GameObject killEffect;
	private int healthPoints = 100;
	
	private bool isAttacking1; //attaques 1
	AudioSource walkLoop, armswing, death1, death2, pain1, pain2, pain3, att1, att2, att3;
	List <AudioSource> deathSounds, attackSounds, painSounds;
	bool playSound;

	private float stuckTime = 4;
	private Vector3 lastPosition;
	private bool isStuck;
	
	//------------------------------------//    
	
	void Awake()
	{
		myTransform = transform;
		
	}
	
	
	void Start ()
	{
		canAttack = false;
		maxdistance = 5;
		agent = GetComponent<NavMeshAgent>();
		
		initDestinationTargets ();
		destination = null;
		hasReachedTarget = true;
		
		currentAnim = this.gameObject.GetComponent<Animation> ();
		currentAnim.Play ("WalkAnim");
		
		InvokeRepeating ("pickRandomTarget", idleTime/2, idleTime);

		//Vérification toutes les 4 secondes si le personnage est coincé
		InvokeRepeating ("checkCharacterStuck", stuckTime, stuckTime);
		
		isAttacking1 = false;
		
		//Sons
		deathSounds = new List <AudioSource> ();
		attackSounds = new List <AudioSource> ();
		painSounds = new List <AudioSource> ();
		
		walkLoop = GetComponents<AudioSource> () [0];
		armswing = GetComponents<AudioSource> () [1];
		att1 = GetComponents<AudioSource> () [2];
		att2 = GetComponents<AudioSource> () [3];
		att3 = GetComponents<AudioSource> () [4];
		death1 = GetComponents<AudioSource> () [5];
		death2 = GetComponents<AudioSource> () [5];
		pain1 = GetComponents<AudioSource> () [6];
		pain2 = GetComponents<AudioSource> () [7];
		pain3 = GetComponents<AudioSource> () [8];
		
		deathSounds.Add (death1);
		deathSounds.Add (death2);
		attackSounds.Add (att1);
		attackSounds.Add (att2);
		attackSounds.Add (att3);
		painSounds.Add (pain1);
		painSounds.Add (pain2);
		painSounds.Add (pain3);
		playSound = false;
		
		
		
	}
	
	
	void Update ()
	{

		//currentAnim.Play ("WalkAnim");
		//Si l'animation attack n'est plus jouée
		if(currentAnim.IsPlaying("WalkAnim"))
		{
			isAttacking1 = false;
			if(playSound)
				playSound = false;
		}
		if (!isDead) 
		{
			if(destination != null)
			{
				villagerDistance = Vector3.Distance(destination.position, transform.position);
				//Si la cible est morte (et que ce n'est pas le joueur) on en choisit une autre
				if(destination.gameObject.name != "VillagerPlayer" && destination.gameObject.GetComponent<VillagerAIScript>().isDead)
				{
					hasReachedTarget = true;
					villagers.Remove (destination.gameObject);
					Debug.Log ("Villagers remaining : "+villagers.Count);
					pickRandomTarget();
					
				}
				
				//Sinon on la poursuite et on l'attaque si possible
				else
				{
					if (destination != null) 
					{
						
						//Cible atteinte/éliminée
						if(hasReachedTarget)
						{
							isWaiting = false;
							// on attend puis on choisit une nouvelle destination
							Debug.Log ("Demon Waiting before moving !");
							//currentAnim.Play ("IdleAnim");
							StartCoroutine(wait(idleTime));
							
							if(isWaiting)
								pickRandomTarget();
						}
						
						//Si la cible n'est pas encore atteinte/éliminée
						//Si l'ennemi est à portée on l'attaque
						if(villagerDistance <= 3.0f)
						{
							if(currentAnim.IsPlaying("WalkAnim"))
							{
								currentAnim.Stop ();
							}
							
							gameObject.GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePosition;
							agent.Stop();
							
							isAttacking1 = true;
							currentAnim.Play ("AttackAnim");
							gameObject.GetComponent<Animation>()["AttackAnim"].speed = 2.0f;
							
							if(!playSound)
							{
								armswing.Play();
								playRandomSoundInList(attackSounds);
								playSound = true;
							}
							
							//On attend 1 seconde après le coup avant de repartir
							StartCoroutine(wait(1));
							isAttacking1 = false;		
						}
						
						else
						{
							if(!currentAnim.IsPlaying("AttackAnim"))
								currentAnim.Play ("WalkAnim");
							agent.SetDestination(destination.position);
							agent.speed = moveSpeed;
						}
						
						
					}
				}
			}


		}
		
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Villager") 
		{
			canAttack = true;
		}
	}

	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag == "Villager") 
		{
			canAttack = false;
		}
	}
	
	/**
	 * Methode qui verifie si un transform donné correspond au transform destination
	 * */
	public void checkDestinationReached(Transform t)
	{
		//Si T = au node destination, alors c'est bon
		if (t == destination) 
		{
			hasReachedTarget = true;
			Debug.Log ("DEMON DESTINATION REACHED");
		}
	}

	/**
	 * Méthode vérifiant si un personnage est coincé à un endroit
	 * */
	public void checkCharacterStuck()
	{
		//Si la dernière position du démon au précédent check = sa position actuelle => il est bloqué
		if (transform.position == lastPosition) 
		{
			Debug.Log(gameObject.name+" is stuck !");
			isStuck = true;
			pickRandomTarget(); // on lui fait changer de destination
			
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
	 * Méthode qui initialise la liste des villageois cibles à suivre
	 * */
	void initDestinationTargets()
	{
		villagers = new List<GameObject> ();
		GameObject villagersList = GameObject.Find ("Villagers");
		
		//Ajout des villageois a la liste
		foreach (Transform child in villagersList.transform) 
		{
			villagers.Add (child.gameObject);
		}
		
	}
	
	/**
	 * Methode pour choisir une destination aleatoire
	 * */
	void pickRandomTarget()
	{
		if (!isDead) 
		{
			Debug.Log ("Demon PickRandomTarget");
			bool chosen = false;

			//On choisit un villageois au hasard
			//On ne le lache pas sauf si on croise un autre villageois

			if (hasReachedTarget) 
			{
				while(!chosen)
				{
					Debug.Log ("Demon Picking target...");
					int randIndex = Random.Range (0, villagers.Count);
					
					//Si la destination choisie != de la destination actuelle alors OK
					if(villagers[randIndex].transform != destination)
					{
						if(villagers[randIndex].GetComponent<VillagerAIScript>() != null || villagers[randIndex].GetComponent<VillagerScript>() != null )
						{
								setTarget (villagers[randIndex].transform);
								chosen = true;
								hasReachedTarget = false;
								Debug.Log ("Demon New target picked : "+destination.gameObject.name);
								break;
						}
					}
				}
			}
		}
		
	}
	
	public bool setTarget(Transform destination)
	{
		if (this.destination != destination) 
		{
			Debug.Log ("New target :"+destination.gameObject.name);
			this.destination = destination;
			return true;
		}

		return false;

	}

	public Transform getTarget()
	{
		return destination.transform;
	}

	/**
	 * Methode qui reduit les points de vie du démon IA
	 * */
	public void hurt(int dmg)
	{
		if (healthPoints > 0) 
		{
			healthPoints -= dmg;
			playRandomSoundInList(painSounds);
			Debug.Log ("health ="+healthPoints);
		}
		
		if (healthPoints < 0)
			healthPoints = 0;
		
		//Points de vie egaux à 0 : victoire des survivants
		if (healthPoints == 0) 
		{
			playRandomSoundInList(deathSounds);
			currentAnim.PlayQueued("DeathAnim");
			GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().endGame(true);
		}
		
		
	}

	public bool isAttacking()
	{
		if (isAttacking1 || currentAnim.IsPlaying("AttackAnim")) 
		{
			Debug.Log("JATTAK3");
			return true;
		}
		return false;			
	}

	public void setCanAttack(bool val)
	{
		canAttack = val;
	}
	
	
	/**
	 * Méthode qui permet de jouer un son aléatoire dans une liste (attaques, dégats, mort...)
	 * */
	public void playRandomSoundInList(List<AudioSource> list)
	{
		int randIndex = Random.Range (0, list.Count);
		list[randIndex].Play ();
	}
	
	
}
