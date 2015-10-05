using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour 
{
	public List<Item> items = new List<Item>();

	void Start()
	{
		//-----------Weapons/Tools
		//Wood
		items.Add (new Item ("Wood Sword",0,"Makes good firewood.\n\nPower: 1", 1, 1, Item.ItemType.Sword));
		items.Add (new Item ("Wood Shovel",1,"Might want to just use your hands.\n\nPower: 1", 1, 1, Item.ItemType.Shovel));
		items.Add (new Item ("Wood Pickaxe",2,"Can crack stone. I swear./n/nPower: 1", 1, 1, Item.ItemType.Pickaxe));
		items.Add (new Item ("Wood Axe",3,"Better than punching the tree down with your fists./n/nPower: 1", 1, 1, Item.ItemType.Axe));
		//Stone
		items.Add (new Item ("Stone Sword", 4, "Barney Rubble approves.\n\nPower: 2", 2, 1, Item.ItemType.Sword));
		items.Add (new Item ("Stone Shovel",5,"Really should lay off the herb.\n\nPower: 2", 2, 1, Item.ItemType.Shovel));
		items.Add (new Item ("Stone Pickaxe",6,"Maybe you'll get lucky and it'll work.\n\nPower: 2", 2, 1, Item.ItemType.Pickaxe));
		items.Add (new Item ("Stone Axe",7,"It'll chip away at the bark. One day.\n\nPower: 2", 2, 1, Item.ItemType.Axe));
		//Iron
		items.Add (new Item ("Iron Sword",8,"Beats stone./n/nPower: 3", 3, 1, Item.ItemType.Sword));
		items.Add (new Item ("Iron Shovel",9,"Actually an effective means of digging\n\nPower: 3.", 3, 1, Item.ItemType.Shovel));
		items.Add (new Item ("Iron Pickaxe",10,"It's actually a pickaxe.\n\nPower: 3", 3, 1, Item.ItemType.Pickaxe));
		items.Add (new Item ("Iron Axe",11,"Don't lose your head over it.\n\nPower: 3", 3, 1, Item.ItemType.Axe));
		//Gold
		items.Add (new Item ("Gold Sword",12,"Quite fancy.\n\nPower: 4", 4, 1, Item.ItemType.Sword));
		items.Add (new Item ("Gold Shovel",13,"Fancy way to bury the body.\n\nPower: 4", 4, 1, Item.ItemType.Shovel));
		items.Add (new Item ("Gold Pickaxe",14,"Crack that rock open with some swag.\n\nPower: 4", 4, 1, Item.ItemType.Pickaxe));
		items.Add (new Item ("Gold Axe",15,"Not the video game.\n\nPower: 4", 4, 1, Item.ItemType.Axe));
		//Diamond
		items.Add (new Item ("Diamond Sword",16,"Nothing stronger.\n\nPower: 5", 5, 1, Item.ItemType.Sword));
		items.Add (new Item ("Diamond Shovel",17,"If you love her, bury her with diamonds.\n\nPower: 5", 5, 1, Item.ItemType.Shovel));
		items.Add (new Item ("Diamond Pickaxe",18,"No stones stand a chance.\n\nPower: 5", 5, 1, Item.ItemType.Pickaxe));
		items.Add (new Item ("Diamond Axe",19,"Comes with a life time warranty.\n\nPower: 5", 5, 1, Item.ItemType.Axe));

		//-----------Map
		items.Add (new Item ("Map",50,"Better draw it neatly.", 1, 1, Item.ItemType.Map));

		//-----------Armor
		//Leather
		items.Add (new Item ("Leather Helmet",20,"Well it's fashionable. I think.\n\nDefense: 2", 1, 2, Item.ItemType.Helmet));
		items.Add (new Item ("Leather Chest",21,"Biker gang certified.\n\nDefense: 2", 1, 2, Item.ItemType.Chest));
		items.Add (new Item ("Leather Legs",22,"Straight from the 80's.\n\nDefense: 2", 1, 2, Item.ItemType.Legs));
		items.Add (new Item ("Leather Boots",23,"Recently spit shined.\n\nDefense: 2", 1, 2, Item.ItemType.Boots));
		//Iron
		items.Add (new Item ("Iron Helmet",24,"Lookin chivalrous.\n\nDefense: 3", 1, 3, Item.ItemType.Helmet));
		items.Add (new Item ("Iron Chest",25,"Helps soften the hurt.\n\nDefense: 3", 1, 3, Item.ItemType.Chest));
		items.Add (new Item ("Iron Legs",26,"Breathes less than the leather.\n\nDefense: 3", 1, 3, Item.ItemType.Legs));
		items.Add (new Item ("Iron Boots",27,"Sure to clank.\n\nDefense: 3", 1, 3, Item.ItemType.Boots));
		//Gold
		items.Add (new Item ("Gold Helmet",28,"Swag hat.\n\nDefense: 4", 1, 4, Item.ItemType.Helmet));
		items.Add (new Item ("Gold Chest",29,"swag armor.\n\nDefense: 4", 1, 4, Item.ItemType.Chest));
		items.Add (new Item ("Gold Legs",30,"Swag pants.\n\nDefense: 4", 1, 4, Item.ItemType.Legs));
		items.Add (new Item ("Gold Boots",31,"Swag boots.\n\nDefense: 4", 1, 4, Item.ItemType.Boots));
		//Diamond
		items.Add (new Item ("Diamond Helmet",32,"Will definitely protect that pea-brain.\n\nDefense: 5", 1, 5, Item.ItemType.Helmet));
		items.Add (new Item ("Diamond Chest",33,"Stand firm like Super Man.\n\nDefense: 5", 1, 5, Item.ItemType.Chest));
		items.Add (new Item ("Diamond Legs",34,"Yes. You butt looks good in that.\n\nDefense: 5", 1, 5, Item.ItemType.Legs));
		items.Add (new Item ("Diamond Boots",35,"Step on all the nails you want.\n\nDefense: 5", 1, 5, Item.ItemType.Boots));

		//-----------Crafting
		items.Add (new Item ("Coal",36,"Used for fires.", 1, 1, Item.ItemType.Crafting));
		items.Add (new Item ("Stick",37,"Good for lots of things.", 1, 1, Item.ItemType.Crafting));
		items.Add (new Item ("Stone",38,"Throw it at someone. Or use it for science.", 1, 1, Item.ItemType.Crafting));
		items.Add (new Item ("String",39,"My tapeworm tells me what to do.", 1, 1, Item.ItemType.Crafting));
		items.Add (new Item ("Iron Ingot",40,"Ready for a blacksmith.", 1, 1, Item.ItemType.Crafting));
		items.Add (new Item ("Gold Ingot",41,"Maybe worth more to sell than make something with.", 1, 1, Item.ItemType.Crafting));
		items.Add (new Item ("Diamond Ore",42,"Makes the best stuff in the game.", 1, 1, Item.ItemType.Crafting));
		items.Add (new Item ("Raw Chicken",43,"Definitely has bird flu.", 1, 1, Item.ItemType.Crafting));
		items.Add (new Item ("Raw Pork",44,"Bacon everything.", 1, 1, Item.ItemType.Crafting));
		items.Add (new Item ("Raw Steak",45,"Already ready to eat.", 1, 1, Item.ItemType.Crafting));

		//-----------food
		items.Add (new Item ("Cooked Chicken",46,"Better than KFC. That wasn't a compliment.", 1, 1, Item.ItemType.Food));
		items.Add (new Item ("Cooked Pork",47,"You failed to make Bacon.", 1, 1, Item.ItemType.Food));
		items.Add (new Item ("Cooked Steak",48,"Tasted better Raw.", 1, 1, Item.ItemType.Food));
		items.Add (new Item ("Lying Cake",49,"It's a lie.", 1, 1, Item.ItemType.Food));



	}
}
