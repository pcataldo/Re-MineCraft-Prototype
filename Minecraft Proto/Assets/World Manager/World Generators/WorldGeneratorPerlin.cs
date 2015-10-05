using UnityEngine;
using System.Collections;

public class WorldGeneratorPerlin : MonoBehaviour {

	public GameObject theWorld;
	World world;
	
	public GameObject[] block;
	public int size;
	public float scale =  6.5f;
	public float[,] height;

	int x;
	int y;
	int z;

	void Awake()
	{
		if(theWorld == null)
			theWorld = GameObject.Find ("World Manager");
		if (theWorld.GetComponent<World> () != false)
			world = theWorld.GetComponent<World> ();
		
		Vector3 tmp = world.GetWorldSize ();
		size = (int)tmp.x;
	}

	// Use this for initialization
	void Start ()
	{
		block = theWorld.GetComponent<Blocks> ().blocks;

		height = new float[size,size];
		for (x = 0; x < size; x++) 
		{
			for (z = 0; z < size; z++) 
			{
				//GameObject tmp = Instantiate (block[0],new Vector3(x,-1,z), Quaternion.identity) as GameObject;
				//tmp.transform.parent = transform;
				height[x,z] = Mathf.PerlinNoise(x/scale,z/scale);

				//Renderer rend = tmp.transform.GetComponent<Renderer>();
				//rend.material.color = new Color (height[x,z],height[x,z],height[x,z]);
			}	
		}
		
		for (x=0; x < size; x++) 
		{
			for (z=0; z < size; z++) 
			{
				float tmp = height[x,z] * 5;
				for (y=0; y < tmp; y++) 
				{
					GameObject cube = null;
					if(tmp < 1.4f)
						cube = Instantiate (block[0],new Vector3(x,y,z), Quaternion.identity) as GameObject;
					else if(tmp < 2)
						cube = Instantiate (block[1],new Vector3(x,y,z), Quaternion.identity) as GameObject;
					else
						cube = Instantiate (block[2],new Vector3(x,y,z), Quaternion.identity) as GameObject;
					cube.transform.parent = transform;

					world.AddBlock(cube, new Vector3(x,y,z));
				}
			}
		}

		world.VisibleBlocks (true);

		x = 0;
		y = 0;
		z = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		for (; x < size; x++) 
		{
			for (; z < size; z++) 
			{
				for (; y < height[x,z]; y++) 
				{
					print ("X:" + x);
					print ("Y:" + y);
					print ("Z:" + z);
					GameObject tmp = Instantiate (block,new Vector3(x,y,z), Quaternion.identity) as GameObject;
					tmp.transform.parent = transform;
				}
			}
		}
		*/
	}
}
