using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Classe de gestion du joueur villageois
 * */
public class VillagerScript : MonoBehaviour {
	Animation currentAnim;
	private float forwardSpeed = 6.0f, backwardSpeed = 3.0f;
	private bool isAttacking1; //attaque
	private float baseDamage = 5;
	private bool gamePaused;
	private float yPos;
	private int healthPoints = 100;
	private Vector3 moveDirection;
	float speed  = 5;
	private bool canAttack;
	public bool isDead;
	AudioSource footSteps;

	// Use this for initialization
	void Start () {
		GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().displayInfoMessage ("You are a villager");

		moveDirection = Vector3.zero;
		currentAnim = this.gameObject.GetComponent<Animation> ();
		GetComponent<Rigidbody>().inertiaTensorRotation = Quaternion.identity;
		isAttacking1 = false;
		gamePaused = false;
		GameObject.Find ("PauseText").GetComponent<GUIText>().enabled = false;
		yPos = transform.position.y;
		canAttack = false;
		isDead = false;
		gamePaused = false;
		footSteps = GetComponents<AudioSource> () [0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Si l'animation attack n'est plus jouée
		if(!currentAnim.isPlaying)
		{
			isAttacking1 = false;
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
			if(canAttack && Input.GetMouseButtonUp(0))
			{
				if (!currentAnim.IsPlaying("punch")) 
				{
					isAttacking1 = true;
					currentAnim.Play ("punch");
					gameObject.GetComponent<Animation>()["punch"].speed = 2.0f;

					//Tirer un projectile
					foreach (Transform child in transform)
					{
						if (child.gameObject.name == "WeaponCube")
						{
							child.gameObject.GetComponent<MagicWeaponScript>().shoot ();
						}
						
					}
				}

				
				//gameObject.rigidbody.constraints=RigidbodyConstraints.FreezePosition;
			}


			if (!isAttacking1) 
			{
				//idle
				if (!Input.anyKey) 
				{
					currentAnim.Play ("idle");
					gameObject.GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePosition;
				} 

				//mouvements
				else 
				{
					
					CharacterController controller  = GetComponent<CharacterController>();

					//Correction de hauteur
					if(yPos < transform.position.y)
					{
						transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
					}
					
					
					
					if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
					{
						currentAnim.Play ("walk");
						gameObject.GetComponent<Animation>()["walk"].speed = 1;
						
						moveDirection = transform.TransformDirection(0, 0, Input.GetAxis("Vertical"));
						moveDirection *= forwardSpeed;
						controller.Move(moveDirection * Time.deltaTime);

						if(!footSteps.isPlaying)
							footSteps.Play ();
					}  
					
					if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
					{
						currentAnim.Play ("walk");
						moveDirection = transform.TransformDirection(Vector3.back);
						moveDirection *= backwardSpeed;
						controller.Move(moveDirection * Time.deltaTime);
						if(!footSteps.isPlaying)
							footSteps.Play ();
					}
					
					
					//rotation gauche
					if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
					{
						currentAnim.Play ("walk");
						gameObject.transform.Rotate(Vector3.down * 4.0f);
					}
					
					//rotation droite
					if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) 
					{
						currentAnim.Play ("walk");
						gameObject.transform.Rotate(Vector3.up * 4.0f);
					}
					
					
				}

			}

		}
	
	}

	/**
	 * Méthode pour tuer le villageois joueur
	 * */
	public void kill()
	{
		isDead = true;
		currentAnim.Play ("death");
		BoxCollider col = GetComponent<BoxCollider> ();
		col.enabled = false;
		gamePaused = true;
		
		//On signale au StateCheck que le joueur est mort
		GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().endGame (false);

	}

	public void setCanAttack(bool val)
	{
		canAttack = val;
	}
}
