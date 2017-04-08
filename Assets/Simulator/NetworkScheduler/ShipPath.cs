using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipPath {
	public List<Node> nodes = new List<Node>();

	public bool IsNodeOnPath(Node node) {
		return nodes.Contains(node);
	}

	public void AppendNode(Node node) {
		nodes.Add(node);
	}

	public void AddNodeFromBeginning(Node node) {
		nodes.Insert(0, node);
	}

	public ShipPath ConcatenatePath(ShipPath path) {
		ShipPath newPath = new ShipPath();
		foreach (Node node in this.nodes) {
			newPath.AppendNode(node);
		}
		foreach (Node node in path.nodes) {
			newPath.AppendNode(node);
		}
		return newPath;
	}
}
