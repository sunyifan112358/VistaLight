using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TimeWidgetView : NetworkBehaviour {

	public GameObject PauseRedBorder;

	public Image PauseButtonImage;
	public Image Speed1ButtonImage;
	public Image Speed2ButtonImage;
	public Image Speed3ButtonImage;

	public Sprite SelectedImage;
	public Sprite UnselectedImage;

	[ClientRpc]
	public void RpcPause() {
		SelectButton (PauseButtonImage);
		PauseRedBorder.SetActive (true);
	}

	[ClientRpc]
	public void RpcSpeedOne() {
		SelectButton (Speed1ButtonImage);
		PauseRedBorder.SetActive (false);
	}

	[ClientRpc]
	public void RpcSpeedTwo() {
		SelectButton (Speed2ButtonImage);
		PauseRedBorder.SetActive (false);
	}

	[ClientRpc]
	public void RpcSpeedThree() {
		SelectButton (Speed3ButtonImage);
		PauseRedBorder.SetActive (false);
	}

	private void DeselectAllButtons() {
		PauseButtonImage.sprite = UnselectedImage;
		Speed1ButtonImage.sprite = UnselectedImage;
		Speed2ButtonImage.sprite = UnselectedImage;
		Speed3ButtonImage.sprite = UnselectedImage;
	}

	private void SelectButton(Image Button) {
		DeselectAllButtons ();
		Button.sprite = SelectedImage;
	}
}
