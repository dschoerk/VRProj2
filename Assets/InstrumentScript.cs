using UnityEngine;
using System.Collections;

public class InstrumentScript : MonoBehaviour {

	public bool active = true;
	public AudioSource source;
	public float volume = 1.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (source == null)
			return;

		if(Network.isServer)
		{
			if(volume > 0)
				volume -= Time.deltaTime*0.05f;

			if (Input.GetKeyDown(KeyCode.Space))
				volume = 1;

		    source.volume = volume;
		}

		if (active && !source.isPlaying) 
			source.Play();
		else if(!active && source.isPlaying)
			source.Pause();
	}
}
