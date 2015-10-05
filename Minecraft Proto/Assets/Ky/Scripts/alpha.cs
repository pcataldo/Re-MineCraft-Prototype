using UnityEngine;
using System.Collections;

public class alpha : MonoBehaviour {
	Mesh mesh;

	// Use this for initialization
	void Start () 
	{
		mesh = transform.GetComponent<MeshFilter> ().mesh;
		updateCrack(0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Z)) 
		{
			updateCrack(2);
		}
		if (Input.GetKeyDown (KeyCode.X)) 
		{
			updateCrack(5);
		}
		if (Input.GetKeyDown (KeyCode.C)) 
		{
			updateCrack(9);
		}
	}

	public void updateCrack(float dmg)
	{
		/*
		mesh.uv = new Vector2[] {
			new Vector2(0, 0+dmg),    new Vector2(0, 0.1f+dmg),    new Vector2(1, 0.1f+dmg),    new Vector2 (1, 0+dmg),
			new Vector2(0, 0+dmg),    new Vector2(0, 0.1f+dmg),    new Vector2(1, 0.1f+dmg),    new Vector2 (1, 0+dmg),
			new Vector2(0, 0+dmg),    new Vector2(0, 0.1f+dmg),    new Vector2(1, 0.1f+dmg),    new Vector2 (1, 0+dmg),
			new Vector2(0, 0+dmg),    new Vector2(0, 0.1f+dmg),    new Vector2(1, 0.1f+dmg),    new Vector2 (1, 0+dmg),
			new Vector2(0, 0+dmg),    new Vector2(0, 0.1f+dmg),    new Vector2(1, 0.1f+dmg),    new Vector2 (1, 0+dmg),
			new Vector2(0, 0+dmg),    new Vector2(0, 0.1f+dmg),    new Vector2(1, 0.1f+dmg),    new Vector2 (1, 0+dmg),
		};
		*/
		mesh.uv = new Vector2[] {
			new Vector2(0, 0),    new Vector2(0, 0.5f),    new Vector2(1, 1),    new Vector2 (1, 0),
			new Vector2(0, 0),    new Vector2(0, 0.5f),    new Vector2(1, 1),    new Vector2 (1, 0),
			new Vector2(0, 0),    new Vector2(0, 0.5f),    new Vector2(1, 1),    new Vector2 (1, 0),
			new Vector2(0, 0),    new Vector2(0, 0.5f),    new Vector2(1, 1),    new Vector2 (1, 0),
			new Vector2(0, 0),    new Vector2(0, 0.5f),    new Vector2(1, 1),    new Vector2 (1, 0),
			new Vector2(0, 0),    new Vector2(0, 0.5f),    new Vector2(1, 1),    new Vector2 (1, 0),
		};
	}
}
