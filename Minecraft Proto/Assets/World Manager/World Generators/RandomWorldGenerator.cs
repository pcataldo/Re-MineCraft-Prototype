using UnityEngine;
using System.Collections;

public class RandomWorldGenerator : MonoBehaviour 
{
	public enum TerrainType	//determines the shape of the land
	{
		EMPTY,	//No Terrain, will need to be assigned
		FLAT,
		HILL,		//increased height, with a gradual shift
		MOUNTAIN,	//Large increas in height, sudden shift
		SINK,		//gradual decreas in height, use for RIVERS and LAKES
		RAVINE,		//large sudden drop in height, use for OCEANS
		AIR 	//Terrain is AIR. There are no blocks within it. can have clouds.

	}

	public enum Biome	//determines what blocks go onto the land
	{
		EMPTY,	//No Biome, will need to be assigned.
		PLAIN,	//default BIOME.  all other BIOMEs will build their edges from a PLAIN.  uses grass on dirt
		FOREST,	//grassy dirt, and trees
		ROCK,	//no grass, predominantly stone
		DESERT,
		OCEAN,	//will build beaches from PLAIN edges
		LAKE,	//has a gradual decrease in height
		RIVER,			
		SNOW,
		SWAMP,
		LAVA	//*** not randomly determined at the moment, must be assigned ***

		//Determine water and LAVA levels from the lowest surrounding chunk height.
	}

	public struct Chunk
	{
		public bool beenSet;

		public TerrainType terrain;
		public Biome biome;

		public int[,] blockHeights;	//used by BuildTerrain() and BuildBiome() to store the locations of the surface blocks

		public int[,,] blocks;  //stores the location of each block within the chunk, used by BuildTerrain() for surface blocks and BuildBiome()

		public int height; //base chunkHeight
		public int up;
		public int down;
		public int left;
		public int right;

		//Biome Type	//used to determine block types and patterns.
		//Biome variables?
		//direction 0 - 8 0 = none; 1-8 = 8-directions clockwise starting at up; 9 means up and down, or surrounded;
		//with most land, direction would define where the edges are, or where the slope would go, **
		// in water a 0 would mean all water, a 9 would be a small pond, directions would create a river
		//** this could also be determined using the surroundiong average heights and simply created

	}

	public bool useIndicators = true;
	public GameObject heightIndicator;

	public GameObject worldManager;
	World world;
	int chunkSize;
	int worldSizeX;
	int worldSizeZ;
	int worldHeight;
	int seaLevel;
	
	public Chunk[,] worldChunks = new Chunk[4,4];
	//int[,,] caves??  Or use Biome variable as reference?

	public GameObject[] blocks = new GameObject[0];
	int xChunkMax = 0;
	int zChunkMax = 0;

	void Awake()
	{


	}

	void Start () 
	{
		if(worldManager == null)
			worldManager = GameObject.Find("World Manager");
		world = worldManager.GetComponent<World>();

		chunkSize = world.chunkSize;
		worldSizeX = world.worldSizeX;
		worldSizeZ = world.worldSizeZ;
		worldHeight = world.height;
		seaLevel = world.seaLevel;

		worldChunks = new Chunk[worldSizeX,worldSizeZ];

		blocks = world.blocks;

		xChunkMax = worldChunks.GetUpperBound(0);
		zChunkMax = worldChunks.GetUpperBound(1);
		
		for(int x = 0; x <= worldChunks.GetUpperBound(0); x++)
		{
			for(int z = 0; z <= worldChunks.GetUpperBound(1); z++)
			{
				worldChunks[x,z].beenSet = false;
				worldChunks[x,z].terrain = TerrainType.EMPTY;
				worldChunks[x,z].biome = Biome.EMPTY;
				worldChunks[x,z].blockHeights = new int[chunkSize,chunkSize];
				worldChunks[x,z].blocks = new int[chunkSize,chunkSize * worldHeight,chunkSize];
				worldChunks[x,z].height = 0;
				worldChunks[x,z].up = 0;
				worldChunks[x,z].down = 0;
				worldChunks[x,z].left = 0;
				worldChunks[x,z].right = 0;
			}
		}

		BuildEdges();
		BaseHeight();
		DetermineTerrain();
		DetermineBiome();
		if(useIndicators)
			HeightIndicators();
		BuildTerrain();


	}
	

	void Update () 
	{
	
	}

	void BuildEdges()	//will set the outer edges of teh world to a height of 0;
	{
		for(int x = 0; x <= worldChunks.GetUpperBound(0); x++)
		{
			worldChunks[x,0].height = 0;
			worldChunks[x,worldChunks.GetUpperBound(1)].height = 0;

			worldChunks[x,0].beenSet = true;
			worldChunks[x,worldChunks.GetUpperBound(1)].beenSet = true;
		}
		for(int z = 1; z <= worldChunks.GetUpperBound(1) - 1; z++)
		{
			worldChunks[0,z].height = 0;
			worldChunks[worldChunks.GetUpperBound(0),z].height = 0;

			worldChunks[0,z].beenSet = true;
			worldChunks[worldChunks.GetUpperBound(0),z].beenSet = true;
		}

	}

	void BaseHeight()	//establsih the base height of each chunk before Biomes are determined
	{
			//get average surrounding hieght
				//if chunk !beenSet then don't count it in the average

		int iMax;

		if(xChunkMax > zChunkMax)	
			iMax = (int)(zChunkMax / 2) + zChunkMax%2;
		else
			iMax = (int)(xChunkMax / 2) + xChunkMax%2;

		int variant = 0;
		for(int i = 1; i <= iMax; i++)
		{	variant = 0;
			for(int x = i; x <= xChunkMax - i; x++)
			{
				variant = Random.Range (-1, 2) + Random.Range (0,2);

				worldChunks[x,i].height = i + variant;
				worldChunks[x,i].beenSet = true;

				variant = Random.Range (-1, 2) + Random.Range (0,2);

				worldChunks[x,zChunkMax - i].height = i + variant;
				worldChunks[x,zChunkMax - i].beenSet = true;

			}
			for(int z = i + 1; z <= zChunkMax - i - 1; z++)
			{
				variant = Random.Range (-1, 2) + Random.Range (0,2);

				worldChunks[i,z].height = i + variant;
				worldChunks[i,z].beenSet = true;

				variant = Random.Range (-1, 2) + Random.Range (0,2);

				worldChunks[xChunkMax - i,z].height = i + variant;
				worldChunks[xChunkMax - i,z].beenSet = true;
			}
		}

	}

	void HeightIndicators()
	{
		for(int x = 0; x <= worldChunks.GetUpperBound(0); x++)
		{
			for(int z = 0; z <= worldChunks.GetUpperBound(1); z++)
			{
				Vector3 pos = new Vector3( (x * chunkSize ) + (chunkSize/2), worldChunks[x,z].height, (z * chunkSize) + (chunkSize/2) );

				GameObject indicator = Instantiate(heightIndicator, pos, Quaternion.identity) as GameObject;
				indicator.name = "Incidicator - [" + x + "," + z + "] " + "Height: " + worldChunks[x,z].height + " " + worldChunks[x,z].beenSet;

				switch (worldChunks[x,z].terrain)
				{
					case TerrainType.FLAT:
					indicator.GetComponent<Indicator>().terrainType = "Flat";
						break;
				default:
						indicator.GetComponent<Indicator>().terrainType = "other";
						break;
				}

				switch (worldChunks[x,z].biome)
				{
				case Biome.PLAIN:
					indicator.GetComponent<Indicator>().biome = "Plain";
					break;
				default:
					indicator.GetComponent<Indicator>().biome = "other";
					break;
				}


			}
		}

	}

	void DetermineTerrain()	//add an arguement that will allow for different terrain types, like islands, continents, pangea, etc.
	{	
		//establsih edges as RAVINES. most likely to be turned into OCEAN
		for(int x = 1; x <= xChunkMax - 1; x++)
		{
			worldChunks[x,0].terrain = TerrainType.RAVINE;
			worldChunks[x,zChunkMax].terrain = TerrainType.RAVINE;
		}
		for(int z = 1; z <= zChunkMax - 1; z++)
		{
			worldChunks[0,z].terrain = TerrainType.RAVINE;
			worldChunks[xChunkMax,z].terrain = TerrainType.RAVINE;
		}

		//establish terrain inside randomly
		for(int x = 1; x <= xChunkMax - 1; x++)
		{
			for(int z = 1; z <= zChunkMax - 1; z++)
			{
				int i = Random.Range (1,6);

				if(i == 1)
					worldChunks[x,z].terrain = TerrainType.FLAT;
				if(i == 2)
					worldChunks[x,z].terrain = TerrainType.HILL;
				if(i == 3)
					worldChunks[x,z].terrain = TerrainType.MOUNTAIN;
				if(i == 4)
					worldChunks[x,z].terrain = TerrainType.SINK;
				if(i == 5)
					worldChunks[x,z].terrain = TerrainType.RAVINE;
			}
		}

	}

	void DetermineBiome()	//add an argument to be able to choose a particular climate, like temperate, tropical, artic
	{
		//establish edges as OCEAN
		for(int x = 0; x <= xChunkMax; x++)
		{
			worldChunks[x,0].biome = Biome.OCEAN;
			worldChunks[x,zChunkMax].biome = Biome.OCEAN;
		}
		for(int z = 1; z <= zChunkMax - 1; z++)
		{
			worldChunks[0,z].biome = Biome.OCEAN;
			worldChunks[xChunkMax,z].biome = Biome.OCEAN;
		}

		//establish Biomes inside randomly
		for(int x = 1; x <= xChunkMax - 1; x++)
		{
			for(int z = 1; z <= zChunkMax - 1; z++)
			{
				int i = Random.Range (1,7);
				
				if(i == 1)
					worldChunks[x,z].biome = Biome.PLAIN;
				if(i == 2)
					worldChunks[x,z].biome = Biome.FOREST;
				if(i == 3)
					worldChunks[x,z].biome = Biome.ROCK;
				if(i == 4)
					worldChunks[x,z].biome = Biome.DESERT;
				if(i == 5)
					worldChunks[x,z].biome = Biome.OCEAN;
				if(i == 6)
					worldChunks[x,z].biome = Biome.LAKE;
				if(i == 7)
					worldChunks[x,z].biome = Biome.RIVER;
				if(i == 8)
					worldChunks[x,z].biome = Biome.SNOW;
				if(i == 9)
					worldChunks[x,z].biome = Biome.SWAMP;
			}
		}
	}

	void BuildTerrain()
	{
		TerrainType terrainType;

		for(int x = 0; x <= xChunkMax; x++)
		{
			for(int z =0; z<= zChunkMax; z++)
			{
				terrainType = worldChunks[x,z].terrain;
				switch(terrainType)
				{
				case TerrainType.FLAT:
					BuildFlat(x,z, 0);
					break;
				case TerrainType.HILL:
					
					break;
				case TerrainType.MOUNTAIN:
					
					break;
				case TerrainType.SINK:
					
					break;
				case TerrainType.RAVINE:
					
					break;
				}
			}
		}

	}

	void BuildBiome(Biome biome)
	{
		for(int x = 0; x <= xChunkMax; x++)
		{
			for(int z =0; z<= zChunkMax; z++)
			{
				switch(biome)
				{
				case Biome.PLAIN:

					break;
				case Biome.FOREST:
					
					break;
				case Biome.ROCK:
					
					break;
				case Biome.DESERT:
					
					break;
				case Biome.OCEAN:
					
					break;
				case Biome.LAKE:
					
					break;
				case Biome.RIVER:
					
					break;
				case Biome.SNOW:
					//no current function
					break;
				case Biome.SWAMP:
					//no current function
					break;
				}
			}
		}

	}

	void BuildFlat(int chunkX, int chunkZ, float terrainMod)	//the array coordinates need to be passed for the chunk that's being processed.  They are needed for reference
{					//terrainMod is a multiplier that allows for different heights to be applied to the chunks over all height.  the value sets the height at the center of the chunk
		//add a random variant to each column and row, variants should be no more than 0.5 each. (can add multipliers based on terrain type)

		Chunk chunk = worldChunks [chunkX,chunkZ];

		//get the chunks height
		int chunkHeight = chunk.height;
		int up;
		int down;
		int left;
		int right;

		//get all the surrounding chunk heights
		if (chunkZ == zChunkMax)
			up = chunkHeight;
		else
			up = worldChunks [chunkX, chunkZ + 1].height;

		if (chunkZ == 0)
			down = chunkHeight;
		else
			down = worldChunks [chunkX, chunkZ - 1].height;

		if (chunkX == 0)
			left = chunkHeight;
		else
			left = worldChunks [chunkX - 1, chunkZ].height;

		if (chunkX == xChunkMax)
			right = chunkHeight;
		else
			right = worldChunks [chunkX + 1, chunkZ].height;

		//determine adjustments due to surrounding heights.  adjustments will be calculated at 50% each direction because each point will have at least two adjustments applied to it.
		float[,] blockAdj = new float[chunkSize, chunkSize];	//the 50% reduction will prevent adjustments from exceeding surrounding baseheights

		int xMax = chunk.blockHeights.GetUpperBound(0);
		int zMax = chunk.blockHeights.GetUpperBound(1);


		//initialize array
		for(int x = 0; x <= xMax; x++)
		{
			for(int z = 0; z <= zMax; z++)
			{
				blockAdj[x,z] = 0f;
			}
		}

		int difference = left - chunkHeight;
		for(int x = 0; x < (int)xMax/2f; x++)
		{
			for(int z = 0; z <= zMax; z++)
			{	
				float adj = 0f;
				//find adjustment from left to center
				adj = difference - ( (x+1) * ( difference/(chunkSize/2f) ) );
				blockAdj[x,z] += adj / 2f;
			}
		}

		difference = right - chunkHeight;
		for(int x = (int)xMax/2 + 1; x <= xMax; x++)
		{
			for(int z = 0; z <= zMax; z++)
			{	
				float adj = 0f;
				//find adjustment from center to right   
				adj = difference - ( (chunkSize - x) * ( difference/(chunkSize/2f) ) );
				blockAdj[x,z] += adj / 2f;
			}
		}

		difference = down - chunkHeight;
		for(int z = 0; z < (int)zMax/2f; z++)
		{	
			for(int x = 0; x <= xMax; x++)
			{	
				float adj = 0f;
				//find adjustment from down to center
				adj = difference - ( (z+1) * ( difference/(chunkSize/2f) ) );
				blockAdj[x,z] += adj / 2f;
			}
		}

		difference = up - chunkHeight;
		for(int z = (int)zMax/2 + 1; z <= zMax; z++)
		{
			for(int x = 0; x <= xMax; x++)
			{	
				float adj = 0f;
				//find adjustment from center to up
				adj = difference - ( (chunkSize - z) * ( difference/(chunkSize/2f) ) );
				blockAdj[x,z] += adj / 2f;
			}
		}

		//add other random adjustments.  or PerlinNoise it.

		//store the surface block locations for the chunk;
		for(int x = 0; x <= xMax; x++)
		{
			for(int z = 0; z <= zMax; z++)
			{
				int y = (int)(seaLevel + chunk.height + blockAdj[x,z]);
				chunk.blockHeights[x,z] = y;
			}
		}

		//block creation for testing
		for(int x = 0; x <= xMax; x++)
		{
			for(int z = 0; z <= zMax; z++)
			{
				Vector3 worldPos = new Vector3( (chunkX * chunkSize) + x, chunk.blockHeights[x,z], (chunkZ * chunkSize) + z);
				GameObject block = Instantiate(blocks[2], worldPos, Quaternion.identity) as GameObject;
				world.AddBlock(block, worldPos);
				block.name = "Adj = " + blockAdj[x,z];
			}
		}
		//create and add block to world.blocks
		//create blocks at new Vector3(chunks x, chunk.height + blockAdj[x,z],chunks z);



	}


}
