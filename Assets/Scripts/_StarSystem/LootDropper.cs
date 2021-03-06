﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootDropper : MonoBehaviour {

	public Transform dropContainerPrefab;

	private static Transform dropContainer;

	private static List<LootContainer> containers = new List<LootContainer>();

	private static LootDisplay lootDisplay;

	public LootDropper init (Inventory inventory, ItemDescriptor itemDescriptor) {
		dropContainer = dropContainerPrefab;
		lootDisplay = GameObject.Find("Loot Display").GetComponent<LootDisplay>().init(inventory, itemDescriptor);
		return this;
	}

	public static void drop (Ship ship) {
		LootContainer container = null;
		foreach (LootContainer cont in containers) {
			if (!cont.onScene) {
				container = cont;
			}
		}
		if (container == null) {
			container = Instantiate<Transform>(dropContainer).GetComponent<LootContainer>().init(lootDisplay);
			containers.Add(container);
		}
		container.initDrop(ship);
	}

	public void clearAllLoot () {
		foreach (LootContainer container in containers) {
			container.hideDrop();
		}
	}

	public static void clearContainers () {
		containers.Clear();
	}
}