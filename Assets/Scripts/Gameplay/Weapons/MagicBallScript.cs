using UnityEngine;
using System.Collections;

public class MagicBallScript : MonoBehaviour {
	private int damage = 10;
	public GameObject splashEffect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		//Debug.Log("Bullet collision");

		splashEffect = (GameObject)Instantiate (splashEffect, transform.position, Quaternion.identity);
		splashEffect .transform.position = this.transform.position;
		splashEffect .transform.rotation = Quaternion.LookRotation(transform.forward);

		if (col.gameObject.name == "demon") 
		{
			//partie survivant
			if (GameObject.Find ("GameStateChecker").GetComponent<StateCheckScript> ().playAsVillager) 
			{
				col.gameObject.GetComponent<DemonAI>().hurt(damage);
			}

			else
				col.gameObject.GetComponent<DemonScript>().hurt(damage);
		}			

		Destroy (splashEffect, 1);
		Destroy (this.gameObject);
	}
	
	void OnCollisionExit(Collision col)
	{
		
	}
}
