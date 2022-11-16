using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DestinyBlade
{
    public class PausePanelUI : MonoBehaviour
    {
        [SerializeField] private GameUI _gui;

        public void OnResumeButton()
        {
            Time.timeScale = 1;

            gameObject.SetActive(false);

            _gui.isPaused = false;
        }

        public void OnRestartButton()
        {
            Time.timeScale = 1;

            gameObject.SetActive(false);

            _gui.isPaused = false;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnQuitButton()
        {
            Application.Quit();
        }
    }
}