using UnityEngine;
using System.Collections;

/**
 * Script gérant les attaques "magiques" du villageois joueur
 * */
public class MagicWeaponScript : MonoBehaviour {
	public GameObject magicEffect;
	public GameObject magicProjectile;
	public GameObject splashEffect;
	AudioSource magicSound;
	

	// Use this for initialization
	void Start () {
		magicSound = GetComponents<AudioSource> () [0];
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void shoot()
	{
		GameObject projectile = Instantiate(magicProjectile, transform.position,Quaternion.identity) as GameObject;
		projectile.AddComponent<MagicBallScript> ();
		projectile.GetComponent<MagicBallScript> ().splashEffect = splashEffect;

		projectile.GetComponent<Rigidbody>().AddForce (transform.forward * 150, ForceMode.Impulse);
		magicSound.Play ();
		
		GameObject magicParticles = (GameObject)Instantiate (magicEffect, transform.position, Quaternion.identity);
		//magicParticles.transform.LookAt (transform.forward);
		magicParticles.transform.position = this.transform.position;
		magicParticles.transform.rotation = Quaternion.LookRotation(transform.forward);
		
		Destroy (magicParticles, 1);
		Destroy (projectile, 2);
	}

	void OnTriggerEnter(Collider col)
	{

	}

	void OnTriggerExit(Collider col)
	{
		
	}
}
