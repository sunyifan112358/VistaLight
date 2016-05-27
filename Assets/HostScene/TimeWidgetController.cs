using UnityEngine;
using UnityEngine.Networking;

public class TimeWidgetController : NetworkBehaviour {

	public Timer timer;

	public RoundManager roundManager;
	public NotificationSystem notificationSystem;

	[ClientCallback]
	public void PauseGameButtonClickHandler(){
		if (roundManager.phase == GamePhase.Simulation) {
			notificationSystem.Notify (NotificationType.Warning, 
				"Cannot Pause the game during the simulation phase");
			return;
		}
		timer.CmdPause ();
	}

	public void SpeedOneButtonClickHandler() {
		if (roundManager.phase == GamePhase.Decision) {
			notificationSystem.Notify (NotificationType.Warning, 
				"Please use the \"Submit and Continue\" button to submit your decision and enter simulation phase");
			return;
		}
		timer.CmdSpeedOne ();
	}

	public void SpeedTwoButtonClickHandler() {
		if (roundManager.phase == GamePhase.Decision) {
			notificationSystem.Notify (NotificationType.Warning, 
				"Please use the \"Submit and Continue\" button to submit your decision and enter simulation phase");
			return;
		}
		timer.CmdSpeedTwo ();
	}

	public void SpeedThreeButtonClickHandler() {
		if (roundManager.phase == GamePhase.Decision) {
			notificationSystem.Notify (NotificationType.Warning, 
				"Please use the \"Submit and Continue\" button to submit your decision and enter simulation phase");
			return;
		}
		timer.CmdSpeedThree ();
	}
}
