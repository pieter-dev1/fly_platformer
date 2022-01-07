using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject teleporterBaseUi;
    private List<(GameObject btn, bool checkpointReached)> activeTpButtons = new List<(GameObject btn, bool checkpointReached)>();

    private Color skipColor = new Color(1, 0.97f, 0.63f);

    private void Start()
    {

    }

    public void Pause()
    {
        Time.timeScale = 0;
        TogglePauseMenu(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        TogglePauseMenu(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TogglePauseMenu(bool enabled)
    {
        pauseMenu.SetActive(enabled);
        if (activeTpButtons.Any())
        {
            teleporterBaseUi.SetActive(enabled);
            activeTpButtons.ForEach(x => {
                if (!x.checkpointReached)
                    x.btn.GetComponent<Image>().color = skipColor;
                else
                    x.btn.GetComponent<Image>().color = Color.white;
                x.btn.SetActive(enabled);
                }
            );
        }
    }

    public void AddTpButton(GameObject btn, bool checkpointReached = true)
    {
        var foundElementIndex = activeTpButtons.FindIndex(x => x.btn == btn);
        if (foundElementIndex >= 0)
        {
            activeTpButtons[foundElementIndex] = (btn, checkpointReached);
        }
        else
            activeTpButtons.Add((btn, checkpointReached));
    }

    public List<(GameObject btn, bool checkpointReached)> GetTpButtons()
    {
        return activeTpButtons;
    }
}
