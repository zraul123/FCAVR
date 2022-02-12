using Assets.Scripts.Configuration;
using Assets.Scripts.Configuration.Menus;
using Assets.Scripts.Input;
using Assets.Scripts.Input.Actions;
using Assets.Scripts.Input.Pointers;
using Assets.Scripts.Scenario;
using Assets.Scripts.Scenario.Context;
using Assets.Scripts.Scenario.Lifetrack;
using Assets.Scripts.Scenario.Scala;
using Assets.Scripts.UI.MainMenu;
using Assets.Scripts.View.Menus;
using FCA.Filesystem;
using FCA.Repository;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

namespace InputSystem
{
    public class EventManager : MonoBehaviour
    {
        Configuration Configuration { get; set; }
        Scenario Scenario { get; set; }
        MenuController MenuController { get; set; }
        Pivot Pivot { get; set; }

        public InputMode CurrentMode;

        [Header("Pointers")]

        public SteamVR_LaserPointer LaserPointer;

        public GameObject CanvasPointer;

        private MainMenu MainMenu { get; set; }
        private SelectLatticeTypeMenu SelectLatticeMenu { get; set; }
        private SelectFileMenu LatticesMenu { get; set; }
        private TutorialMenu TutorialMenu { get; set; }
        private SelectScaleDiagramsMenu SelectScaleDiagramsMenu { get; set; }

        #region Initialization

        void Start()
        {
            Configuration = Configuration.Instance;
            MenuController = Configuration.Instance.MenuSwitcher;
            Pivot = Configuration.Instance.Pivot;

            InitializeMenus();
            RegisterMenus();
        }

        private void InitializeMenus()
        {
            MainMenu = new MainMenu(Configuration.Instance.MainMenuConfiguration);
            SelectLatticeMenu = new SelectLatticeTypeMenu(Configuration.Instance.SelectLatticeTypeMenuConfiguration);
            LatticesMenu = new SelectFileMenu(Configuration.Instance.SelectFileMenuConfiguration);
            TutorialMenu = new TutorialMenu(Configuration.Instance.TutorialMenuConfiguration);
            SelectScaleDiagramsMenu = new SelectScaleDiagramsMenu(Configuration.SelectScaleDiagramsMenuConfiguration);
        }

        private void RegisterMenus()
        {
            MenuController.RegisterMenu(Configuration.Instance.SelectScaleDiagramsMenuTransform.gameObject, SelectScaleDiagramsMenu);
        }

        #endregion

        #region Exposed Functions

        public void StartContextScenario(string selectedFilename)
        {
            var scenarioStartup = new ContextScenarioStartupConfiguration
            {
                Name = selectedFilename
            };

            Scenario = new ContextScenario(scenarioStartup);
            ToggleMenuVisibility(false);
            Pivot.SnapToActiveLatticeMiddle();
        }

        public void StartScalaScenario(IList<string> diagramNames)//ScalaScenarioStartupConfiguration configuration)
        {
            var scalaScenario = Scenario as ScalaScenario;
            diagramNames.ForEach(x => scalaScenario.addDiagram(x));

            ToggleMenuVisibility(false);
            Pivot.SnapToActiveLatticeMiddle();
        }

        public void StartLifetracksScenario()//LifetracksScenarioStartupConfiguration configuration)
        {
            //var lifetacksScenario = Scenario as LifetracksScenario;
            //lifetacksScenario.BuildLifetrack(configuration as LifetracksScenarioStartupConfiguration);
        }

        #endregion

        #region Menu Events

        public void SelectLatticeTypeMenu_FormalContextSelected()
        {
            LatticesMenu.InitializeContext(DirectoryConfiguration.Instance.ContextLattices);
        }

        public void SelectLatticeTypeMenu_ScaleContextSelected()
        {
            LatticesMenu.InitializeScale(DirectoryConfiguration.Instance.ScalaLattices);
        }

        public void SelectLatticeTypeMenu_LifetracksContextSelected()
        {
            LatticesMenu.InitializeLifetracks(DirectoryConfiguration.Instance.LifetrackLattices);
        }

        public void SelectFileMenu_ScaleContextSelected(string filename)
        {
            var scenario = new ScalaScenario(filename);
            var diagrams = new List<string>(scenario.Diagrams);
            SelectScaleDiagramsMenu.Initialize(diagrams);

            Scenario = scenario;
        }

        public void SelectFileMenu_LifetracksContextSelected(string filename)
        {
            var scenario = new LifetracksScenario(filename);
        }

        #endregion

        #region Input Events

        public void ToggleMenuVisibility(bool shouldShow)
        {
            MenuController.ToggleMenuVisibility(shouldShow);
            CurrentMode = shouldShow ? InputMode.Menu : InputMode.Game;
            TogglePointersIfNeccessary();
        }

        public void ToggleMenuLocalStateOrClose()
        {
            if (!MenuController.ToggleLocalState())
                ToggleMenuVisibility(false);
        }

        #endregion

        #region Helpers

        private void TogglePointersIfNeccessary()
        {
            switch(CurrentMode)
            {
                case InputMode.Game:
                    CanvasPointer.SetActive(false);
                    LaserPointer.active = true;
                    LaserPointer.enabled = true;
                    break;
                case InputMode.Menu:
                    CanvasPointer.SetActive(true);
                    LaserPointer.active = true;
                    LaserPointer.enabled = false;
                    break;
            }
        }

        #endregion

        /*

        #region Main Menu Events

        private void MainMenu_MenuItemSelected(object sender, MainMenuSelectionEventArgs e)
        {
            switch (e.Option) 
            {
                case MainMenuOption.ContextStart:
                    break;
                case MainMenuOption.ScaleConfigure:
                    var scalaScenario = new ScalaScenario(e.Name);
                    //mainMenu.ActivateDiagramSelection(scalaScenario.Diagrams);

                    scenario = scalaScenario;
                    break;
                case MainMenuOption.LifetrackConfigure:
                    var lifetracksScenario = new LifetracksScenario(e.Name);
                    //mainMenu.ActivateDiagramSelection(lifetracksScenario.Diagrams);

                    scenario = lifetracksScenario;
                    break;
                case MainMenuOption.LifetrackStart:
                    
                    break;
                case MainMenuOption.ScaleStart:
                    
                    (e.StartupConfiguration as ScalaScenarioStartupConfiguration).Diagrams.ForEach(Debug.Log);
                    break;
                default:
                    break;
            }
        }

        #endregion

    */

        #region Laser Events

        public void HitNode(PointerEventArgs e)
        {
            e.target.GetComponent<GameobjectNode>().setHighlightLevel(HighlightLevel.Selected);
        }

        public void OutNode(PointerEventArgs e)
        {
            e.target.GetComponent<GameobjectNode>().resetHighlightDefault();
        }

        #endregion

        #region Grip Selection

        public void ShortSelect(GameObject node)
        {
            if ((node == null) || (node.name.Equals("Default")))
            {
                Configuration.SelectedNode = null;
                Scenario.resetSelected();
                return;
            }

            Configuration.SelectedNode = node;
            Scenario.setSelected(node);
        }

        public void LongSelect(GameObject node)
        {
            Scenario.longSelect(node);
        }

        #endregion

        public GameObject getSelectedNode()
        {
            return Configuration.SelectedNode;
        }
        public float GetWarpSeconds()
        {
            return Configuration.WarpSeconds;
        }

        //// LIFETRACKS

        public GameObject getArrowPrefab()
        {
            return Configuration.ArrowPrefab;
        }

        public void touchpadPress()
        {
            Scenario.touchpadPress();
        }
    }
}

