using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
	//Blank class used for interaction between player damage and reciever

	public float health = 100;
        
	public bool instantiateAfterDeath = false;

	public GameObject objToInstantiate;

	void Update()
	{
		if (health < 0)
		{
			if(instantiateAfterDeath)
				Instantiate(objToInstantiate, transform.position, transform.rotation);

			Destroy(gameObject);
		}
	}
}

