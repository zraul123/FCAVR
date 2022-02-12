using Assets.Scripts.Configuration.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View.Menus
{
    abstract class DoubleMenu : Menu
    {
        private GameObject _currentLocalStateText;
        private GameObject _disabledLocalStateText;
        private Transform _currentLocalState;
        private Transform _disabledLocalState;


        public DoubleMenu(DoubleMenuConfiguration configuration)
        {
            _currentLocalStateText = configuration.PrimaryLocalMenuText;
            _disabledLocalStateText = configuration.SecondaryLocalMenuText;

            _currentLocalState = configuration.PrimaryLocalMenuTransform;
            _disabledLocalState = configuration.SecondaryLocalMenuTransform;
        }

        public void SwitchLocalMenu()
        {
            _currentLocalState.gameObject.SetActive(false);

            SwitchTextPositions();
            SwitchTextColors();
            SwapTextReferences();
            SwapMenuReferences();

            _currentLocalState.gameObject.SetActive(true);
        }

        #region Helpers

        private void SwapMenuReferences()
        {
            var activeState = _currentLocalState;
            _currentLocalState = _disabledLocalState;
            _disabledLocalState = activeState;
        }

        private void SwitchTextPositions()
        {
            var activePosition = _currentLocalStateText.transform.localPosition;
            _currentLocalStateText.transform.localPosition = _disabledLocalStateText.transform.localPosition;
            _disabledLocalStateText.transform.localPosition = activePosition;
        }

        private void SwitchTextColors()
        {
            var activeColor = _currentLocalStateText.GetComponentInChildren<Text>().color;
            _currentLocalStateText.GetComponentInChildren<Text>().color = _disabledLocalStateText.GetComponentInChildren<Text>().color;
            _disabledLocalStateText.GetComponentInChildren<Text>().color = activeColor;
        }

        private void SwapTextReferences()
        {
            var toBeDisabledState = _currentLocalStateText;
            _currentLocalStateText = _disabledLocalStateText;
            _disabledLocalStateText = toBeDisabledState;
        }

        #endregion
    }
}
