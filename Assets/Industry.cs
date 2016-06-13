using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class Industry : NetworkBehaviour
{
	public IndustryManager IndustryManager;
	public NetworkScheduler NetworkScheduler;

	[SyncVar]
	public IndustryType playerIndustry;
	[SyncVar]
	bool playerIndustrySet = false;
	public GameObject SubmitButton;
	PriorityQueue queueProposal;

	void Start ()
	{
	}

	void Update ()
	{
		if (connectionToServer == null || !connectionToServer.isConnected) {
			return;
		}
		if (isLocalPlayer && !playerIndustrySet) {
			connectionToServer.Disconnect ();
		}
	}

	public void AssignIndustry (IndustryType industry)
	{
		playerIndustry = industry;
		playerIndustrySet = true;

		SubmitButton.GetComponent<Button> ().onClick.AddListener (delegate () {
			this.ProposeQueue ();
		});
	}

	public void MakeProposer ()
	{
		SubmitButton.SetActive (true);
		queueProposal = NetworkScheduler.priorityQueue;
	}

	[ClientCallback]
	public void ProposeQueue ()
	{
		SubmitButton.SetActive (false);
		IndustryManager.CmdPropose (queueProposal);
	}
}
