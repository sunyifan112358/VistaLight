using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimization : MonoBehaviour {

	public PriorityQueue queue;
	public NetworkScheduler networkScheduler;
	public int limit;

	public List<ShipController> ships;
	public PriorityQueue newQueue;

	public void Optimize() {
		foreach (var ship in queue.queue) {
			ships.Add(ship);
		}

		networkScheduler.ClearAllSchedule();

		newQueue = new PriorityQueue();
		for (int priority = 0; priority < limit; priority++) {
			double maxWeight = -1;
			ShipController selectedShip = null;
			foreach (ShipController ship in ships) {
				double weight = this.CaculateWeight(ship);
				if (weight > maxWeight) {
					maxWeight = weight;
					selectedShip = ship;
				}
			}

			newQueue.EnqueueShip(selectedShip);
			ships.Remove(selectedShip);
		}

		networkScheduler.ClearAllSchedule();
	}

	private double CaculateWeight(ShipController ship) {
		ShipScheduler shipScheduler = new ShipScheduler();
		shipScheduler.Ship = ship;

		shipScheduler.Schedule();
		var schdule = ship.schedule;

		


		return 0;
	}
}
