using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class IndustryManager : NetworkBehaviour {
	public NetworkScheduler networkScheduler;
	public RoundManager roundManager;

	public List<IndustryType> freeIndustries = new List<IndustryType> ();
	List<Industry> assignedIndustries = new List<Industry> ();
	int nextProposer;

	[Command]
	public void CmdRequestIndustry(GameObject playerObject) {
		if (freeIndustries.Count == 0) {
			return;
		}
		IndustryType chosenIndustry = freeIndustries [UnityEngine.Random.Range (0, freeIndustries.Count)];
		Industry clientHandle = playerObject.GetComponent<Industry> ();
		assignedIndustries.Add (clientHandle);
		clientHandle.AssignIndustry (chosenIndustry);

		freeIndustries.Remove (chosenIndustry);
	}

	[Server]
	public void SendProposalRequest() {
		if (nextProposer >= assignedIndustries.Count) {
			if (assignedIndustries.Count == 0) {
				throw new InvalidOperationException ("There is nobody to issue a proposal.");
			}
			nextProposer = (nextProposer + 1) % assignedIndustries.Count;
			SendProposalRequest ();
		} else {
			assignedIndustries [nextProposer].MakeProposer ();
		}
	}

	[Command]
	public void CmdPropose(PriorityQueue queue) {
		networkScheduler.priorityQueue.Propose (queue);
		roundManager.SubmitAndContinue ();
		nextProposer++;
	}
}
