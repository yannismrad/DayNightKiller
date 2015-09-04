using UnityEngine;
using System.Collections;

/**
 *Gestion de la luminosité ambiante
 * */
public class AmbienLightScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void incColor(int incr)
	{
		GetComponent<Light>().color += Color.white / 4 * Time.deltaTime;
	}

	public void incIntensity(float incr)
	{
		GetComponent<Light>().intensity += incr;
	}

	public void changeLightColor(int r, int g, int b)
	{
		GetComponent<Light>().color = new Color (r, g, b);
	}

	public void changeLightIntensity(float i)
	{
		GetComponent<Light>().intensity = i;
	}
}
