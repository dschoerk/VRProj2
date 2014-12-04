using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Timers;


public class CreateInstruments : MonoBehaviour {

	public Transform InstrumentPrefab;

	private Dictionary<GameObject, bool> SpawnedInstruments = new Dictionary<GameObject, bool>();

	// Timer - repatly tries to spawn instruments
	private double SpawnTestInterval = 3000; // 3 Seconds
//	private Timer SpawnInstrumentsTimer = null;


	// Use this for initialization
	void Start () 
	{	
//		SpawnInstrumentsTimer = new Timer( SpawnTestInterval);
//		SpawnInstrumentsTimer.Elapsed += new ElapsedEventHandler(tryToSpawn);
//		SpawnInstrumentsTimer.Start (); // TODO: timer stop
	}

//	private void tryToSpawn(object sender, ElapsedEventArgs e)
//	{
//		lock (SpawnedInstruments) 
//		{
//			tryToSpawn ();
//		}
//	}


	private DateTime TryToSpawnTime = DateTime.MinValue;
	private readonly TimeSpan TryToSpawnTimeout = new TimeSpan (0, 0, 0, 3, 0);
	private void TryToSpawnInstruments()
	{
		// use timeout to avoid permanent toggle
		if (DateTime.Now > this.TryToSpawnTime) 
		{
			tryToSpawn();
			this.TryToSpawnTime = DateTime.Now + this.TryToSpawnTimeout;
		}
	}

	private void tryToSpawn()
	{
		// try to spawn Instruments that aren't already spawned
		// TODO: check unwanted disconnects and reset spawn-flags
		foreach (GameObject instrument in SpawnedInstruments.Keys) 
		{
			try
			{
				if (instrument != null && SpawnedInstruments[instrument] == false) 
				{
					// get the SpawnPrefab Script from GameObject-Instance and Spawn Object if possible
					SpawnPrefab spawnScript = instrument.GetComponent<SpawnPrefab> ();
					if (spawnScript != null) 
					{
						try 
						{
								spawnScript.SpawnNetworkObject ();
								SpawnedInstruments [instrument] = true;
						} 
						catch (Exception ex) 
						{
							// Log
							int i=0;
						}
					}
				}
			}
			catch(Exception ex)
			{
				int i=0;
			}
		}

	}

	/// <summary>
	/// Unity Callback
	/// OnGUI is called for rendering and handling GUI events.
	/// </summary>
	void OnGUI () 
	{
	
		if (Input.GetButton ("Guitar") && this.AddGuitar)  // register button G
		{
			// create Instrument-Instance out of Instrument-Prefab
			if (this.InstrumentPrefab != null)
			{
				Transform instrument = Instantiate(this.InstrumentPrefab) as Transform;
				instrument.parent = this.transform.parent;
				SpawnPrefab spawnScript = instrument.GetComponent<SpawnPrefab> ();
				if (spawnScript != null)
				{
					if (instrument.parent == null)
						spawnScript.PathInHierarchy = "/";
					else
						spawnScript.PathInHierarchy = instrument.parent.name; // TODO: verify
					spawnScript.playerPrefab = instrument; // TODO: verify
				}
				lock(SpawnedInstruments)
				{
					SpawnedInstruments.Add(instrument.gameObject, false);
				}
			}
		}

	}

	private DateTime AddGuitarTime = DateTime.MinValue;
	private readonly TimeSpan AddGuitarTimeout = new TimeSpan (0, 0, 0, 2, 0);
	private bool AddGuitar
	{
		get
		{
			// use timeout to avoid permanent toggle
			if (DateTime.Now > this.AddGuitarTime) 
			{
				this.AddGuitarTime = DateTime.Now + this.AddGuitarTimeout;
				return true;
			}
			return false;
		}
	}

	// Update is called once per frame
	void Update () 
	{	
		TryToSpawnInstruments ();
	}
}
