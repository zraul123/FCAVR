using Assets.Scripts.Scenario.Context;
using Assets.Scripts.Scenario.Lifetrack;
using Assets.Scripts.Scenario.Scala;
using Assets.Scripts.UI.MainMenu;
using FCA.Repository;
using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Assets.Scripts
{
    public class Controller : MonoBehaviour
    {
        MainMenu MainMenu { get; set; }
        Scenario.Scenario _scenario;

        /*

        #region Initialization
        public Controller()
        {
            MainMenu = new MainMenu(Configuration.Configuration.Instance.MainMenuConfiguration);

            InitializeEventHandlers();
            InitializeMainMenuActions();
        }

        private void InitializeEventHandlers()
        {
            MainMenu.MenuItemSelected += MainMenu_MenuItemSelected;
        }

        private void InitializeMainMenuActions()
        {
            _mainMenuActions = new Dictionary<MainMenuOption, Action<MainMenuSelectionEventArgs>>();

            _mainMenuActions.Add(MainMenuOption.ContextStart, StartContext);
            _mainMenuActions.Add(MainMenuOption.ScaleConfigure, HandleScale);
            _mainMenuActions.Add(MainMenuOption.LifetrackConfigure, HandleLifetrack);
            _mainMenuActions.Add(MainMenuOption.ScaleStart, StartScale);
            _mainMenuActions.Add(MainMenuOption.LifetrackStart, StartLifetrack);
        }
        #endregion

        #region Main Menu Event Handlers

        private void MainMenu_MenuItemSelected(object sender, MainMenuSelectionEventArgs selectionEventArgs)
        {
            _mainMenuActions[selectionEventArgs.Option](selectionEventArgs);
        }

        #endregion

        #region Main Menu Actions

        private void StartLifetrack(MainMenuSelectionEventArgs eventArgs)
        {
            var startupConfiguration = eventArgs.StartupConfiguration as LifetracksScenarioStartupConfiguration;
            var scenario = _scenario as LifetracksScenario;

            scenario.BuildLifetrack(startupConfiguration);
        }

        private void HandleLifetrack(MainMenuSelectionEventArgs eventArgs)
        {
            var scenario = new LifetracksScenario(eventArgs.Name);
            //MainMenu.ActivateDiagramSelection(scenario.Diagrams);

            _scenario = scenario;
        }

        private void StartScale(MainMenuSelectionEventArgs eventArgs)
        {
            var startupConfiguration = eventArgs.StartupConfiguration as ScalaScenarioStartupConfiguration;
            var scenario = _scenario as ScalaScenario;

            startupConfiguration.Diagrams.ForEach(diagram => scenario.addDiagram(diagram));
        }

        private void HandleScale(MainMenuSelectionEventArgs eventArgs)
        {
            var scenario = new ScalaScenario(eventArgs.Name);
            //MainMenu.ActivateDiagramSelection(scenario.Diagrams);

            _scenario = scenario;
        }

        private void StartContext(MainMenuSelectionEventArgs eventArgs)
        {
            var startupConfiguration = eventArgs.StartupConfiguration as ContextScenarioStartupConfiguration;
            _scenario = new ContextScenario(startupConfiguration);
        }

        #endregion
        */
    }
}
