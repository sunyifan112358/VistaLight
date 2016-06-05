using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[Serializable]
public class PriorityQueue {
	List<ShipController> queue = new List<ShipController> ();

	public int Count {
		get {
			return queue.Count;
		}
	}

	public void EnqueueShip(ShipController ship) {
		queue.Add (ship);
	}

	public void RemoveShip(ShipController ship) {
		queue.Remove (ship);
	}

	public void ChangePriority(ShipController ship, int priority) {
		queue.Remove(ship);
		if (priority >= queue.Count + 1) {
			queue.Add (ship);
		} else {
			queue.Insert (priority - 1, ship);
		}
	}

	public int GetPriority(ShipController ship) {
		return queue.IndexOf(ship) + 1;
	}

	public ShipController GetShipWithPriority(int priority) {
		return queue[priority];
	}

	public void SwapPriority(int priority, int otherPriority) {
		ShipController temp = queue [priority - 1];
		queue [priority - 1] = queue [otherPriority - 1];
		queue [otherPriority - 1] = temp;
	}

	public void Clear() {
		queue.Clear ();
	}

	public IEnumerator GetEnumerator() {
		return queue.GetEnumerator ();
	}

	public void Propose(PriorityQueue queue) {
		this.queue = queue.queue;
	}
}
