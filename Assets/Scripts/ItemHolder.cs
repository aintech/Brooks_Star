using UnityEngine;
using System.Collections;

public abstract class ItemHolder : MonoBehaviour {
//	public bool itemChanged;
//	public abstract ItemQuality getQuality ();
//	public abstract string getName ();
//	public bool haveDescribableObject { get; protected set; }
	public Item item { get; protected set; }
}