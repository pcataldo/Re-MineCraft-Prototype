using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	GameObject target = null;
	public float speed = 2.0f;
	public float rotateSpeed = 2.0f;
	Vector3 startPos;
	public string EnemyType;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (target) 
		{
			Vector3 direction = enemyAction();
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);

			transform.Translate (Vector3.forward * Time.deltaTime * speed );	
		}
		else
		{
			if(Vector3.Distance(startPos, transform.position) > 1)
			{
				Vector3 direction = (startPos - transform.position).normalized;
				Quaternion lookRotation = Quaternion.LookRotation(direction);
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);

				transform.Translate (Vector3.forward * Time.deltaTime * speed );
			}
		}
	}

	public void setTarget(GameObject t)
	{
		target = t;
	}

	public Vector3 enemyAction()
	{
		Vector3 dir = (transform.position - target.transform.position).normalized;
		switch (EnemyType) 
		{
		case "Coward":
			dir = (transform.position - target.transform.position).normalized;
			break;
		case "Badass":
			dir = (target.transform.position - transform.position).normalized;
			break;
		default:
			break;
		}
		return dir;
	}
}
