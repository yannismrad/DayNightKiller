using UnityEngine;
using System.Collections;

public class LightMenuScript : MonoBehaviour {
	private float minIntensity = 0.80f; 
	private float maxIntensity = 1.2f;
	float random;
	


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float noise = Mathf.PerlinNoise(random, Time.time);
		GetComponent<Light>().intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

	}
}
