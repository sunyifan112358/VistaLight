using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionnaireScript : MonoBehaviour {

    public InputField ageField;
    public Dropdown ethnicityField, raceField, genderField, educationField, employmentField;
    public Text error;
    public VistaLightsLogger logger;
    public ChallengeSelector cs;
    public GameObject infoText;
    public Toggle ageDisclose;
    // Use this for initialization
    void Start () {
        infoText = GameObject.Find("Information Message");
        logger = GameObject.Find("BasicLoggerManager").GetComponent<VistaLightsLogger>();
        cs = GameObject.Find("ChallengeSelector").GetComponent<ChallengeSelector>();
        if (cs.questionnaireFilled) {
            gameObject.SetActive(false);
            infoText.SetActive(false);
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
        bool isAllFilled = (!ethnicityField.captionText.text.Contains("<")) && (!raceField.captionText.text.Contains("<")) && (!genderField.captionText.text.Contains("<")) && (!educationField.captionText.text.Contains("<")) && (!employmentField.captionText.text.Contains("<"));
        if ((ageDisclose.isOn || ageField.text != "") && isAllFilled)
        {
            string agelog = "";
            if (ageDisclose.isOn) {
                agelog = "Prefer not to disclose";
            }
            else
            {
                agelog = ageField.text;
            }
            logger.LogDemographicInfo(agelog, raceField.captionText.text, ethnicityField.captionText.text, genderField.captionText.text, educationField.captionText.text, employmentField.captionText.text);
            cs.questionnaireFilled = true;
        }
        else {
            error.gameObject.SetActive(true);
        }
    }
    public void skipQuestionnaire()
    {
        logger.LogDemographicInfo("N/A", "N/A", "N/A", "N/A", "N/A", "N/A");
        cs.questionnaireFilled = true;
    }
    public void doQuestionnaire()
    {
        infoText.SetActive(false);
    }
}
