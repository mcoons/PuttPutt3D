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
    

    public EventSystem eventSystem;

    void Start()
    {
        eventSystem.SetSelectedGameObject(instructionsPanel.transform.Find("Exit Button").gameObject);
    }

    private void Update()
    {
        parText.GetComponent<TextMeshProUGUI>().text = "Par: " + GameManager.Instance.par.ToString();
        strokeText.GetComponent<TextMeshProUGUI>().text = "Stroke: " +GameManager.Instance.stroke.ToString();
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

    // Wait one frame to clear Space/Return button press
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
