using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
	public string itemName;
	public int itemID;
	public string itemDesc;
	public Texture2D itemIcon;
	public int itemPower;
	public int itemDefense;
	public ItemType itemType;
		
		public enum ItemType 
		{
		Sword, Shovel, Pickaxe, Axe, Block, Helmet, Chest, Legs, Boots, Food, Crafting, Map
		}

	public Item(string name, int id, string desc, int power, int defense, ItemType type)
	{
		itemName = name;
		itemID = id;
		itemDesc = desc;
		itemIcon = Resources.Load<Texture2D>("Item Icons/" + name);
		itemPower = power;
		itemDefense = defense;
		itemType = type;
	}
	public Item()
	{

	}
	
}
