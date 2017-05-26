using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ChallengeSelector : MonoBehaviour {
	public SceneSetting sceneSetting;
    public bool toggleRecommendations;
    public bool toggleFeedback;
    public bool tutorialPlayed;
    public bool challenge1Played;
    public bool challenge2Played;
    public bool challenge3Played;

	public VistaLightsLogger logger;

	void Awake() {
        tutorialPlayed = "true" == PlayerPrefs.GetString("tutorialplayed", "false");
        challenge1Played = "true" == PlayerPrefs.GetString("challenge1played", "false");
        challenge2Played = "true" == PlayerPrefs.GetString("challenge2played", "false");
        challenge3Played = "true" == PlayerPrefs.GetString("challenge3played", "false");
        DontDestroyOnLoad(transform.gameObject);
		DestoryIfInstanceExist ();
        logger = GameObject.Find ("BasicLoggerManager").GetComponent<VistaLightsLogger> ();
	}

	private static ChallengeSelector instance = null;
	void DestoryIfInstanceExist() {
		if (instance != null) {
			Destroy (gameObject); 
			return;
		}
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!GameObject.Find("ToggleReccomendations").GetComponent<Toggle>().isOn) {
            GameObject.Find("ToggleFeedback").GetComponent<Toggle>().isOn = false;
        }
    }

	public void SelectTutorial() {	
		sceneSetting.MapName = "houston_game_0";
        sceneSetting.GiveRecommendation = GameObject.Find("ToggleReccomendations").GetComponent<Toggle>().isOn;
        sceneSetting.GiveFeedback = GameObject.Find("ToggleFeedback").GetComponent<Toggle>().isOn;
        sceneSetting.inTutorial = true;
        PlayerPrefs.SetString("tutorialplayed", "true");
        StartGame ();
	}

	public void SelectChallenge1() {
		sceneSetting.MapName = "houston_game_1";
		sceneSetting.GiveRecommendation = GameObject.Find("ToggleReccomendations").GetComponent<Toggle>().isOn;
        sceneSetting.GiveFeedback = GameObject.Find("ToggleFeedback").GetComponent<Toggle>().isOn; ;
        sceneSetting.inTutorial = false;
        PlayerPrefs.SetString("challenge1Played", "true");
		StartGame ();
	}

	public void SelectChallenge2() {
		sceneSetting.MapName = "houston_game_2";
		sceneSetting.GiveRecommendation = GameObject.Find("ToggleReccomendations").GetComponent<Toggle>().isOn;
        sceneSetting.GiveFeedback = GameObject.Find("ToggleFeedback").GetComponent<Toggle>().isOn; ;
        //sceneSetting.RecommendWithJustification = false;
        sceneSetting.inTutorial = false;
        PlayerPrefs.SetString("challenge2Played", "true");
		StartGame ();
	}

	public void SelectChallenge3() {
		sceneSetting.MapName = "houston_game_3";
		sceneSetting.GiveRecommendation = GameObject.Find("ToggleReccomendations").GetComponent<Toggle>().isOn;
        sceneSetting.GiveFeedback = GameObject.Find("ToggleFeedback").GetComponent<Toggle>().isOn; 
        //sceneSetting.RecommendWithJustification = true;
        sceneSetting.inTutorial = false;
        PlayerPrefs.SetString("challenge3Played", "true");
		StartGame ();
	}

	private void StartGame() {
		SceneManager.LoadScene("HostGameScene", LoadSceneMode.Single);
	}
}
