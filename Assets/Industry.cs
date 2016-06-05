using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class Industry : NetworkBehaviour {
	IndustryManager industryManager;
	NetworkScheduler networkScheduler;

	[SyncVar]
	public IndustryType playerIndustry;
	[SyncVar]
	bool playerIndustrySet = false;
	public GameObject SubmitButton;
	PriorityQueue queueProposal;

	void Start() {
		SubmitButton = GameObject.Find ("SubmitButton");
		SubmitButton.GetComponent<Button> ().onClick.AddListener(delegate () {
			this.ProposeQueue ();
		});
		SubmitButton.SetActive (false);
	}

	public override void OnStartLocalPlayer() {
		industryManager = GameObject.Find ("IndustryManager").GetComponent<IndustryManager> ();
		networkScheduler = GameObject.Find ("NetworkScheduler").GetComponent<NetworkScheduler> ();

		industryManager.CmdRequestIndustry (gameObject);
	}

	void Update() {
		if (!playerIndustrySet) {
			connectionToClient.Disconnect ();
		}
	}

	public void AssignIndustry(IndustryType industry) {
		playerIndustry = industry;
		playerIndustrySet = true;
	}

	public void MakeProposer() {
		SubmitButton.SetActive (true);
		queueProposal = networkScheduler.priorityQueue;
	}

	[ClientCallback]
	public void ProposeQueue () {
		SubmitButton.SetActive (false);
		industryManager.CmdPropose (queueProposal);
	}
}
