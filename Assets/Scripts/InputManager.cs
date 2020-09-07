using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    UI_Manager uiManager;
	
    CommandType commandType = CommandType.Open;

    void Update()
    {
        if (Input.touchCount > 0 && Time.timeScale != 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
                GameMaster.Instance.SendCommandToGrid(touch.position, commandType);
        }
	
#if UNITY_EDITOR
	if(Input.GetMouseButtonDown(0) && Time.timeScale != 0)
		GameMaster.Instance.SendCommandToGrid(Input.mousePosition, commandType);
#endif

	if(Input.GetKeyDown(KeyCode.Escape))
	{
	    uiManager.PauseAndResume(!GameMaster.Instance.isPaused);
	}
    }

    public void ChangeCommandType(int command)
    {
        commandType = (CommandType)command;
    }
}
