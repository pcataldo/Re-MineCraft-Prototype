using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Item") {
			/*
			 * if(=INVENTORY NOT FULL=)
			 * {
			 * =SEND TO INVENTORY=
			 * Destory (other);
			 * }
			 */
		}
	}
}
