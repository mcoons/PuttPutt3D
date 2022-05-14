using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    public GameObject powerObject;
    public GameObject puttButton;
    public GameObject leftButton;
    public GameObject rightButton;

    public EventSystem eventSystem;
    public string result;

    public Hashtable results = new Hashtable()
    {
        {"-4", "Condor"},
        {"-3", "Albatross"},
        {"-2", "Eagle"},
        {"-1", "Birdie"},
        {"0", "Par"},
        {"1", "Bogie"},
        {"2", "Double Bogie"},
        {"3", "Triple Bogie"},
        {"4", "Quadruple Bogie"}
    };

    #region Unity Callbacks

    private void Start()
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

    protected void OnDestroy()
    {
        EventManager.Instance.OnGameStateChange.RemoveListener(HandleHole);
        EventManager.Instance.OnPowerBarSizeChange.RemoveListener(HandlePowerBarSizeChange);
        StopAllCoroutines();
    }

    #endregion

    private void HandleHole(GameManager.State newState)
    {
        if ( newState == GameManager.State.Hole)
        {
            Debug.Log("UIManager: " +
            //GameManager.Instance.result[(GetStrokes() - GetPar()).ToString()]);
            results[(GetStrokes() - GetPar()).ToString()]);

            StartCoroutine("ShowScoreAndNext");
        }
    }

    private int GetPar()
    {
        return GameManager.Instance.scores[GameManager.Instance.currentGreenIndex].par;
    }

    private int GetStrokes()
    {
        return GameManager.Instance.scores[GameManager.Instance.currentGreenIndex].strokes;
    }

    private IEnumerator ShowScoreAndNext()
    {
        //result = (string)GameManager.Instance.result[(GetStrokes() - GetPar()).ToString()];
        result = (string)results[(GetStrokes() - GetPar()).ToString()];

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

    private void HandlePowerBarSizeChange(Vector2 newSize)
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
        EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Idle);
        //StartCoroutine("SetIdleNextFrame");  // Apply next frame so Space for putt is not triggered
    }

    public void OnInstructionsClick()
    {
        instructionsPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OnInstructionsExit()
    {
        SetMainItems(true);

        instructionsPanel.SetActive(false);
        EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Idle);
        //StartCoroutine("SetIdleNextFrame"); // Apply next frame so Space for putt is not triggered
    }

    public void OnQuit()
    {
        Debug.Log("Quitting App");
        Application.Quit();
    }

    public void OnRestart()
    {
        Debug.Log("Restarting App");
        gameOverPanel.SetActive(false);
        settingsPanel.SetActive(false);
        instructionsPanel.SetActive(true);
        SetMainItems(false);

        EventManager.Instance.OnGameRestart.Invoke();
    }

    // Wait one frame to clear Space/Return key press
    //private IEnumerator SetIdleNextFrame()
    //{
    //    yield return null;
    //    EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Idle);
    //}

    private void SetMainItems(bool state)
    {
        titleText.SetActive(state);
        settingsButton.SetActive(state);
        parText.SetActive(state);
        strokeText.SetActive(state);
        powerObject.SetActive(state);
        puttButton.SetActive(state);
        leftButton.SetActive(state);
        rightButton.SetActive(state);
    }

}
