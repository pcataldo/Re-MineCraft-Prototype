using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour 
{
	public int slotsX, slotsY;
	public GUISkin skin;
	public List<Item> inventory = new List<Item>();
	public List<Item>slots = new List<Item>();
	private bool showInventory;
	private ItemDatabase database;
	private bool showToolTip;
	private string toolTip;
	private bool draggingItem;
	private Item draggedItem;
	private int prevIndex;


	// Use this for initialization
	void Start () 
	{
		for(int i = 0; i < (slotsX * slotsY); i++)
		{
			slots.Add (new Item());
			inventory.Add (new Item());
		}
		database = GameObject.FindGameObjectWithTag ("Item Database").GetComponent<ItemDatabase>();
		AddItem(0);
		AddItem(1);
		AddItem(2);
		AddItem(3);
		AddItem(4);
		AddItem(5);
		AddItem(6);
		AddItem(7);
		AddItem(8);
		AddItem(9);
		AddItem(10);
		AddItem(11);
		AddItem(12);
		AddItem(13);
		AddItem(14);
		/*AddItem(15);
		AddItem(16);
		AddItem(17);
		AddItem(18);
		AddItem(19);
		AddItem(20);
		AddItem(21);
		AddItem(22);
		AddItem(23);
		AddItem(24);
		AddItem(25);
		AddItem(26);
		AddItem(27);
		AddItem(28);
		AddItem(29);
		AddItem(30);
		AddItem(31);
		AddItem(32);
		AddItem(33);
		AddItem(34);
		AddItem(35);
		AddItem(36);
		AddItem(37);
		AddItem(38);
		AddItem(39);
		AddItem(40);
		AddItem(41);
		AddItem(42);
		AddItem(43);
		AddItem(44);
		AddItem(45);
		AddItem(46);
		AddItem(47);
		AddItem(38);
		*/



		
		

	}

	void Update()
	{
		//HotKeyButtons (); //Keep track of player using hotkey items

		if(Input.GetButtonDown("Inventory"))
		{
			if(Time.timeScale == 0)
			{
				Time.timeScale = 1;
			}
			else
				Time.timeScale = 0;
			showInventory = !showInventory;
		}
	}
	
		void OnGUI ()
		{
		toolTip = "";
		GUI.skin = skin;

			if(showInventory)
			{
				DrawInventory();
				
				if (showToolTip) 
				{
					GUI.Box (new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200, 200), toolTip, skin.GetStyle ("Tooltip"));
				}
			}

			if(draggingItem)
			{
				GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 50, 50), draggedItem.itemIcon);
			}
			
		}

		

	void DrawInventory()		//Responsible for creating inventory and managing which slots contain items as well as player's ability to manually move and sort items.
		{
		Event e = Event.current; //Helps cut down on typing by using e instead of Event.current
		int i = 0;
<<<<<<< HEAD
		
		Rect screenRect = new Rect(0,0, slotsX*35, slotsY*35);
			for (int y = 0; y < slotsY; y++)	//Creates rows and columns of inventory slots
			{
				for(int x = 0; x< slotsX; x++)	
			{
				Rect slotRect = new Rect(x * 35, y * 35, 30, 30);
				GUI.Box (slotRect, "", skin.GetStyle("Slot"));	//attaches slot skin to GUI placement

=======
		int x = 0;
		int y = 0;
		
		Rect screenRect = new Rect(0,0, slotsX*35 + 45, slotsY*35 + 45); //Used to drop items

		//Rect craftRect = new Rect(x * 35 + 150, y * 35 + 150, 30, 30);
		//GUI.Box (craftRect, "", skin.GetStyle("Slot"));
		
		
			for (y = 0; y < slotsY; y++)	//Creates rows and columns of inventory slots
			{
				for(x = 0; x< slotsX; x++)	
				{
				Rect slotRect = new Rect(x * 35, y * 35, 30, 30);
				GUI.Box (slotRect, "", skin.GetStyle("Slot"));	//attaches slot skin to GUI placement


>>>>>>> origin/master
				slots[i] = inventory[i];	//creates temperary inventory info holder

				if(slots[i].itemName != null)	//if slot contains item
				{
					GUI.DrawTexture(slotRect, slots[i].itemIcon);
					//GUI.DrawTexture(craftSlotRect, slots[i].itemIcon);

					if (slotRect.Contains(e.mousePosition))
					{
						toolTip = CreateToolTip(slots[i]);
						showToolTip = true;

						if (e.button == 0 && e.type == EventType.mouseDrag && !draggingItem)		//if left clicked on item and dragged
						{
							draggingItem = true;
							prevIndex = i;
							draggedItem = slots[i];	//placeholder for currently dragged item info
							inventory[i] = new Item();	//original slot emptied
						}

						if (e.type == EventType.mouseUp && draggingItem)		//What happens when letting go of mouse button with item being dragged
						{
							inventory[prevIndex] = inventory[i];
							inventory[i] = draggedItem;
							draggingItem = false;
							draggedItem = null;
						}
					}
				
				} 

				else		//If no item is in slot that currently dragged item is placed over. Basically the same code just without using placeholder prevIndex to swap items.
				{
					
					if (screenRect.Contains(e.mousePosition))
					{
						if (slotRect.Contains(e.mousePosition))
						{
							if(e.type == EventType.mouseUp && draggingItem)
							{
							print ("item move");
								inventory[i] = draggedItem;
								draggingItem = false;
								draggedItem = null;
							}
						}
					}
					else
					{
						if(e.type == EventType.mouseUp && draggingItem)
						{
							print ("item drop");
							GameObject droppedItem = GameObject.CreatePrimitive(PrimitiveType.Plane);
							droppedItem.transform.position = GameObject.Find("FPSController").transform.position;
							//droppedItem.AddComponent<Item>();
							draggingItem = false;
							draggedItem = null;
						}
					}
<<<<<<< HEAD

=======
>>>>>>> origin/master
				}


				if(toolTip == "")
				{
					showToolTip = false;
				}


				i++;
				}
			}

		}

	string CreateToolTip(Item item)
	{
		toolTip = "<color=#0b3861>" + item.itemName + "</color>\n" + item.itemDesc;
		return toolTip;
	}

	void RemoveItem(int id)		//Removes item by replacing with empty item slot (as to not destroy the slot itself)
	{
		for (int i = 0; i < inventory.Count; i++) 
		{
			if (inventory[i].itemID == id)
			{
				inventory[i] = new Item();
				break;
			}
		}
	}

	void AddItem(int id)		//Adds item to player inventory
	{
		for (int i = 0; i < inventory.Count; i++) 
		{
			if (inventory [i].itemName == null)
			{
				for (int j = 0; j < database.items.Count; j++)
				{
					if (database.items[j].itemID == id)
					{
						inventory[i] = database.items[j];
					}
				}
					break;
			}
		}
	}


	bool InventoryContains(int id)		//This finds whether a player has a specific item in the inventory.
	{
		bool result = false;
		for (int i = 0; i < inventory.Count; i++) 
		{
			result = inventory[i].itemID == id;
			if (result)
			{
				break;
			}
		}
		return result;
	}

	public void HotKeyButtons()
	{
		if(Input.GetButtonDown("HotKey1"))
		{
			//use Item in HotKey1 - inventory[27]
		}
		if(Input.GetButtonDown("HotKey2"))
		{
			//use Item in HotKey2 - inventory[28]
		}
		if(Input.GetButtonDown("HotKey3"))
		{
			//use Item in HotKey3 - inventory[29]
		}
		if(Input.GetButtonDown("HotKey4"))
		{
			//use Item in HotKey4 - inventory[30]
		}
		if(Input.GetButtonDown("HotKey5"))
		{
			//use Item in HotKey5 - inventory[31]
		}
		if(Input.GetButtonDown("HotKey6"))
		{
			//use Item in HotKey6 - inventory[32]
		}
		if(Input.GetButtonDown("HotKey7"))
		{
			//use Item in HotKey7 - inventory[33]
		}
		if(Input.GetButtonDown("HotKey8"))
		{
			//use Item in HotKey8 - inventory[34]
		}
		if(Input.GetButtonDown("HotKey9"))
		{
			//use Item in HotKey9 - inventory[35]
		}

	}
}
