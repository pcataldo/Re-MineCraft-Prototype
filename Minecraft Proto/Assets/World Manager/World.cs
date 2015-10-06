using UnityEngine;
using System.Collections;

public class World : MonoBehaviour 
{
	public Blocks blocksScript;
	public GameObject[] blocks = new GameObject[0];	//block types

	public GameObject thePlayer;
	GameObject player = null;
	public bool useStart = false;
	
	public int chunkSize = 16;
	public int worldSizeX = 4;
	public int worldSizeZ = 4;
	public int height = 16;
	public int seaLevel = 56;

	public GameObject[,,] blockLocation;	//x,z coordinates in the world, Height in the world.

	public int viewDistance = 4;

	Vector3 playerLoc = new Vector3(8,4,8);
	Vector3 lastPlayerLoc = new Vector3 (0, 0, 0);

	//chunk cooridnates are based on the of the chunks 0,0,0 i.e. chunk 0,0,0 = Loc 0,0,0 chunk 0,1,0 = Loc 0,16,0;  Chunk 34,5,10 = Loc 544, 80, 160

	void Awake ()
	{
		blocksScript = GetComponent<Blocks> ();
		blocks = blocksScript.blocks;

		blockLocation = new GameObject[chunkSize * worldSizeX, chunkSize * height, chunkSize * worldSizeZ];

		lastPlayerLoc = playerLoc = new Vector3 (chunkSize * worldSizeX / 2, 3, chunkSize * worldSizeZ / 2);
	}


	void Start () 
	{
		//Will Instantiate an area of blocks for testing
			Vector3 popLoc = new Vector3 (0, 0, 0);
			int popBlock = 0;
			
			if(useStart)
			{
				for(int locH = 0; locH < 3; locH++)
				{
					for(int locX = 0; locX < blockLocation.GetUpperBound(0); locX++)
					{
						for(int locZ = 0; locZ < blockLocation.GetUpperBound(2); locZ++)
						{
							popBlock = Random.Range (0,2);
							popLoc = new Vector3(locX, locH, locZ);
							
							GameObject poppedBlock = Instantiate (blocks[popBlock], popLoc, transform.rotation ) as GameObject;
							blockLocation[locX, locH, locZ] = poppedBlock;
							poppedBlock.SetActive(false);
						}
					}
				}
			}

		//Check height of blocks at the player location and Instantiate y+1 higher.
		player = Instantiate (thePlayer, playerLoc, transform.rotation) as GameObject;

		//Initiate the visible blocks
		if(useStart)
			VisibleBlocks(true);
	}
	

	void Update () 
	{
		playerLoc = new Vector3((int)player.transform.position.x, (int)player.transform.position.y, (int)player.transform.position.z);

		if (playerLoc.x != lastPlayerLoc.x || playerLoc.z != lastPlayerLoc.z)
		{
			//change visible blocks;
			VisibleBlocks(false);
			lastPlayerLoc = playerLoc;
		}

	}

	public void AddBlock(GameObject block, Vector3 atPosition)
	{
		blockLocation[(int)atPosition.x, (int)atPosition.y, (int)atPosition.z] = block;
	}

	public void RemoveBlock(GameObject block, Vector3 fromPosition)	
	{
		blockLocation[(int)fromPosition.x, (int)fromPosition.y, (int)fromPosition.z] = null;
	}

	public void VisibleBlocks(bool reset = false)
	{
		Vector3 posDifference = new Vector3(playerLoc.x - lastPlayerLoc.x, playerLoc.y - lastPlayerLoc.y, playerLoc.z - lastPlayerLoc.z);
		float posDistance = Vector3.Distance (playerLoc, lastPlayerLoc);
		int drawBounds = viewDistance * chunkSize;

		if (reset)	//Turns off all block, then make all blocks in the viewDistance Active
		{	//turns them all off	
			for(int h = 0; h <= blockLocation.GetUpperBound(1); h++)
			{
				for(int x = 0; x <= blockLocation.GetUpperBound(0); x++)
				{
					for(int z = 0; z <= blockLocation.GetUpperBound(2); z++)
					{
						if( blockLocation[x,h,z] != null)
							blockLocation[x,h,z].SetActive(false);
					}
				}
			}
			//turns the ones in viewDistance on
			for(int x = (int)playerLoc.x - drawBounds; x <= playerLoc.x + drawBounds; x++)
			{
				if(x < 0 || x > blockLocation.GetUpperBound(0))
					continue;
				for(int z = (int)playerLoc.z - drawBounds; z <= playerLoc.z + drawBounds; z++)
				{
					if(z < 0 || z > blockLocation.GetUpperBound(2))
						continue;
					for(int h = 0; h <= blockLocation.GetUpperBound(1); h++)
					{
						if( blockLocation[x,h,z] != null)
							blockLocation[x,h,z].SetActive(true);
					}
				}
			}
		}
		else if(posDistance > 16f)
		{
			//turns the block in viewDistance at lastPlayerLoc off
			for(int x = (int)lastPlayerLoc.x - drawBounds; x <= lastPlayerLoc.x + drawBounds; x++)
			{
				if(x < 0 || x > blockLocation.GetUpperBound(0))
					continue;
				for(int z = (int)lastPlayerLoc.z - drawBounds; z <= lastPlayerLoc.z + drawBounds; z++)
				{
					if(z < 0 || z > blockLocation.GetUpperBound(2))
						continue;
					for(int h = 0; h <= blockLocation.GetUpperBound(1); h++)
					{
						if( blockLocation[x,h,z] != null)
							blockLocation[x,h,z].SetActive(false);
					}
				}
			}

			//turns the ones in viewDistance at PlayerLoc on
			for(int x = (int)playerLoc.x - drawBounds; x <= playerLoc.x + drawBounds; x++)
			{
				if(x < 0 || x > blockLocation.GetUpperBound(0))
					continue;
				for(int z = (int)playerLoc.z - drawBounds; z <= playerLoc.z + drawBounds; z++)
				{
					if(z < 0 || z > blockLocation.GetUpperBound(2))
						continue;
					for(int h = 0; h <= blockLocation.GetUpperBound(1); h++)
					{
						if( blockLocation[x,h,z] != null)
							blockLocation[x,h,z].SetActive(true);
					}
				}
			}
		}
		else  //shift the visble blocks in range
		{
			if(posDifference.x != 0)
			{
				//new Blocks to turn on
				//x on
				for(int h = 0; h < blockLocation.GetUpperBound(1); h++)
				{
					for(int z = (int)(playerLoc.z - drawBounds); z <= playerLoc.z + (drawBounds); z++)
					{
						if(z < 0 || z > blockLocation.GetUpperBound(2))
							continue;

						if(posDifference.x == 0)  //player hasn't moved, get out of here!
						{	Debug.Log("posDifference.x = 0.  Shouldn't be here" );
							break;
						}
						else if(posDifference.x > 0)  //player has moved x+
						{
							for(int x = (int)(lastPlayerLoc.x + drawBounds + 1); x <= playerLoc.x + drawBounds; x++)
							{
								if(x < 0 || x > blockLocation.GetUpperBound(0))
									continue;

								if(blockLocation[x,h,z] != null)
									blockLocation[x,h,z].SetActive(true);
							}
						}
						else if (posDifference.x < 0)  //player has moved x-
						{
							for(int x = (int)lastPlayerLoc.x - drawBounds - 1; x >= playerLoc.x - drawBounds; x--)
							{
								if(x < 0 || x > blockLocation.GetUpperBound(0))
									continue;

								if(blockLocation[x,h,z] != null)
									blockLocation[x,h,z].SetActive(true);
							}
						}
					}
				}

				//blocks to turn off
				//x off
				for(int h = 0; h < blockLocation.GetUpperBound(1); h++)
				{
					for(int z = (int)(playerLoc.z - drawBounds); z <= playerLoc.z + (drawBounds); z++)
					{
						if(z < 0 || z > blockLocation.GetUpperBound(2))
							continue;
						
						if(posDifference.x == 0)  //player hasn't moved, get out of here!
						{	Debug.Log("posDifference.x = 0.  Shouldn't be here" );
							break;
						}
						else if(posDifference.x > 0)  //player has moved x+
						{
							for(int x = (int)(lastPlayerLoc.x - drawBounds); x < playerLoc.x - drawBounds; x++)
							{
								if(x < 0 || x > blockLocation.GetUpperBound(0))
									continue;

								if(blockLocation[x,h,z] != null)
									blockLocation[x,h,z].SetActive(false);
							}
						}
						else if (posDifference.x < 0)  //player has moved x-
						{
							for(int x = (int)(lastPlayerLoc.x + drawBounds); x > playerLoc.x + drawBounds; x--)
							{
								if(x < 0 || x > blockLocation.GetUpperBound(0))
									continue;

								if(blockLocation[x,h,z] != null)
									blockLocation[x,h,z].SetActive(false);
							}
						}
					}
				}

			}

			if(posDifference.z != 0)
			{
				//z on
				for(int h = 0; h < blockLocation.GetUpperBound(1); h++)
				{
					for(int x = (int)(playerLoc.x - drawBounds); x <= playerLoc.x + (drawBounds); x++)
					{
						if(x < 0 || x > blockLocation.GetUpperBound(0))
							continue;
						
						if(posDifference.z == 0)  //player hasn't moved, get out of here!
						{	Debug.Log("posDifference.z = 0.  Shouldn't be here" );
							break;
						}
						else if(posDifference.z > 0)  //player has moved z+
						{
							for(int z = (int)(lastPlayerLoc.z + drawBounds + 1); z <= playerLoc.z + drawBounds; z++)
							{
								if(z < 0 || z > blockLocation.GetUpperBound(2))
									continue;
								
								if(blockLocation[x,h,z] != null)
									blockLocation[x,h,z].SetActive(true);
							}
						}
						else if (posDifference.z < 0)  //player has moved z-
						{
							for(int z = (int)lastPlayerLoc.z - drawBounds - 1; z >= playerLoc.z - drawBounds; z--)
							{
								if(z < 0 || z > blockLocation.GetUpperBound(2))
									continue;
								
								if(blockLocation[x,h,z] != null)
									blockLocation[x,h,z].SetActive(true);
							}
						}
					}
				}

				//z off
				for(int h = 0; h < blockLocation.GetUpperBound(1); h++)
				{
					for(int x = (int)(playerLoc.x - drawBounds); x <= playerLoc.x + (drawBounds); x++)
					{
						if(x < 0 || x > blockLocation.GetUpperBound(0))
							continue;
						
						if(posDifference.z == 0)  //player hasn't moved, get out of here!
						{	Debug.Log("posDifference.z = 0.  Shouldn't be here" );
							break;
						}
						else if(posDifference.z > 0)  //player has moved z+
						{
							for(int z = (int)(lastPlayerLoc.z - drawBounds); z < playerLoc.z - drawBounds; z++)
							{
								if(z < 0 || z > blockLocation.GetUpperBound(2))
									continue;
								
								if(blockLocation[x,h,z] != null)
									blockLocation[x,h,z].SetActive(false);
							}
						}
						else if (posDifference.z < 0)  //player has moved z-
						{
							for(int z = (int)(lastPlayerLoc.z + drawBounds); z > playerLoc.z + drawBounds; z--)
							{
								if(z < 0 || z > blockLocation.GetUpperBound(2))
									continue;
								
								if(blockLocation[x,h,z] != null)
									blockLocation[x,h,z].SetActive(false);
							}
						}
					}
				}
			}

			//if the player has change x and z positions at the same time, clean up the corners
			if(posDifference.x != 0 && posDifference.z != 0)
			{
				for(int h = 0; h < blockLocation.GetUpperBound(1); h++)
				{
					if(posDifference.x > 0)
					{
						for(int x = (int)(playerLoc.x - drawBounds - posDifference.x); x <= playerLoc.x - drawBounds; x++)
						{
							if(x < 0 || x > blockLocation.GetUpperBound(0))
								continue;

							if(posDifference.z > 0)  //player has moved z+
							{
								for(int z = (int)(playerLoc.z - drawBounds - posDifference.z); z <= playerLoc.z - drawBounds; z++)
								{
									if(z < 0 || z > blockLocation.GetUpperBound(2))
										continue;
									
									if(blockLocation[x,h,z] != null)
										blockLocation[x,h,z].SetActive(false);
								}
							}
							else if (posDifference.z < 0)  //player has moved z-
							{
								for(int z = (int)(playerLoc.z + drawBounds - posDifference.z); z >= playerLoc.z + drawBounds; z--)
								{
									if(z < 0 || z > blockLocation.GetUpperBound(2))
										continue;
									
									if(blockLocation[x,h,z] != null)
										blockLocation[x,h,z].SetActive(false);
								}
							}
						}
					}

					if(posDifference.x < 0)
					{
						for(int x = (int)(playerLoc.x + drawBounds - posDifference.x); x >= playerLoc.x + drawBounds; x--)
						{
							if(x < 0 || x > blockLocation.GetUpperBound(0))
								continue;
							
							if(posDifference.z > 0)  //player has moved z+
							{
								for(int z = (int)(playerLoc.z - drawBounds - posDifference.z); z <= playerLoc.z - drawBounds; z++)
								{
									if(z < 0 || z > blockLocation.GetUpperBound(2))
										continue;
									
									if(blockLocation[x,h,z] != null)
										blockLocation[x,h,z].SetActive(false);
								}
							}
							else if (posDifference.z < 0)  //player has moved z-
							{
								for(int z = (int)(playerLoc.z + drawBounds - posDifference.z); z >= playerLoc.z + drawBounds; z--)
								{
									if(z < 0 || z > blockLocation.GetUpperBound(2))
										continue;
									
									if(blockLocation[x,h,z] != null)
										blockLocation[x,h,z].SetActive(false);
								}
							}
						}
					}
					//end h
				}

			}
		}

	}

	public Vector3 GetWorldSize()
	{
		return new Vector3(chunkSize * worldSizeX, chunkSize * height, chunkSize * worldSizeZ);
	}
	
}
