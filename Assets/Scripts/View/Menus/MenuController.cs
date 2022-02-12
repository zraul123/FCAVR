using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.View.Menus
{
    class MenuController : MonoBehaviour
    {
        private GameObject _currentState;

        private Dictionary<GameObject, DoubleMenu> DoubleMenuMapping = new Dictionary<GameObject, DoubleMenu>();

        void Start()
        {
            InitializePlayerPreferences();
            InitializeMenu();
        }

        public void RegisterMenu(GameObject gameObject, DoubleMenu menu)
        {
            DoubleMenuMapping.Add(gameObject, menu);
        }

        public void ToggleMenuVisibility(bool shouldShow)
        {
            _currentState.SetActive(shouldShow);
        }

        public bool ToggleLocalState()
        {
            if (!DoubleMenuMapping.ContainsKey(_currentState))
                return false;
            DoubleMenuMapping[_currentState].SwitchLocalMenu();
            return true;
        }

        private void InitializePlayerPreferences()
        {
            if (!PlayerPrefs.HasKey("tutorial"))
                PlayerPrefs.SetString("tutorial", "yes");

            var tutorial = PlayerPrefs.GetString("tutorial");
            if (string.IsNullOrEmpty(tutorial) || !tutorial.Equals("yes") || tutorial.Equals("no"))
                PlayerPrefs.SetString("tutorial", "yes");

            PlayerPrefs.Save();
        }

        private void InitializeMenu()
        {
            _currentState = PlayerPrefs.GetString("tutorial").Equals("yes") ? 
                Configuration.Configuration.Instance.TutorialMenuTransform.gameObject :
                Configuration.Configuration.Instance.MainMenuTransform;

            _currentState.SetActive(true);
        }

        public void SwitchMenuState(Transform newStateTransform)
        {
            SwitchMenuState(newStateTransform.gameObject);
        }

        public void SwitchMenuState(GameObject newState)
        {
            _currentState.SetActive(false);
            _currentState = newState;
            _currentState.SetActive(true);
        }
    }
}
