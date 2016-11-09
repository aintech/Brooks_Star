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

	public int quantity { get { return itemData.quantity; } set {itemData.quantity = value; updateQuantityText(); } }

	public float volume { get { return itemData.volume; } private set {;} }

	public ItemKind kind { get { return itemData.kind; } private set {;} }

	public int cost { get { return itemData.cost; } private set {;} }

	public int energyNeeded { get { return itemData.energyNeeded; } private set {;} }

	public ItemType type { get { return itemData.itemType; } private set {;} }

	public ItemQuality quality { get { return itemData.quality; } private set {;} }

	public float level { get { return itemData.level; } private set {;} }

	public string itemName { get { return itemData.name; } private set {;} }

	public string description { get { return itemData.description; } private set {;} }

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
			case ItemType.SUPPLY: render.sprite = ImagesProvider.getSupplySprite(((SupplyData)itemData).type); break;
			default: Debug.Log("Unknown item type: " + itemData.itemType); break;
		}
		return this;
	}

	public void changeSortOrder (int newOrder) {
		render.sortingOrder = newOrder;
		quantityRender.sortingOrder = newOrder + 1;
	}

	private void updateQuantityText () {
		quantityText.text = itemData.quantity.ToString();
		quantityText.gameObject.SetActive(itemData.itemType == ItemType.GOODS && itemData.quantity > 1);
	}

	public void returnToParent () {
		if (cell != null) {
			cell.inventory.addItemToCell (this, cell);
		} else if (slot != null) {
			slot.setItem(this);
		} else {
			Debug.Log("Dont know where return item: " + itemName);
		}
	}

	public void destroy () {
		Destroy(gameObject);
	}
}