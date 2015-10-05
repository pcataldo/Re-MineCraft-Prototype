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

		public Biome biome;
		public TerrainType terrain;

		public int[,,] blocks;  //stores the location of each block within the chunk

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
	
	public GameObject worldManager;
	World world;
	int chunkSize;
	int worldSizeX;
	int worldSizeZ;
	int worldHeight;

	public bool useIndicators = true;
	public GameObject heightIndicator;

	public Chunk[,] worldChunks = new Chunk[4,4];
	//int[,,] caves??  Or use Biome variable as reference?
	int xMax = 0;
	int zMax = 0;

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

		worldChunks = new Chunk[worldSizeX,worldSizeZ];

		xMax = worldChunks.GetUpperBound(0);
		zMax = worldChunks.GetUpperBound(1);
		
		for(int x = 0; x <= worldChunks.GetUpperBound(0); x++)
		{
			for(int z = 0; z <= worldChunks.GetUpperBound(1); z++)
			{
				worldChunks[x,z].beenSet = false;
				worldChunks[x,z].terrain = TerrainType.EMPTY;
				worldChunks[x,z].biome = Biome.EMPTY;
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
		if(useIndicators)
			HeightIndicators();
		DetermineTerrain();
		DetermineBiome();

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

		if(xMax > zMax)	
			iMax = (int)(zMax / 2) + zMax%2;
		else
			iMax = (int)(xMax / 2) + xMax%2;

		int variant = 0;
		for(int i = 1; i <= iMax; i++)
		{	variant = 0;
			for(int x = i; x <= xMax - i; x++)
			{
				variant = Random.Range (-1, 2) + Random.Range (0,2);

				worldChunks[x,i].height = i + variant;
				worldChunks[x,i].beenSet = true;

				variant = Random.Range (-1, 2) + Random.Range (0,2);

				worldChunks[x,zMax - i].height = i + variant;
				worldChunks[x,zMax - i].beenSet = true;

			}
			for(int z = i + 1; z <= zMax - i - 1; z++)
			{
				variant = Random.Range (-1, 2) + Random.Range (0,2);

				worldChunks[i,z].height = i + variant;
				worldChunks[i,z].beenSet = true;

				variant = Random.Range (-1, 2) + Random.Range (0,2);

				worldChunks[xMax - i,z].height = i + variant;
				worldChunks[xMax - i,z].beenSet = true;
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

			}
		}

	}

	void DetermineTerrain()	//add an arguement that will allow for different terrain types, like islands, continents, pangea, etc.
	{	
		//establsih edges as RAVINES. most likely to be turned into OCEAN
		for(int x = 1; x <= xMax - 1; x++)
		{
			worldChunks[x,0].terrain = TerrainType.RAVINE;
			worldChunks[x,zMax].terrain = TerrainType.RAVINE;
		}
		for(int z = 1; z <= zMax - 1; z++)
		{
			worldChunks[0,z].terrain = TerrainType.RAVINE;
			worldChunks[xMax,z].terrain = TerrainType.RAVINE;
		}

		//establish terrain inside randomly
		for(int x = 1; x <= xMax - 1; x++)
		{
			for(int z = 1; z <= zMax - 1; z++)
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
		for(int x = 0; x <= xMax; x++)
		{
			worldChunks[x,0].biome = Biome.OCEAN;
			worldChunks[x,zMax].biome = Biome.OCEAN;
		}
		for(int z = 1; z <= zMax - 1; z++)
		{
			worldChunks[0,z].biome = Biome.OCEAN;
			worldChunks[xMax,z].biome = Biome.OCEAN;
		}

		//establish Biomes inside randomly
		for(int x = 1; x <= xMax - 1; x++)
		{
			for(int z = 1; z <= zMax - 1; z++)
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

	void BuildTerrain(TerrainType terrain)
	{
		for(int x = 0; x <= xMax; x++)
		{
			for(int z =0; z<= zMax; z++)
			{
				switch(terrain)
				{
				case TerrainType.FLAT:
					BuildFlat(x,z);
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
		for(int x = 0; x <= xMax; x++)
		{
			for(int z =0; z<= zMax; z++)
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

	void BuildFlat(int chunkX, int chunkZ)
	{
		//add scalar from x-1 baseheight to x + 1 baseheight,	scalarEnd = 0, which is the baseHeight		going outer to chunk center
		//add Scalar from z-1 baseheihgt to x+1 baseheight		scalarStart = other.baseheight - baseheight
		//block heights are baseheight + scalar.
		//add a random variant to each column and row, variants should be no more than 0.5 each. (can add multipliers based on terrain type)
		//scalar mid positions (7,8 ?) need to be at the baseHeight.  
			//x - x +7 is leftHeight - baseheight, x+8 - x + 15 is baseheight - rightHeight
			//z - z +7 is downHieght - baseheight, z+8 - z + 15 is baseHeight = upHeight

		Chunk chunk = worldChunks [chunkX,chunkZ];

		//get the chunks height
		int chunkHeight = chunk.height;
		int up;
		int down;
		int left;
		int right;

		//get all the surrounding chunk heights
		if (chunkZ == 0)
			up = chunkHeight;
		else
			up = worldChunks [chunkX, chunkZ + 1].height;

		if (chunkZ == zMax)
			down = chunkHeight;
		else
			down = worldChunks [chunkX, chunkZ - 1].height;

		if (chunkX == 0)
			left = chunkHeight;
		else
			left = worldChunks [chunkX - 1, chunkZ].height;

		if (chunkX == xMax)
			right = chunkHeight;
		else
			right = worldChunks [chunkX + 1, chunkZ].height;

		//determine scalars
		float[,] blockAdj = new float[chunk.blocks.GetUpperBound(0), chunk.blocks.GetUpperBound(1)];

		for(int x = 0; x < xMax/2; x++)
		{

		}


	}


}
