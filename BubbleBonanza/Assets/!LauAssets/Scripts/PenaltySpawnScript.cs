using UnityEngine;
using System.Collections;

public class PenaltySpawnScript : MonoBehaviour {

	public ParticleSystem penaltySpawn;

	void Start ()
	{
		penaltySpawn = Resources.Load("ParticleSystems/TimePenaltySpawn", typeof(ParticleSystem)) as ParticleSystem;
	}

	void Enable () 
	{
		Instantiate(penaltySpawn, this.gameObject.transform.position, Quaternion.identity);
	}

}
