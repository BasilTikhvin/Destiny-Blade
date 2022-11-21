using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace DestinyBlade
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Fighter _player;
        [SerializeField] private Fighter _playerPrefab;
        [SerializeField] private CameraController _camera;
        [SerializeField] private MovementController _movementController;
        [SerializeField] private GameUI _statsUI;
        [SerializeField] private LevelPassArea _levelPassArea;

        private void Start()
        {
            _player.EventOnDeath.AddListener(OnDeath);
        }

        private void OnDeath()
        {
            _player.EventOnDeath.RemoveListener(OnDeath);

            _player = Instantiate(_playerPrefab);

            _camera.SetTarget(_player.transform);
            _movementController.SetPlayer(_player);
            _statsUI.SetPlayer(_player);
            _levelPassArea.SetPlayer(_player);

            _player.EventOnDeath.AddListener(OnDeath);
        }
    }
}