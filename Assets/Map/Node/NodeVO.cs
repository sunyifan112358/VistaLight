﻿using UnityEngine;
using System.Collections;
using System;

public class NodeVO : MonoBehaviour, MapSelectableVO {

	public Node node;
    public GameObject sidePanel;

	public void Start () {
	}
	
	public void Update () {
		if (node != null) {
			gameObject.transform.position = new Vector3((float)node.X, (float)node.Y, (float)MapController.MapZIndex);

			if (node.IsExit) {
				gameObject.transform.Find("Exit").gameObject.SetActive(true);
			}
		}
	}

	public void OnMouseDrag() {
		SceneSetting sceneSetting = GameObject.Find("SceneSetting").GetComponent<SceneSetting>();
		if(sceneSetting.AllowMapEditing) {
			RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (ray.point.x == 0 && ray.point.y == 0) return;
			node.X = ray.point.x;
			node.Y = ray.point.y;
			gameObject.transform.position = new Vector3((float)node.X, (float)node.Y, (float)MapController.MapZIndex);
		}
	}

	public void Select() {
		gameObject.transform.Find("NodeDot").gameObject.SetActive(false);
		gameObject.transform.Find("NodeDotSelected").gameObject.SetActive(true);
	}

	public void Deselect() {
		gameObject.transform.Find("NodeDot").gameObject.SetActive(true);
		gameObject.transform.Find("NodeDotSelected").gameObject.SetActive(false);
	}

    public GameObject GetSidePanel()
    {
		GameObject sidePanel = GameObject.Find("SidePanels").transform.Find("NodeInformationSidePanel").gameObject;
		NodeInformationSidePanelController controller = sidePanel.GetComponent<NodeInformationSidePanelController>();
		controller.node= node;
		controller.UpdateDisplay();
		return sidePanel;
	}
}
