using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Timers;


public class CreateInstruments : MonoBehaviour {

	public Transform GuitarPrefab;
	public Transform DrumPrefab;
	public Transform PianoPrefab;

	private Dictionary<GameObject, bool> SpawnedInstruments = new Dictionary<GameObject, bool>();

	// Use this for initialization
	void Start () 
	{	
	}

	private DateTime TryToSpawnTime = DateTime.MinValue;
	private readonly TimeSpan TryToSpawnTimeout = new TimeSpan (0, 0, 0, 3, 0);
	private void TryToSpawnInstruments()
	{
		// use timeout to avoid permanent toggle
		if (DateTime.Now > this.TryToSpawnTime) 
		{
			lock(SpawnedInstruments)
			{
				tryToSpawn();
			}
			this.TryToSpawnTime = DateTime.Now + this.TryToSpawnTimeout;
		}
	}

	private void tryToSpawn()
	{
//		// TEST
//		return;

		// try to spawn Instruments that aren't already spawned
		// TODO: check unwanted disconnects and reset spawn-flags
		List<GameObject> SpawnedInstrumentsKeys = new List<GameObject> (SpawnedInstruments.Keys);
		foreach (GameObject instrument in SpawnedInstrumentsKeys)
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
							spawnScript.OnInstantiateObject ();
							SpawnedInstruments [instrument] = true;
						} 
						catch (Exception ex) 
						{
							Debug.Log("tryToSpawn:" + ex.ToString());
						}
					}
				}
			}
			catch(Exception ex)
			{
				Debug.Log("tryToSpawn:" + ex.ToString());
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
			if(Network.isClient)
				this.networkView.RPC("instantiateInstrumentRpc", RPCMode.Server, GuitarPrefab.name);
			else
				this.InstantiateInstrument(this.GuitarPrefab);
		}
		if (Input.GetButton ("Drum") && this.AddDrum)  // register button D
		{
			// create Instrument-Instance out of Instrument-Prefab
			if(Network.isClient)
				this.networkView.RPC("instantiateInstrumentRpc", RPCMode.Server, DrumPrefab.name);
			else
				this.InstantiateInstrument(this.DrumPrefab);
		}
		if (Input.GetButton ("Piano") && this.AddPiano)  // register button P
		{
			// create Instrument-Instance out of Instrument-Prefab
			if(Network.isClient)
				this.networkView.RPC("instantiateInstrumentRpc", RPCMode.Server, PianoPrefab.name);
			else
				this.InstantiateInstrument(this.PianoPrefab);
		}
	}

	[RPC]
	void instantiateInstrumentRpc(string prefabName)
	{
		if(prefabName == GuitarPrefab.name)
		{
			InstantiateInstrument(GuitarPrefab);
		}
		else if(prefabName == DrumPrefab.name)
		{
			InstantiateInstrument(DrumPrefab);
		}
		else if(prefabName == PianoPrefab.name)
		{
			InstantiateInstrument(PianoPrefab);
		}
	}

	private void InstantiateInstrument(Transform instrumentPrefab)
	{
		// create Instrument-Instance out of Instrument-Prefab
		if (instrumentPrefab != null)
		{
			// TEST
			//Transform instrument = Instantiate(instrumentPrefab) as Transform;
			Transform instrument = Network.Instantiate(instrumentPrefab, new Vector3(0,5,0) ,Quaternion.identity, 0) as Transform;

			return;

			//Transform instrument = Instantiate(instrumentPrefab) as Transform;
			instrument.parent = this.transform.parent;
			SpawnPrefab spawnScript = instrument.GetComponent<SpawnPrefab> ();
			if (spawnScript != null)
			{
				if (spawnScript.PathInHierarchy == string.Empty)
				{
					if (instrument.parent == null)
						spawnScript.PathInHierarchy = "/";
					else
						spawnScript.PathInHierarchy = instrument.parent.name; 
				}
				if (spawnScript.playerPrefab == null)
					spawnScript.playerPrefab = instrument; 
			}
			lock(SpawnedInstruments)
			{
				SpawnedInstruments.Add(instrument.gameObject, false);
				instrument.gameObject.name += "_" + Guid.NewGuid().ToString(); // to have a unique name
				// immediatly try to spawn
				tryToSpawn();
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

	private DateTime AddDrumTime = DateTime.MinValue;
	private readonly TimeSpan AddDrumTimeout = new TimeSpan (0, 0, 0, 2, 0);
	private bool AddDrum
	{
		get
		{
			// use timeout to avoid permanent toggle
			if (DateTime.Now > this.AddDrumTime) 
			{
				this.AddDrumTime = DateTime.Now + this.AddDrumTimeout;
				return true;
			}
			return false;
		}
	}

	private DateTime AddPianoTime = DateTime.MinValue;
	private readonly TimeSpan AddPianoTimeout = new TimeSpan (0, 0, 0, 2, 0);
	private bool AddPiano
	{
		get
		{
			// use timeout to avoid permanent toggle
			if (DateTime.Now > this.AddPianoTime) 
			{
				this.AddPianoTime = DateTime.Now + this.AddPianoTimeout;
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
