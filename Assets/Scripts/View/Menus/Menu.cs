using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.View.Menus
{
    abstract class Menu
    {
        protected MenuController MenuSwitcher = Configuration.Configuration.Instance.MenuSwitcher;

        #region Overrides

        protected abstract void RegisterNavigationEvents();
        protected abstract void RegisterActionEvents();

        #endregion

        #region Register Events
        protected void RegisterNavigationEvent(GameObject buttonObject)
        {
            buttonObject.GetComponent<Button>().onClick.AddListener(delegate { NavigatorButtonPressed(buttonObject); });
        }

        protected void RegisterActionEvent(GameObject button, UnityAction action)
        {
            button.GetComponent<Button>().onClick.AddListener(action);
        }

        protected void RegisterActionEventWithGameObjectParameters(GameObject button, UnityAction<GameObject> action)
        {
            button.GetComponent<Button>().onClick.AddListener(delegate { action(button); });
        }

        #endregion

        #region Events

        private void NavigatorButtonPressed(GameObject buttonGameobject)
        {
            var newState = buttonGameobject.GetComponent<ButtonNavigation>().NextState;
            MenuSwitcher.SwitchMenuState(newState);
        }

        #endregion

        #region Exposed functions



        #endregion
    }
}
