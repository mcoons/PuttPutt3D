using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject titleText;
    public GameObject settingsButton;
    public GameObject instructionsButton;
    public GameObject settingsPanel;
    public GameObject instructionsPanel;

    public EventSystem eventSystem;


    public void OnSettingsClick()
    {
        if (GameManager.Instance.gameState != GameManager.State.Idle)
        {
            return;
        }
        titleText.SetActive(false);
        settingsButton.SetActive(false);
        instructionsButton.SetActive(false);

        settingsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(settingsPanel.transform.Find("Exit Button").gameObject);
        GameManager.Instance.gameState = GameManager.State.Menu;
    }

    public void OnSettingsExit()
    {
        titleText.SetActive(true);
        settingsButton.SetActive(true);
        instructionsButton.SetActive(true);

        settingsPanel.SetActive(false);
        //GameManager.Instance.gameState = GameManager.State.Idle;
        StartCoroutine("SetIdle");
    }

    public void OnInstructionsClick()
    {
        if (GameManager.Instance.gameState != GameManager.State.Idle)
        {
            return;
        }
        titleText.SetActive(false);
        settingsButton.SetActive(false);
        instructionsButton.SetActive(false);

        instructionsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(instructionsPanel.transform.Find("Exit Button").gameObject);
        GameManager.Instance.gameState = GameManager.State.Menu;

    }
    public void OnInstructionsExit()
    {
        titleText.SetActive(true);
        settingsButton.SetActive(true);
        instructionsButton.SetActive(true);

        instructionsPanel.SetActive(false);
        //GameManager.Instance.gameState = GameManager.State.Idle;
        StartCoroutine("SetIdle");

    }

    // Wait one frame to clear Space/Return button press
    IEnumerator SetIdle()
    {
        yield return null;
        GameManager.Instance.gameState = GameManager.State.Idle;
    }

}
