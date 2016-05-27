using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class Timer : NetworkBehaviour {

	public DateTime gameStartTime;
	public VistaLightsLogger logger;

	public int SpeedOne;
	public int SpeedTwo;
	public int SpeedThree;

	private DateTime virtualTime = new DateTime(2015, 10, 10, 10, 10, 10);
	private TimeSpan timeElapsed = new TimeSpan(0, 0, 0);
	private double previousTime;
	private double speed;

	private TimeWidgetView view;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		previousTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (view == null) {
			view = GameObject.Find ("TimeWidget").GetComponent<TimeWidgetView> ();
			return;
		}
		double currentTime = Time.time;

		double virtualTimeAdvance = (currentTime - previousTime) * speed;
		virtualTime = virtualTime.AddSeconds(virtualTimeAdvance);
		timeElapsed = TimeSpan.FromSeconds(virtualTimeAdvance);

		previousTime = currentTime;
	}

	public DateTime VirtualTime {
		get { return virtualTime; }
		set { virtualTime = value; }
	}

	public TimeSpan TimeElapsed {
		get { return timeElapsed; }
	}

	public double Speed {
		get {
			return speed;
		}
		set {
			speed = value;
		}
	}

	private void LoggedSpeedChange(int newSpeed) {
		logger.LogTimer (newSpeed);
		Speed = newSpeed;

	}

	[Command]
	public void CmdPause() {
		LoggedSpeedChange(0);
		view.RpcPause ();
	}

	[Command]
	public void CmdSpeedOne() {
		LoggedSpeedChange (SpeedOne);
		view.RpcSpeedOne ();
	}

	[Command]
	public void CmdSpeedTwo() {
		LoggedSpeedChange (SpeedTwo);
		view.RpcSpeedTwo ();
	}

	[Command]
	public void CmdSpeedThree() {
		LoggedSpeedChange (SpeedThree);
		view.RpcSpeedThree ();
	}
}
