using UnityEngine;
using UnityEngine.UI;

namespace DestinyBlade
{ 
public class GameUI : MonoBehaviour
    {
        [SerializeField] private Fighter _player;
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _staminaBar;
        [SerializeField] private GameObject pausePanel;

        public bool isPaused { get; set; }

        private void Start()
        {
            gameObject.SetActive(true);

            pausePanel.gameObject.SetActive(false);
        }

        private void Update()
        {
            _healthBar.fillAmount = Mathf.InverseLerp(0f, _player.MaxHitPoints, _player.CurrentHitPoints);
            _staminaBar.fillAmount = Mathf.InverseLerp(0f, _player.MaxStamina, _player.CurrentStamina);

            if (Input.GetKeyDown(KeyCode.Escape) == true && isPaused == false)
            {
                Time.timeScale = 0;

                pausePanel.gameObject.SetActive(true);

                isPaused = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) == true && isPaused == true)
            {
                Time.timeScale = 1;

                pausePanel.gameObject.SetActive(false);

                isPaused = false;
            }
        }

        public void SetPlayer(Fighter player)
        {
            _player = player;
        }
    }
}