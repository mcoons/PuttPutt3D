using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject titleText;
    public GameObject parText;
    public GameObject strokeText;
    public GameObject settingsButton;
    public GameObject instructionsButton;
    public GameObject settingsPanel;
    public GameObject instructionsPanel;
    public GameObject resultText;
    public RectTransform powerBar;

    public EventSystem eventSystem;
    public string result;

    #region Unity callbacks

        void Start()
        {
        EventManager.Instance.OnGameStateChange.AddListener(HandleWin);
        EventManager.Instance.OnPowerBarSizeChange.AddListener(HandlePowerBarSizeChange);

        eventSystem.SetSelectedGameObject(instructionsPanel.transform.Find("Exit Button").gameObject);
        }

        private void Update()
        {
            parText.GetComponent<TextMeshProUGUI>().text = "Par: " + GameManager.Instance.par.ToString();
            strokeText.GetComponent<TextMeshProUGUI>().text = "Stroke: " +GameManager.Instance.stroke.ToString();
        }

    #endregion

    void HandleWin(GameManager.State newState)
    {
        if ( newState == GameManager.State.Win)
        {
            Debug.Log("UIManager: " + GameManager.Instance.result[(GameManager.Instance.stroke - GameManager.Instance.par).ToString()]);

            StartCoroutine("ShowWinAndNext");


        }
    }

    IEnumerator ShowWinAndNext()
    {
        result = (string)GameManager.Instance.result[(GameManager.Instance.stroke - GameManager.Instance.par).ToString()];

        if (result == null || result == "")
        {
            result = "Wow, impressive!";
        }

        resultText.GetComponent<TextMeshProUGUI>().text = result;
        resultText.SetActive(true);
        SetMainItems(false);

        yield return new WaitForSeconds(5);

        resultText.SetActive(false);
        SetMainItems(true);
        powerBar.sizeDelta = new Vector2(30, 0);
        EventManager.Instance.OnNextGreen.Invoke();

    }

    public void HandlePowerBarSizeChange(Vector2 newSize)
    {
        powerBar.sizeDelta = newSize;
    }

    public void OnSettingsClick()
    {
        if (GameManager.Instance.gameState != GameManager.State.Idle)
        {
            return;
        }

        SetMainItems(false);

        settingsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(settingsPanel.transform.Find("Exit Button").gameObject);
        //GameManager.Instance.gameState = GameManager.State.Menu;
        EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Menu);

    }

    public void OnSettingsExit()
    {

        SetMainItems(true);

        settingsPanel.SetActive(false);
        StartCoroutine("SetIdleNextFrame");  // Apply next frame so Space for putt is not triggered
    }

    public void OnInstructionsClick()
    {
        if (GameManager.Instance.gameState != GameManager.State.Idle)
        {
            return;
        }

        SetMainItems(false);

        instructionsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(instructionsPanel.transform.Find("Exit Button").gameObject);
        //GameManager.Instance.gameState = GameManager.State.Menu;
        EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Menu);

    }
    public void OnInstructionsExit()
    {

        SetMainItems(true);

        instructionsPanel.SetActive(false);
        StartCoroutine("SetIdleNextFrame"); // Apply next frame so Space for putt is not triggered

    }

    // Wait one frame to clear Space/Return key press
    IEnumerator SetIdleNextFrame()
    {
        yield return null;
        //GameManager.Instance.gameState = GameManager.State.Idle;
        EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Idle);

    }

    void SetMainItems(bool state)
    {
        titleText.SetActive(state);
        settingsButton.SetActive(state);
        instructionsButton.SetActive(state);
        parText.SetActive(state);
        strokeText.SetActive(state);
    }

}
