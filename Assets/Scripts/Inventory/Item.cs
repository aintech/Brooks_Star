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

		updateQuantityText();

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

	public void updateQuantityText () {
		quantityText.text = itemData.quantity.ToString();
		quantityText.gameObject.SetActive(itemData.itemType == ItemType.GOODS && itemData.quantity > 1);
	}

	public ItemKind kind () {
		return  itemData.kind;
	}

	public void returnToParent () {
		if (cell != null) {
			cell.inventory.addItemToCell (this, cell);
		} else if (slot != null) {
			slot.setItem(this);
		} else {
			Debug.Log("Dont know where return item: " + itemName());
		}
	}

	public float volume () {
		return itemData.volume;
	}

	public int cost () {
		return itemData.cost;
	}

	public int energyNeeded () {
		return itemData.energyNeeded;
	}

	public ItemType type () {
		return itemData.itemType;
	}

	public ItemQuality quality () {
		return itemData.quality;
	}

	public float level () {
		return itemData.level;
	}

	public string itemName () {
		return itemData.name;
	}

	public string description () {
		return itemData.description;
	}

	public void destroy () {
		Destroy(gameObject);
	}
}