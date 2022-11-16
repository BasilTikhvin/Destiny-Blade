using UnityEngine;
using UnityEngine.SceneManagement;

namespace DestinyBlade
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelPassArea _levelPassArea;
        [SerializeField] private float _timeOffset;

        private int _currentScene;

        private void Start()
        {
            _currentScene = SceneManager.GetActiveScene().buildIndex;
        }

        private void Update()
        {
            OnPass();
        }

        private void OnPass()
        {
            if (_levelPassArea.LevelPassed == true)
            {
                _timeOffset -= Time.deltaTime;

                if (_timeOffset < 0f)
                {
                    SceneManager.LoadScene(_currentScene + 1);
                }
            }
        }
    }
}