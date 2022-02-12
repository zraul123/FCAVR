using Assets.Scripts.Configuration.Menus;
using Assets.Scripts.Factories;
using InputSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

namespace Assets.Scripts.View.Menus
{
    class SelectScaleDiagramsMenu : DoubleMenu
    {
        private SelectScaleDiagramsMenuConfiguration Configuration;
        private ScrollViewItemFactory ItemFactory;
        private EventManager EventManager;

        private IList<GameObject> NotSelectedDiagramButtons = new List<GameObject>();
        private IList<GameObject> SelectedDiagramButtons= new List<GameObject>();
        private ScrollContainerSizer NotSelectedDiagramsScrollViewSizer;
        private ScrollContainerSizer SelectedDiagramsScrollViewSizer;

        private IList<string> _notSelectedDiagrams = new List<string>();
        private IList<string> _selectedDiagrams = new List<string>();


        public SelectScaleDiagramsMenu(SelectScaleDiagramsMenuConfiguration configuration) : base(configuration)
        {
            Configuration = configuration;
            EventManager = Scripts.Configuration.Configuration.Instance.EventManager;
            NotSelectedDiagramsScrollViewSizer = Configuration.NotSelectedDiagramsContent.GetComponent<ScrollContainerSizer>();
            SelectedDiagramsScrollViewSizer = Configuration.SelectedDiagramsContent.GetComponent<ScrollContainerSizer>();
            ItemFactory = new ScrollViewItemFactory();

            RegisterActionEvent(Configuration.StartButton, StartButton_Action);
            RegisterNavigationEvents();
        }

        #region Registering Events

        protected override void RegisterActionEvents()
        {
            foreach (var buttonObject in NotSelectedDiagramButtons)
            {
                RegisterActionEventWithGameObjectParameters(buttonObject, SwitchToSelected);
            }
            foreach (var buttonObject in SelectedDiagramButtons)
            {
                RegisterActionEventWithGameObjectParameters(buttonObject, SwitchToNotSelected);
            }
        }

        protected override void RegisterNavigationEvents()
        {
            RegisterNavigationEvent(Configuration.BackButton);
        }

        #endregion

        #region Exposed functions

        public void Initialize(IList<string> diagramNames)
        {
            _notSelectedDiagrams = diagramNames;
            ComputeDiagramItems();
        }

        #endregion

        #region Helpers

        private void SwitchToSelected(GameObject button)
        {
            var diagramName = button.GetComponentInChildren<Text>().text;

            _notSelectedDiagrams.Remove(diagramName);
            _selectedDiagrams.Add(diagramName);
            ComputeDiagramItems();
        }

        private void SwitchToNotSelected(GameObject button)
        {
            var diagramName = button.GetComponentInChildren<Text>().text;

            _notSelectedDiagrams.Add(diagramName);
            _selectedDiagrams.Remove(diagramName);
            ComputeDiagramItems();
        }

        private void ComputeDiagramItems()
        {
            PopulateNotSelectedDiagrams(_notSelectedDiagrams);
            PopulateSelectedDiagrams(_selectedDiagrams);
            RegisterActionEvents();
        }

        private void PopulateNotSelectedDiagrams(IList<string> diagramNames)
        {
            NotSelectedDiagramButtons.ForEach(GameObject.Destroy);
            NotSelectedDiagramButtons = CreateDiagramItems(Configuration.NotSelectedDiagramsContent, NotSelectedDiagramsScrollViewSizer, diagramNames);
        }

        private void PopulateSelectedDiagrams(IList<string> diagramNames)
        {
            SelectedDiagramButtons.ForEach(GameObject.Destroy);
            SelectedDiagramButtons = CreateDiagramItems(Configuration.SelectedDiagramsContent, SelectedDiagramsScrollViewSizer, diagramNames);
        }

        private IList<GameObject> CreateDiagramItems(Transform diagramContentTransform, ScrollContainerSizer sizer, IList<string> diagramNames)
        {
            sizer.ComputeRect(diagramNames.Count);
            return ItemFactory.CreateList(diagramContentTransform, diagramNames);
        }

        #endregion

        #region Actions

        private void StartButton_Action()
        {
            if (_selectedDiagrams.Count == 0)
                return;

            EventManager.StartScalaScenario(_selectedDiagrams);
        }

        #endregion
    }
}
