using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {

    [SerializeField, Tooltip("Pause Menu Canvas")]
    private GameObject m_pauseMenu = null;
    [SerializeField, Tooltip("Original menu button")]
    private GameObject m_menuButton = null;
    // Is game paused?
    private bool m_isPaused = false;
    
    void Start()
    {
        m_pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        m_isPaused = true;
        m_pauseMenu.SetActive(true);
        m_menuButton.SetActive(false);
    }

    /// <summary>
    /// Unpause the game
    /// </summary>
    public void UnPauseGame()
    {
        m_isPaused = false;
        m_pauseMenu.SetActive(false);
        m_menuButton.SetActive(true);
    }

    /// <summary>
    /// Is the game paused?
    /// </summary>
    /// <returns>Check if the game is paused</returns>
    public bool IsGamePaused()
    {
        return m_isPaused;
    }

    /// <summary>
    /// Return the main menu
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
