using System;
using UnityEngine;
using UnityEngine.ScriptableObjectWizard;

public abstract class Item {

	public readonly ItemInfo info;

	protected IInventory inventory;

	protected Character character;

	protected Item( ItemInfo info ) {

		this.info = info;
	}

	public void SetCharacter( Character character ) {

		this.character = character;
	}

	public virtual void Apply() {
		
		throw new NotImplementedException();
	}
}

[HideInWizard]
public class ItemInfo : ScriptableObject {

	public string name;

	public ItemView groundView;

	public Sprite inventoryIcon;
	
	//public virtual void Apply( Character target ) {
		
	//	throw new NotImplementedException();
	//}

	public virtual Item GetItem() {
		
		throw new NotImplementedException();
	} 
}
