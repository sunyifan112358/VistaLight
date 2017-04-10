using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ChallengeSelector : MonoBehaviour {
	public SceneSetting sceneSetting;
    public bool randRecommendations;
    public bool randFeedback;
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
        int x = Random.Range(0, 100);
        if (x < 33) {
            randRecommendations = false;
        }
        else
        {
            randRecommendations = true;
            int y = Random.Range(0, 100);
            if (y < 50)
            {
                randFeedback = false;
            }
            else
            {
                randFeedback = true;
            }
        }
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
	
	}

	public void SelectTutorial() {	
		sceneSetting.MapName = "houston_game_0";
        sceneSetting.GiveRecommendation = randRecommendations;
        sceneSetting.GiveFeedback = randFeedback;
        sceneSetting.inTutorial = true;
        PlayerPrefs.SetString("tutorialplayed", "true");
        StartGame ();
	}

	public void SelectChallenge1() {
		sceneSetting.MapName = "houston_game_1";
		sceneSetting.GiveRecommendation = randRecommendations;
        sceneSetting.GiveFeedback = randFeedback;
        sceneSetting.inTutorial = false;
        PlayerPrefs.SetString("challenge1Played", "true");
		StartGame ();
	}

	public void SelectChallenge2() {
		sceneSetting.MapName = "houston_game_2";
		sceneSetting.GiveRecommendation = randRecommendations;
        sceneSetting.GiveFeedback = randFeedback;
        //sceneSetting.RecommendWithJustification = false;
        sceneSetting.inTutorial = false;
        PlayerPrefs.SetString("challenge2Played", "true");
		StartGame ();
	}

	public void SelectChallenge3() {
		sceneSetting.MapName = "houston_game_3";
		sceneSetting.GiveRecommendation = randRecommendations;
        sceneSetting.GiveFeedback = randFeedback;
        //sceneSetting.RecommendWithJustification = true;
        sceneSetting.inTutorial = false;
        PlayerPrefs.SetString("challenge3Played", "true");
		StartGame ();
	}

	private void StartGame() {
		SceneManager.LoadScene("HostGameScene", LoadSceneMode.Single);
	}
}
