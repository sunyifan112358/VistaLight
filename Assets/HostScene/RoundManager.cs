using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public enum GamePhase{ 
	Simulation, Decision
}

public class RoundManager : MonoBehaviour {

	public GamePhase phase = GamePhase.Simulation;

	public GameObject SubmitButton;
	public TimeWidgetController timeWidgetController;
	public Timer timer;
	public ShipListController shipListController;
	public NetworkScheduler networkScheduler;
	public VistaLightsLogger logger;
	public SceneSetting sceneSetting;
	public RecommendationSystem recommendataionSystem;
	public NotificationSystem notificationSystem;
	public MapLoader mapLoader;
	public MapController mapController;
	public IntroductionWindowController introductionWindowController;


	public Toggle burningToggle;
	public Toggle dispersantToggle;
	public Toggle skimmerToggle;
	public OilSpillingAction oilCleaningAction;

	public DateTime SimulationPhaseStartTime;
	public TimeSpan DecisionInterval = new TimeSpan(6, 0, 0);

	public DateTime DecisionPhaseStartTime;
	public TimeSpan DecisionTimeLimit = new TimeSpan(0, 2, 0);

	// Use this for initialization
	void Start () {
		mapLoader.LoadMap ();
		StartSimulationPhase ();

		if (!sceneSetting.inTutorial) {
			ShowIntroductionWindow ();
		}

	}

	void Awake() {
		logger = GameObject.Find("BasicLoggerManager").GetComponent<VistaLightsLogger>();	
		sceneSetting = GameObject.Find ("SceneSetting").GetComponent<SceneSetting> ();

		logger.StartRun ("run");
	}

	private void ShowIntroductionWindow() {
		introductionWindowController.gameObject.SetActive (true);
		introductionWindowController.UpdateText ();
		timeWidgetController.PauseGame ();
	}

	public void CloseIntroductionWindow() {
		introductionWindowController.gameObject.SetActive (false);

		StartSimulationPhase ();
	}
	
	// Update is called once per frame
	void Update () {
		// if (sceneSetting.inTutorial) {
		// 	return;
		// }

		if (networkScheduler.Scheduling){
			return;
		}

		if (phase == GamePhase.Simulation) {
			DateTime currentVirtualTime = timer.VirtualTime;
			if (currentVirtualTime >= SimulationPhaseStartTime + DecisionInterval) {
				StartDecisionPhase ();
			}
		} else if(phase == GamePhase.Decision) {
			DateTime currentDecisionTime = DateTime.Now;
			if (currentDecisionTime >= DecisionPhaseStartTime + DecisionTimeLimit) {
				TimeUp ();
			}
		}
	}

	public void StartDecisionPhase() {
		Debug.Log("Start Decision Phase");
		PriorityQueue priorityQueue = GameObject.Find("PriorityQueue").GetComponent<PriorityQueue>();
		foreach (var ship in priorityQueue.queue) {
			double weight;
			double maintainess = 0.000002;
			double overdue = 0.00005;
			double cargo = ship.Ship.cargo;
			double value = ship.Ship.value;
			double duration =  6 * 60 * 60;
			Timer timer = GameObject.Find("Timer").GetComponent<Timer>();
			var currTime = timer.VirtualTime;
			weight = cargo * duration * maintainess + cargo * value;
			if (currTime.CompareTo(ship.Ship.dueTime) > 0) {
				weight += cargo * duration * overdue;
			}
			Debug.Log(cargo);
			Debug.Log(value);
			Debug.Log(weight);
		}

		timeWidgetController.PauseGame ();

		recommendataionSystem.EnableRecommendationButton ();

		SubmitButton.SetActive (true);
		shipListController.ShowNewPriority ();

		DecisionPhaseStartTime = DateTime.Now;
		phase = GamePhase.Decision;

		logger.LogPhaseChange (GamePhase.Decision);
	}

	public void StartSimulationPhase() {
		timeWidgetController.SetSpeedOne ();

		recommendataionSystem.DisableRecommendationButton ();

		SubmitButton.SetActive (false);
		shipListController.HideNewPriority ();

		SimulationPhaseStartTime = timer.VirtualTime;
		phase = GamePhase.Simulation;

		logger.LogPhaseChange (GamePhase.Simulation);
	}

	public void SubmitAndContinueButtonClickHandler() {
		if (GameObject.Find ("SceneSetting").GetComponent<SceneSetting> ().GiveRecommendation) {
			if (!recommendataionSystem.recommendationRequested ||
			   !recommendataionSystem.isAllRecommendationsProcessed ()) {
				notificationSystem.Notify (NotificationType.Warning, 
					"Please request your recommendations and process them before submit.");
				return;
			}
		}
		SubmitAndContinue ();
	}



	public void SubmitAndContinue() {
        for (int x = 0; x < recommendataionSystem.recommendations.Count; x++) {
            if (!recommendataionSystem.recommendations[x].accepted && recommendataionSystem.recommendations[x].isActiveAndEnabled) {
                recommendataionSystem.denyCount++;
            }
            if (recommendataionSystem.recommendations[x].accepted)
            {
                recommendataionSystem.denyCount = 0;
            }
        }
        if (recommendataionSystem.denyCount >= 10)
        {
            recommendataionSystem.denyCount = 0;
            notificationSystem.Notify(NotificationType.Warning, "You've been ignoring a lot of reccomendations, maybe try listening to a few to raise profits.");
        }
        recommendataionSystem.ClearRecommendation ();

		Submit ();
		StartSimulationPhase ();
	}

	private void Submit() {
		networkScheduler.RequestReschedule ();

		if (burningToggle.isOn) {
			burningToggle.isOn = false;
			oilCleaningAction.Burn ();
		} else if (dispersantToggle.isOn) {
			dispersantToggle.isOn = false;
			oilCleaningAction.Dispersant ();
		} else if (skimmerToggle.isOn) {
			skimmerToggle.isOn = false;
			oilCleaningAction.Skimmers ();
		}

		logger.LogSubmitButton ();
	}

	private void TimeUp() {
		SubmitAndContinue ();
	}
}
