using UnityEngine;
using System.Collections;

public class Destroyable : MonoBehaviour {
	
	public int maxHP;
	public int currHP;
	public Texture[] textures;
	public GameObject explosion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (currHP < maxHP) 
		{
			currHP++;
		}
	}

	public void onHit(int damage)
	{
		if (damage < 2)
			damage = 2;

		currHP -= damage;

		if (currHP <= 0) 
		{
			die ();
		}
	}

	public void die()
	{
		World theWorld = GameObject.Find ("World Manager").GetComponent<World>();
		theWorld.RemoveBlock (gameObject, gameObject.transform.position);
		//Instantiate (explosion, transform.position, Quaternion.Euler(0,0,90));
		Destroy (gameObject);
	}
}
