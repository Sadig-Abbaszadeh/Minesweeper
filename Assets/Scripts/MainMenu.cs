using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    #region Variables
    int mode; // bckg color mode
    bool darkMode;

    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    GameObject settingsMenu;
    [SerializeField]
    GameObject infoMenu;
    [SerializeField]
    GameObject exitPermissionMenu;

    [SerializeField]
    Slider heightSlider;
    [SerializeField]
    Slider widthSlider;
    [SerializeField]
    Slider bombSlider;

    [SerializeField]
    Text heightText;
    [SerializeField]
    Text widthText;
    [SerializeField]
    Text bombText;
    [SerializeField]
    GameObject darkCheck;

    [SerializeField]
    Camera cam;
    #endregion

    // std cam bckg color: 859DC3

    void Awake()
    {
        // set frame rate
        Application.targetFrameRate = 30;
        
        // camera adjustment
        int sampleWidth = 480, sampleHeight = 800;

        // set background mode
        darkMode = PlayerPrefs.GetInt("ScreenMode") == 1 ? true : false;
        SetBackgroundColor();

        StaticData.camSize = (5f * sampleWidth * Camera.main.pixelHeight) / (sampleHeight * Camera.main.pixelWidth);
        StaticData.allowableHeight = Mathf.FloorToInt((StaticData.camSize * 2 - 2) / 0.4f);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (infoMenu.activeSelf)
            {
                infoMenu.SetActive(false);
                mainMenu.SetActive(true);
            }

            else if (settingsMenu.activeSelf)
                QuitSettingsMenu();

            else if (exitPermissionMenu.activeSelf)
                DisapproveExit();

            else
                Exit();
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        heightSlider.maxValue = StaticData.allowableHeight;

        // set last values on text/slider. Also clamp to not break the game lol
        heightSlider.value = Mathf.Clamp(PlayerPrefs.GetInt("lastHeightValue"), heightSlider.minValue, heightSlider.maxValue);
        widthSlider.value = Mathf.Clamp(PlayerPrefs.GetInt("lastWidthValue"), widthSlider.minValue, widthSlider.maxValue);
        bombSlider.value = Mathf.Clamp(PlayerPrefs.GetInt("lastBombValue"), bombSlider.minValue, bombSlider.maxValue);

        heightText.text = "" + heightSlider.value;
        widthText.text = "" + widthSlider.value;
        bombText.text = bombSlider.value + " %";

        darkCheck.SetActive(darkMode);

        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Help()
    {
        mainMenu.SetActive(false);
        infoMenu.SetActive(true);
    }

    public void Exit()
    {
        mainMenu.SetActive(false);
        exitPermissionMenu.SetActive(true);
    }

    public void ApproveExit()
    {
        Application.Quit();
        Debug.Log("quit");
    }

    public void DisapproveExit()
    {
        exitPermissionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    void SetBackgroundColor()
    {
        cam.backgroundColor = darkMode ? StaticData.DarkColor : StaticData.NormalColor;
    }

    void OnApplicationQuit()
    {
        if (settingsMenu.activeSelf)
            SaveSettings();
    }

    #region GameSettings
    public void HeightSetting()
    {
        heightText.text = "" + heightSlider.value;
    }

    public void WidthSetting()
    {
        widthText.text = "" + widthSlider.value;
    }

    public void BombSetting()
    {
        bombText.text = bombSlider.value + " %";
    }

    public void DarkModeButton()
    {
        darkMode = !darkMode;
        SetBackgroundColor();
        darkCheck.SetActive(darkMode);
    }

    void QuitSettingsMenu()
    {
        SaveSettings();
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt("lastHeightValue", (int)heightSlider.value);
        PlayerPrefs.SetInt("lastWidthValue", (int)widthSlider.value);
        PlayerPrefs.SetInt("lastBombValue", (int)bombSlider.value);
        PlayerPrefs.SetInt("ScreenMode", darkMode ? 1 : 0);
    }
    #endregion
}
