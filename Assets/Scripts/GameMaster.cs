using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    #region Fields
    public static GameMaster Instance;

    [SerializeField]
    UI_Manager uiManager;
    [SerializeField]
    Sprite[] sprites;
    [SerializeField, Space(10)]
    Vector2 gridCenter;

    int width, height, cellsToOpen, bombCount;
    float cellSize = .4f;

    Camera cam;
    GridManager gridManager;

    public bool isPaused { get; private set; }
    public bool isGameOver { get; private set; }
    #endregion

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        cam = Camera.main;

        cam.orthographicSize = StaticData.camSize;
        cam.backgroundColor = PlayerPrefs.GetInt("ScreenMode") == 1 ? StaticData.DarkColor : StaticData.NormalColor;

        // clamp stuff to not break the game if the save file was modified
        int width = Mathf.Clamp(PlayerPrefs.GetInt("lastWidthValue"), StaticData.minWidthAndHeight, StaticData.allowableWidth);
        int height = Mathf.Clamp(PlayerPrefs.GetInt("lastHeightValue"), StaticData.minWidthAndHeight, StaticData.allowableHeight);
        float bombPercent = Mathf.Clamp(PlayerPrefs.GetInt("lastBombValue"), StaticData.minBombPercent, StaticData.maxBombPercent) / 100f;

        bombCount = (int)(width * height * bombPercent);
	UpdateBombCount(0);
        cellsToOpen = width * height - bombCount;

        PlayerPrefs.SetInt("lastWidthValue", width);
        PlayerPrefs.SetInt("lastHeightValue", height);
        PlayerPrefs.SetInt("lastBombValue", (int)(bombPercent * 100));

        Vector2 origin = gridCenter - new Vector2(width - 1, height - 1) * cellSize / 2f;

        gridManager = new GridManager(width, height, cellSize, origin, sprites, bombCount);

	uiManager.UpdateBombCount(bombCount);
    }

    public void CheckGameWin(bool lastCellHasBomb)
    {
        cellsToOpen--;

        if (cellsToOpen <= 0)
            GameOver(!lastCellHasBomb);     
    }

    public void SendCommandToGrid(Vector2 pointerPos, CommandType commandType)
    {
        gridManager.ReceiveCommand(cam.ScreenToWorldPoint(pointerPos), commandType);
    }

    public void PauseAndResume(bool pause)
    {
        isPaused = pause;

	int timeScale = pause ? 0 : 1;
	Time.timeScale = timeScale;
	Time.fixedDeltaTime = timeScale; // just in case
    }

    public void LeaveScene(bool quitToMenu)
    {
	Time.timeScale = 1;
	
	if(quitToMenu)
	    SceneManager.LoadScene(0);
	else
	    SceneManager.LoadScene(1);
    }

    public void UpdateBombCount(int change)
    {
	bombCount += change;
	uiManager.UpdateBombCount(bombCount);
    }

    public void GameOver(bool playerHasWon)
    {
	Time.timeScale = 0;
        uiManager.GameOver(playerHasWon);
    }
}