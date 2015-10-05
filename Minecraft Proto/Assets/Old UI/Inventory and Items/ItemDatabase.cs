using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour 
{
	public List<Item> items = new List<Item>();

	void Start()
	{
		items.Add (new Item ("Wood Sword",0,"Makes good firewood.", 1, 1, Item.ItemType.Sword));
		items.Add (new Item ("Stone Sword",1,"Barney Rubble approves.", 2, 1, Item.ItemType.Sword));

	}
}
