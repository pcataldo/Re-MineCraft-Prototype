using UnityEngine;
using System.Collections;

public class EnemyAggro : MonoBehaviour {
	EnemyAI enemy;
	GameObject player;
	
	// Use this for initialization
	void Start () {
		enemy = transform.parent.gameObject.GetComponent<EnemyAI>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player) {
			float dist = Vector3.Distance(player.transform.position, transform.position);
			if(dist > 10)
			{
				enemy.setTarget(null);
				player = null;
			}
		}
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player") 
		{
			player = other.gameObject;
			enemy.setTarget (other.gameObject);
		}
	}
}
