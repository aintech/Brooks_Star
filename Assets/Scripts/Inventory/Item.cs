using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	
	private SpriteRenderer render;

	public TextMesh quantityText { get; private set; }

	private MeshRenderer quantityRender;

	public int index;

	[HideInInspector]
    public InventoryCell cell;

	[HideInInspector]
	public Slot slot;
    
	public ItemData itemData { get; private set; }

	public Item init (ItemData itemData, int index) {
		this.index = index;
		return init(itemData);
	}

	public Item init (ItemData itemData) {
		this.itemData = itemData;

		render = GetComponent<SpriteRenderer>();

		quantityText = transform.Find("Quantity").GetComponent<TextMesh>();
		quantityRender = quantityText.GetComponent<MeshRenderer>();
		quantityRender.sortingLayerName = "Inventory";
		quantityRender.sortingOrder = 3;

		quantityText.gameObject.SetActive(itemData.itemType == ItemType.GOODS);
		quantityText.text = itemData.quantity.ToString();

		switch (itemData.itemType) {
			case ItemType.WEAPON: render.sprite = ImagesProvider.getWeaponSprite(((WeaponData)itemData).type); break;
			case ItemType.ENGINE: render.sprite = ImagesProvider.getEngineSprite(((EngineData)itemData).type); break;
			case ItemType.ARMOR: render.sprite = ImagesProvider.getArmorSprite(((ArmorData)itemData).type); break;
			case ItemType.GENERATOR: render.sprite = ImagesProvider.getGeneratorSprite(((GeneratorData)itemData).type); break;
			case ItemType.RADAR: render.sprite = ImagesProvider.getRadarSprite(((RadarData)itemData).type); break;
			case ItemType.SHIELD: render.sprite = ImagesProvider.getShieldSprite(((ShieldData)itemData).type); break;
			case ItemType.REPAIR_DROID: render.sprite = ImagesProvider.getRepairDroidSprite(((RepairDroidData)itemData).type); break;
			case ItemType.HARVESTER: render.sprite = ImagesProvider.getHarvesterSprite(((HarvesterData)itemData).type); break;
			case ItemType.HAND_WEAPON: render.sprite = ImagesProvider.getHandWeaponSprite(((HandWeaponData)itemData).type); break;
			case ItemType.BODY_ARMOR: render.sprite = ImagesProvider.getBodyArmorSprite(((BodyArmorData)itemData).type); break;
			case ItemType.GOODS: render.sprite = ImagesProvider.getGoodsSprite(((GoodsData)itemData).type); break;
			default: Debug.Log("Unknown item type: " + itemData.itemType); break;
		}
		return this;
	}

	public void changeSortOrder (int newOrder) {
		render.sortingOrder = newOrder;
		quantityRender.sortingOrder = newOrder + 1;
	}

	public ItemKind getItemKind () {
		return  itemData.itemType.getKind();
	}

	public void returnToParent () {
		if (cell != null) {
			cell.inventory.addItemToCell (this, cell);
		} else if (slot != null) {
			slot.setItem(this);
		} else {
			Debug.Log("Dont know where return item: " + getItemName());
		}
	}

	public float getVolume () {
		return itemData.volume;
	}

	public int getCost () {
		return itemData.cost;
	}

	public int getEnergyNeeded () {
		return itemData.energyNeeded;
	}

	public ItemType getItemType () {
		return itemData.itemType;
	}

	public ItemQuality getItemQuality () {
		return itemData.quality;
	}

	public float getItemLevel () {
		return itemData.level;
	}

	public string getItemName () {
		return itemData.name;
	}

	public string getItemDescription () {
		return itemData.description;
	}
}