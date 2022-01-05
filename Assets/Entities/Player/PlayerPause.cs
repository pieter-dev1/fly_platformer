using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject teleporterBaseUi;
    private List<GameObject> activeTpButtons = new List<GameObject>();

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
        print(activeTpButtons.Count);
        if (activeTpButtons.Any())
        {
            teleporterBaseUi.SetActive(enabled);
            activeTpButtons.ForEach(x => x.SetActive(enabled));
        }
    }

    public void AddTpButton(GameObject btn)
    {
        activeTpButtons.Add(btn);
    }
}
