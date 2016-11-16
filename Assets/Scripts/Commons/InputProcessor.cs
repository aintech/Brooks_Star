using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputProcessor : MonoBehaviour {
	
	private static Stack<Closeable> closeables = new Stack<Closeable>();

	public static void add (Closeable closeable) { closeables.Push(closeable); }
	public static void closeLast () { closeables.Pop().close(true); }
	public static void removeLast () { if (closeables.Count > 0) { closeables.Pop(); }}

	public void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (closeables.Count > 0) { closeLast(); }
		}
	}
}