using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Classe de gestion du joueur demon
 * */
public class DemonScript : MonoBehaviour {
	AnimationClip idle, walk, attack1, attack2;
	Animation currentAnim;
	private float forwardSpeed = 6.0f, backwardSpeed = 3.0f;
	private bool isAttacking1, isAttacking2; //attaques 1 et 2
	private float baseDamage = 5;
	private bool gamePaused = false;
	private float yPos;
	private int healthPoints = 100;
	private Vector3 moveDirection = Vector3.zero;
	float speed  = 5;
	AudioSource walkLoop, armswing, death1, death2, pain1, pain2, pain3, att1, att2, att3;
	List <AudioSource> deathSounds, attackSounds, painSounds;
	bool playSound;



	// Use this for initialization
	void Start () {
		
		GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().displayInfoMessage ("You are the demon");
		currentAnim = this.gameObject.GetComponent<Animation> ();
		GetComponent<Rigidbody>().inertiaTensorRotation = Quaternion.identity;
		isAttacking1 = false;
		isAttacking2 = false;
		gamePaused = false;
		GameObject.Find ("PauseText").GetComponent<GUIText>().enabled = false;
		yPos = transform.position.y;

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
	
	// Update is called once per frame
	//Gestion des entrées clavier (mouvements,attaque, pause)
	void Update () {

		//Si l'animation attack n'est plus jouée
		if(!currentAnim.isPlaying)
		{
			isAttacking1 = false;
			isAttacking2 = false;
			if(playSound)
				playSound = false;
			//gameObject.rigidbody.constraints=RigidbodyConstraints.None;
		}

		//Mise en pause (echap)
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(!gamePaused)
			{
				Time.timeScale = 0;
				gamePaused = true;
				Debug.Log ("game paused");
				GameObject.Find ("PauseText").GetComponent<GUIText>().enabled = true;
			}

			else if(gamePaused)
			{
				GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().resumeGame();
				gamePaused = false;
				Debug.Log ("game unpaused");
				GameObject.Find ("PauseText").GetComponent<GUIText>().enabled = false;
			}

		}

		if (!gamePaused) {

			//Attaque 1
			if(Input.GetMouseButtonUp(0) && !isAttacking2)
			{
				isAttacking1 = true;
				currentAnim.Play ("AttackAnim");
				gameObject.GetComponent<Animation>()["AttackAnim"].speed = 2.0f;

				if(!playSound)
				{
					armswing.Play();
					playRandomSoundInList(attackSounds);
					playSound = true;
				}
					
				gameObject.GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePosition;
			}
			
			//Attaque 2
			if(Input.GetMouseButtonUp(1) && !isAttacking1)
			{
				isAttacking2 = true;
				//(on sépare l'animation d'attaque en 2 pour la gestion de collision des bras du démon
				currentAnim.PlayQueued("Attack2RaiseAnim", QueueMode.PlayNow);
				gameObject.GetComponent<Animation>()["Attack2RaiseAnim"].speed = 2.0f;

				currentAnim.PlayQueued("Attack2Anim", QueueMode.CompleteOthers);
				gameObject.GetComponent<Animation>()["Attack2Anim"].speed = 2.0f;

				if(!playSound)
				{
					playRandomSoundInList(attackSounds);
					playSound = true;
				}
				
				gameObject.GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePosition;
			}
			
			
			if (!isAttacking1 && !isAttacking2) 
			{
				//idle
				if (!Input.anyKey) 
				{
					currentAnim.Play ("IdleAnim");
					gameObject.GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePosition;
					if(walkLoop.isPlaying)
						walkLoop.Stop ();
				} 
				
				//mouvements
				else 
				{

					CharacterController controller  = GetComponent<CharacterController>();
				
					//gameObject.rigidbody.constraints=RigidbodyConstraints.None;

					if(yPos < transform.position.y)
					{
						transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
					}



					if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
					{
						currentAnim.Play ("WalkAnim");
						gameObject.GetComponent<Animation>()["WalkAnim"].speed = 1;

						moveDirection = transform.TransformDirection(0, 0, Input.GetAxis("Vertical"));
						moveDirection *= forwardSpeed;
						controller.Move(moveDirection * Time.deltaTime);
						/*if(!walkLoop.isPlaying)
							walkLoop.Play(); */
					}  
					
					if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
					{
						currentAnim.Play ("WalkAnim");
						//gameObject.transform.Translate(0,0,-(backwardSpeed * Time.deltaTime));
						moveDirection = transform.TransformDirection(Vector3.back);
						moveDirection *= backwardSpeed;
						controller.Move(moveDirection * Time.deltaTime);
					}
					
					
					//rotation gauche
					if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
					{
						currentAnim.Play ("WalkAnim");
						gameObject.transform.Rotate(Vector3.down * 3.0f);
					}
					
					//rotation droite
					if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) 
					{
						currentAnim.Play ("WalkAnim");
						gameObject.transform.Rotate(Vector3.up * 3.0f);
					}


				}
			}

		}

	}

	/**
	 * Méthode qui permet de jouer un son aléatoire dans une liste (attaques, dégats, mort...)
	 * */
	public void playRandomSoundInList(List<AudioSource> list)
	{
		int randIndex = Random.Range (0, list.Count);
		list[randIndex].Play ();
	}

	/**
	 * Methode qui reduit les points de vie du démon
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

		GameObject.Find ("HealthCounter").GetComponent<GUIText>().text = healthPoints + " HP";

		//Points de vie egaux à 0 : game over
		if (healthPoints == 0) 
		{
			playRandomSoundInList(deathSounds);
			currentAnim.PlayQueued("DeathAnim");
			Wait (10);
			GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().onPlayerKilled ();
		}
			

	}

	/**
	 * Méthode qui permet d'attendre la fin d'une animation
	 * */
	private IEnumerator Wait(float secs)
	{
		yield return new WaitForSeconds(secs);
	}

	
	public bool isAttacking()
	{
		if (isAttacking1 || (isAttacking2 && currentAnim.IsPlaying ("Attack2Anim") == true)) 
		{
			return true;
		}
		return false;			
	}

	public void pausePlayer()
	{
		GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().pauseGame();
		gamePaused = true;
	}
}
