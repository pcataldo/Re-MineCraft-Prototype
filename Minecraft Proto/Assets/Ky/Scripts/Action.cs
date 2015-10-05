using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour {
	public Targetting playerTarget;
	public KeyCode actionButton;
	public KeyCode actionButton2;
	public GameObject target;
	public int damage;

	// Use this for initialization
	void Start () {
		damage = 2;
	}
	
	// Update is called once per frame
	void Update () {
		target = playerTarget.getTarget ();
		if (target) {
			if(Input.GetKey(actionButton) || Input.GetKey(actionButton2))
			{
				target.GetComponent<Destroyable>().onHit(damage);	//DEFAULT 2
			}
		}
	}
}
