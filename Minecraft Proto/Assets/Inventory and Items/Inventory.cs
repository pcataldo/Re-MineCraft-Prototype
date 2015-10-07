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

	}

	void Update()
	{
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
		
		Rect screenRect = new Rect(0,0, slotsX*35, slotsY*35);
			for (int y = 0; y < slotsY; y++)	//Creates rows and columns of inventory slots
			{
				for(int x = 0; x< slotsX; x++)	
			{
				Rect slotRect = new Rect(x * 35, y * 35, 30, 30);
				GUI.Box (slotRect, "", skin.GetStyle("Slot"));	//attaches slot skin to GUI placement

				slots[i] = inventory[i];	//creates temperary inventory info holder

				if(slots[i].itemName != null)	//if slot contains item
				{
					GUI.DrawTexture(slotRect, slots[i].itemIcon);
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
}
