using UnityEngine;
using UnityEngine.UI;

namespace DestinyBlade
{ 
public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField] private Fighter _player;
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _staminaBar;

        private void Start()
        {
            gameObject.SetActive(true);
        }

        private void Update()
        {
            _healthBar.fillAmount = Mathf.InverseLerp(0f, _player.MaxHitPoints, _player.CurrentHitPoints);
            _staminaBar.fillAmount = Mathf.InverseLerp(0f, _player.MaxStamina, _player.CurrentStamina);
        }

        public void SetPlayer(Fighter player)
        {
            _player = player;
        }
    }
}