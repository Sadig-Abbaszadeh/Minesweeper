using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

    #region Fields
    [SerializeField]
    InputManager inputManager;
    [SerializeField]
    GameObject pauseScreen, winScreen, lossScreen, finalViewTogglePanel;
    [SerializeField]
    Button[] commandButtons; // open, flag, suspect
    [SerializeField]
    TextMeshProUGUI timeText, finalTimeText, bombCountText;

    GameObject prevScreen;

    int prevButton = 0;

    float time = 0;
    #endregion
  
    void Start()
    {
	commandButtons[0].interactable = false;
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        timeText.text = (int)(time / 60) + " : " + (int)time % 60;
    }

    public void ChangeCommand(int newButton)
    {
        commandButtons[prevButton].interactable = true;
	commandButtons[newButton].interactable = false;
	
	inputManager.ChangeCommandType(newButton);
	prevButton = newButton;
    }
    
    public void PauseAndResume(bool pause)
    {
	pauseScreen.SetActive(pause);
	GameMaster.Instance.PauseAndResume(pause);
    }

    public void LeaveAndRestart(bool leave)
    {
	GameMaster.Instance.LeaveScene(leave);
    }

    public void GameOver(bool playerHasWon)
    {
	if(playerHasWon)
	{
	    winScreen.SetActive(true);
	    finalTimeText.text = timeText.text;
	    prevScreen = winScreen;
	}
	else
	{
	    lossScreen.SetActive(true);
	    prevScreen = lossScreen;
	}
    }

    public void ShowHideFinalScreen(bool showScreen)
    {
	finalViewTogglePanel.SetActive(!showScreen);
	prevScreen.SetActive(showScreen);
    }

    public void UpdateBombCount(int count)
    {
	bombCountText.text = ": " + count;
    }
}
