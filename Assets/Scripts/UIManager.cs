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
    public GameObject gameOverPanel;
    public GameObject instructionsPanel;
    public GameObject resultText;
    public GameObject gameOverText;
    public RectTransform powerBar;

    public EventSystem eventSystem;
    public string result;

    #region Unity callbacks

        void Start()
        {
            EventManager.Instance.OnGameStateChange.AddListener(HandleHole);
            EventManager.Instance.OnPowerBarSizeChange.AddListener(HandlePowerBarSizeChange);

            eventSystem.SetSelectedGameObject(instructionsPanel.transform.Find("Exit Button").gameObject);
        }

        private void Update()
        {
            parText.GetComponent<TextMeshProUGUI>().text = "Par: " + GetPar().ToString();
            strokeText.GetComponent<TextMeshProUGUI>().text = "Stroke: " + GetStrokes().ToString();
        }

    #endregion

    void HandleHole(GameManager.State newState)
    {
        if ( newState == GameManager.State.Hole)
        {
            Debug.Log("UIManager: " +
                GameManager.Instance.result[ (GetStrokes() - GetPar()).ToString()]);

            StartCoroutine("ShowScoreAndNext");
        }
    }

    int GetPar()
    {
        return GameManager.Instance.scores[GameManager.Instance.currentGreenIndex].par;
    }

    int GetStrokes()
    {
        return GameManager.Instance.scores[GameManager.Instance.currentGreenIndex].strokes;
    }

    IEnumerator ShowScoreAndNext()
    {
        result = (string)GameManager.Instance.result[(GetStrokes() - GetPar()).ToString()];

        if (result == null || result == "")
        {
            result = "Wow, impressive!";
        }

        resultText.GetComponent<TextMeshProUGUI>().text = result;
        resultText.SetActive(true);
        SetMainItems(false);

        yield return new WaitForSeconds(5);

        resultText.SetActive(false);

        if (GameManager.Instance.currentGreenIndex == GameManager.Instance.greens.Length -1 )
        {
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.GameOver);
            string gOText = "";
            gOText += "Hole\t\tPar\tStrokes\tResult\n";

            foreach (GameManager.Score scoreCard in GameManager.Instance.scores)
            {
                gOText += scoreCard.description + "\t" + scoreCard.par.ToString() + "\t" + scoreCard.strokes.ToString() + "\n";
            }
            gameOverText.GetComponent<TextMeshProUGUI>().text = gOText;
            gameOverPanel.SetActive(true);

        }
        else
        {
            SetMainItems(true);
            powerBar.sizeDelta = new Vector2(30, 0);
            EventManager.Instance.OnNextGreen.Invoke();
        }

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
