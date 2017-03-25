using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class VistaLightsLogger : MonoBehaviour {

	private bool inRun = false;

	// Use this for initialization
	void Start () {

	}

	void OnDestroy() {

	}

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
		DestoryIfInstanceExist ();
	}

	private static VistaLightsLogger instance = null;
	void DestoryIfInstanceExist() {
		if (instance != null) {
			Destroy (gameObject); 
			return;
		}
		instance = this;
	}

	// Update is called once per frame
	void Update () {
		if (inRun) {
			LogKeyStroke ();
		}
    }

	private void AddTimeInformation(JSONClass details) {
		Timer timer = GameObject.Find ("Timer").GetComponent<Timer> ();
		details ["current_time"] = timer.VirtualTime.ToString ();

	}

	public void LogTimer(double speed) {
		JSONClass details = new JSONClass();
		AddTimeInformation (details);
		details["timer"] = speed.ToString();
		TheLogger.instance.TakeAction(1, details);
	}

	public void LogKeyStroke() {
		foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode))) {
			if (Input.GetKeyDown(kcode)) {
				JSONClass details = new JSONClass();
				AddTimeInformation (details);
				details["keystroke"] = kcode.ToString();
				details["mouse_x"] = Input.mousePosition.x.ToString();
				details["mouse_y"] = Input.mousePosition.y.ToString();
				TheLogger.instance.TakeAction(1, details);
			}
		}
	}

	public void LogMouseMove() {
		float mouse_move_x = Input.GetAxis("Mouse X");
		float mouse_move_y = Input.GetAxis("Mouse Y");
		if (mouse_move_x != 0 || mouse_move_y != 0) {
			JSONClass details = new JSONClass();
			AddTimeInformation (details);
			details["mouse_x"] = Input.mousePosition.x.ToString();
			details["mouse_y"] = Input.mousePosition.y.ToString();
			TheLogger.instance.TakeAction(1, details);
		}
	}

	public void LogChangeShipPriority(Ship ship, int new_priority) {
        LogAllShipsStates();
        JSONClass details = new JSONClass();
		AddTimeInformation (details);
		details["ship_id"] = ship.shipID.ToString();
		details["new_priority"] = new_priority.ToString();
		TheLogger.instance.TakeAction(1, details);
	}

	public void LogGameOver(double money, double welfare) {
        LogAllShipsStates();
        JSONClass details = new JSONClass();
		AddTimeInformation (details);
		details["budget"] = money.ToString();
		details["welfare"] = welfare.ToString();
		TheLogger.instance.TakeAction(1, details);
	}

	public void LogOilCleaning(OilSpillSolution solution) {
		JSONClass details = new JSONClass();
		AddTimeInformation (details);
		details ["solution"] = solution.ToString ();
		TheLogger.instance.TakeAction(1, details);
	}

	public void LogOilSpilling(OilSpillingEvent oilSpillingEvent) {
        LogAllShipsStates();
        JSONClass details = new JSONClass();
		AddTimeInformation (details);
		details ["map_event"] = "Oil Spilling";
		details ["x"] = oilSpillingEvent.X.ToString ();
		details ["y"] = oilSpillingEvent.Y.ToString ();
		details ["radius"] = oilSpillingEvent.Radius.ToString();
		details ["amount"] = oilSpillingEvent.Amount.ToString();

		TheLogger.instance.TakeAction(1, details);
	}

	public void LogShipGeneration(ShipGenerationEvent shipGenerationEvent) {
        LogAllShipsStates();
        JSONClass details = new JSONClass();
		AddTimeInformation (details);
		details ["map_event"] = "Ship Generation";
		details ["x"] = shipGenerationEvent.X.ToString ();
		details ["y"] = shipGenerationEvent.Y.ToString ();

		details ["ship_id"] = shipGenerationEvent.Ship.shipID.ToString ();
		details ["name"] = shipGenerationEvent.Ship.Name;
		details ["industry"] = shipGenerationEvent.Ship.Industry.ToString();
		details ["cargo"] = shipGenerationEvent.Ship.cargo.ToString();
		details ["value"] = shipGenerationEvent.Ship.value.ToString();
		details ["due_time"] = shipGenerationEvent.Ship.dueTime.ToString ();

		TheLogger.instance.TakeAction(1, details);
	}

	public void LogRedGreenSignal(Ship ship, string signal) {
        LogAllShipsStates();
        JSONClass details = new JSONClass();
		AddTimeInformation (details);
		details ["ship_id"] = ship.shipID.ToString();
		details ["signal"] = signal;
		TheLogger.instance.TakeAction(1, details);
	}

	public void LogPhaseChange(GamePhase phase) {
        LogAllShipsStates();
        JSONClass details = new JSONClass();
		AddTimeInformation (details);
		details ["phase"] = phase.ToString();
		TheLogger.instance.TakeAction(1, details);
	}

	public void LogSubmitButton() {
		JSONClass details = new JSONClass();
		AddTimeInformation (details);
		details ["submit"] = "submit";
		TheLogger.instance.TakeAction(1, details);
	}

	public void LogRecommendationAction(bool isAccepted, Recommendation recommendation) {
        LogAllShipsStates();
        JSONClass details = new JSONClass();
		AddTimeInformation (details);
		details ["isAccepted"] = isAccepted.ToString();
		details ["ship"] = recommendation.ship.Ship.shipID.ToString();
		details ["priority"] = recommendation.desiredPriority.ToString();
		if (recommendation.justification != null) {
			details ["justification"] = recommendation.justification.ToString ();
		}
		TheLogger.instance.TakeAction(1, details);	
	}

	public void StartRun(string challengeName) {
		JSONClass details = new JSONClass ();
		AddTimeInformation (details);
		SceneSetting sceneSetting = GameObject.Find ("SceneSetting").GetComponent<SceneSetting> ();
		details ["map"] = sceneSetting.MapName;
		details ["give_recommendation"] = sceneSetting.GiveRecommendation.ToString ();
		details ["with_justification"] = sceneSetting.RecommendWithJustification.ToString ();
		TheLogger.instance.BeginRun(challengeName, details);
		inRun = true;
	}

	public void EndRun(double money, double welfare, double dockUtilization) {
        LogAllShipsStates();
		JSONClass details = new JSONClass ();
		AddTimeInformation (details);
		details["budget"] = money.ToString();
		details["welfare"] = welfare.ToString();
		details["dock_utilization"] = dockUtilization.ToString();
		TheLogger.instance.EndRun(details);
		inRun = false;
	}

    public void LogAllShipsStates() {
        ShipController[] ships = FindObjectsOfType<ShipController>();
        JSONClass details = new JSONClass();
        AddTimeInformation(details);
        details["log_event"] = "Log All Ship States";
        for (int x = 0; x < ships.Length; x++)
        {
            details["x " + x] = ships[x].Ship.X.ToString();
            details["y " + x] = ships[x].Ship.Y.ToString();

            details["ship_id " + x] = ships[x].Ship.shipID.ToString();
            details["name " + x] = ships[x].Ship.Name;
            details["status " + x] = ships[x].status.ToString();
            details["industry " + x] = ships[x].Ship.Industry.ToString();
            details["cargo " + x] = ships[x].Ship.cargo.ToString();
            details["value " + x] = ships[x].Ship.value.ToString();
            details["due_time " + x] = ships[x].Ship.dueTime.ToString();
        }
        TheLogger.instance.TakeAction(1, details);
    }
}

