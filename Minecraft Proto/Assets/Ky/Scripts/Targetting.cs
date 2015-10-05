using UnityEngine;
using System.Collections;

public class Targetting : MonoBehaviour {
	public GameObject playerCam;
	public byte activeBlockType = 1;
	public Transform cubeAdd, cubeDel;
	public GameObject target;

	// Use this for initialization
	void Start () 
	{
		cubeAdd = Instantiate (cubeAdd);
		cubeDel = Instantiate (cubeDel);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Ray ray = new Ray (playerCam.transform.position + playerCam.transform.forward / 2, playerCam.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 5f)) {
			Vector3 p = hit.point - hit.normal / 2;
			cubeDel.position = new Vector3 (Mathf.Floor (p.x+0.5f), Mathf.Floor (p.y), Mathf.Floor (p.z+0.5f));

			p = hit.point + hit.normal / 2;
			cubeAdd.position = new Vector3 (Mathf.Floor (p.x+0.5f), Mathf.Floor (p.y), Mathf.Floor (p.z+0.5f)); 

			target = hit.transform.gameObject;
		} 
		else 
		{
			cubeAdd.position = new Vector3(0,-100,0);
			/*
			Vector3 tmp = transform.position + Vector3.forward*3;
			tmp.x = (int)tmp.x;
			tmp.y = (int)tmp.y;
			tmp.z = (int)tmp.z;
			cubeAdd.position = tmp;
*/
			cubeDel.position = new Vector3(0,-100,0);
			target = null;
		}
	}

	public GameObject getTarget()
	{
		return target;
	}
}
