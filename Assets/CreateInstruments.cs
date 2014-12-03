using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CreateInstruments : MonoBehaviour {

	public Transform GuitarPrefab;

	private List<Transform> Guitars = new List<Transform>();


	// Use this for initialization
	void Start () 
	{
	
	}

	/// <summary>
	/// Unity Callback
	/// OnGUI is called for rendering and handling GUI events.
	/// </summary>
	void OnGUI () 
	{
	
		if (Input.GetButton ("Guitar"))  // TODO: register button G
		{
			// create Guitar-Instance out of Guitar-Prefab
			Transform guitar = Instantiate(this.GuitarPrefab) as Transform; // TODO: verify
			this.Guitars.Add(guitar);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
