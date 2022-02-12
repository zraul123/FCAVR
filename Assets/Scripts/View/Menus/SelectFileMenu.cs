using Assets.Scripts.Configuration.Menus;
using Assets.Scripts.Factories;
using InputSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

namespace Assets.Scripts.View.Menus
{
    class SelectFileMenu : Menu
    {
        private SelectFileMenuConfiguration Configuration;
        private ScrollViewItemFactory ItemFactory;
        private EventManager EventManager;
        private ScrollContainerSizer Sizer;
        private UnityAction<GameObject> CurrentAction = null;

        private IList<GameObject> containedButtons = new List<GameObject>();

        public SelectFileMenu(SelectFileMenuConfiguration configuration)
        {
            Configuration = configuration;
            EventManager = Scripts.Configuration.Configuration.Instance.EventManager;
            Sizer = Configuration.SelectFileSizer;
            ItemFactory = new ScrollViewItemFactory();

            RegisterNavigationEvents();
        }

        #region Exposed functions

        public void InitializeContext(IList<string> filenames)
        {
            CurrentAction = ContextSelectedEvent;
            PopulateScrollview(filenames);
        }

        public void InitializeScale(IList<string> filenames)
        {
            CurrentAction = ScaleSelectedEvent;
            PopulateScrollview(filenames);
        }

        public void InitializeLifetracks(IList<string> filenames)
        {
            CurrentAction = LifetracksSelectedEvent;
            PopulateScrollview(filenames);
        }

        #endregion

        #region Helpers

        private void PopulateScrollview(IList<string> filenames)
        {
            ClearCurrentButtons();
            Sizer.ComputeRect(filenames.Count + 1);
            containedButtons = ItemFactory.CreateList(Configuration.ScrollViewContentTransform, filenames);
            SetBackButtonPosition(filenames.Count);
            RegisterActionEvents();
        }

        private void ClearCurrentButtons()
        {
            containedButtons.ForEach(Object.Destroy);
        }

        private void SetBackButtonPosition(int count)
        {
            var position = Configuration.BackButton.transform.localPosition;
            position.y = ItemFactory.GetYPositionFromCount(count);

            Configuration.BackButton.transform.localPosition = position;
        }

        #endregion

        #region Event Registering

        protected override void RegisterActionEvents()
        {
            foreach(var buttonObject in containedButtons)
            {
                RegisterActionEventWithGameObjectParameters(buttonObject, CurrentAction);
            }
        }

        protected override void RegisterNavigationEvents()
        {
            RegisterNavigationEvent(Configuration.BackButton);
        }

        #endregion

        #region Actions

        private void ContextSelectedEvent(GameObject button)
        {
            var scenarioFilename = button.GetComponentInChildren<Text>().text;
            EventManager.StartContextScenario(scenarioFilename);
        }

        private void ScaleSelectedEvent(GameObject button)
        {
            var scenarioFilename = button.GetComponentInChildren<Text>().text;
            EventManager.SelectFileMenu_ScaleContextSelected(scenarioFilename);
            MenuSwitcher.SwitchMenuState(Configuration.SelectScaleDiagramsMenuTransform);
        }

        private void LifetracksSelectedEvent(GameObject button)
        {
            var scenarioFilename = button.GetComponentInChildren<Text>().text;
            EventManager.SelectFileMenu_ScaleContextSelected(scenarioFilename);
        }

        #endregion

    }
}
