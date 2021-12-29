using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
    }
}
