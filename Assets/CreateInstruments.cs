using UnityEngine;
using System.Collections;

public class CreateInstruments : MonoBehaviour {

	public Transform GuitarPrefab;

	// Use this for initialization
	void Start () 
	{

		// TODO: find prefab
	
	}

	/// <summary>
	/// Unity Callback
	/// OnGUI is called for rendering and handling GUI events.
	/// </summary>
	void OnGUI () 
	{
	
		if (Input.GetButton ("G"))  // TODO: register button G
		{
			// create Guitar-Instance out of Guitar-Prefab
			// Instantiate(
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
