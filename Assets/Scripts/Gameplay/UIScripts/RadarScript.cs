using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Classe gérant le "radar" du démon
 * */
public class RadarScript : MonoBehaviour {
	public GameObject[] trackedObjects; //liste des objets à afficher dans le radar
	private List<GameObject> radarObjects; //liste des objets affichés
	private List<GameObject> borderObjects; //liste des objets en bordure du radar
	public GameObject radarPrefab;
	public float switchDistance;
	public Transform helpTransform; // objet pour déterminer la rotation entre le joueur et les objets trackés

	// Use this for initialization
	void Start () {
		createRadarObjects ();
	
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i< radarObjects.Count; i++) 
		{
			//On affiche les objets trop loin du joueur en bordure du radar
			if(Vector3.Distance(radarObjects[i].transform.position, transform.position) > switchDistance)
			{
				//on affiche les objets en bordure
				helpTransform.LookAt(radarObjects[i].transform);

				//On détermine la position correcte des objets en bordure du radar par rapport au joueur
				borderObjects[i].transform.position = transform.position + switchDistance * helpTransform.forward;

				borderObjects[i].layer = LayerMask.NameToLayer("RadarLayer");
				radarObjects[i].layer = LayerMask.NameToLayer("InvisibleLayer");
			
			}

			else
			{
				//on réaffiche les objets dans le rayon du radar
				radarObjects[i].layer = LayerMask.NameToLayer("RadarLayer");
				borderObjects[i].layer = LayerMask.NameToLayer("InvisibleLayer");
			}
		}
	
	}

	void createRadarObjects()
	{
		radarObjects = new List<GameObject> ();
		borderObjects = new List<GameObject> ();

		//On crée des points sur le radar pour chaque objet à tracker
		foreach (GameObject o in trackedObjects) 
		{
			GameObject obj = Instantiate(radarPrefab, o.transform.position,Quaternion.identity) as GameObject;
			obj.transform.parent = o.transform;
			obj.name = "rad_"+o.name;
			radarObjects.Add (obj);

			GameObject bObj = Instantiate(radarPrefab, o.transform.position,Quaternion.identity) as GameObject;
			bObj.transform.parent = o.transform;
			bObj.name = "rad_"+o.name;
			borderObjects.Add (bObj);
		}
	}

	/**
	 * Methode qui supprime un point du radar pour un objet donné
	 * */
	public void removeRadarObject(GameObject tracked)
	{
		//Debug.Log ("tracked = " + tracked.name);
		for (int i =0; i< radarObjects.Count; i++) 
		{
			//Debug.Log("radar name = "+radarObjects[i].name);
			if(radarObjects[i].name == "rad_"+tracked.name)
			{
				radarObjects[i].GetComponent<Renderer>().enabled = false;
				radarObjects.RemoveAt(i);
			}
		}

		for (int i =0; i< borderObjects.Count; i++) 
		{
			if(borderObjects[i].name == "rad_"+tracked.name)
			{
				borderObjects[i].GetComponent<Renderer>().enabled = false;
				borderObjects.RemoveAt(i);
			}
		}
	}
}
