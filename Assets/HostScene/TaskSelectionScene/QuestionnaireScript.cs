using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionnaireScript : MonoBehaviour {

    public InputField ageField, ethnicityField;
    public Dropdown genderField, educationField, employmentField;
    public Text error;
    public VistaLightsLogger logger;
    public ChallengeSelector cs;
    // Use this for initialization
    void Start () {
        logger = GameObject.Find("BasicLoggerManager").GetComponent<VistaLightsLogger>();
        cs = GameObject.Find("ChallengeSelector").GetComponent<ChallengeSelector>();
        if (cs.questionnaireFilled) {
            gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (cs.questionnaireFilled)
        {
            gameObject.SetActive(false);
        }
    }

    public void finishQuestionnaire() {
        if (ageField.text != "" && ethnicityField.text != "")
        {
            logger.LogDemographicInfo(ageField.text, ethnicityField.text, genderField.captionText.text, educationField.captionText.text, employmentField.captionText.text);
            cs.questionnaireFilled = true;
        }
        else {
            error.gameObject.SetActive(true);
        }
    }
}
