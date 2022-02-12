using Assets.Scripts.Configuration.Menus;
using Assets.Scripts.Input.Actions;
using Assets.Scripts.UI.MainMenu;
using Assets.Scripts.View.Menus;
using InputSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.Configuration
{
    class Configuration : MonoBehaviour
    {
        private static Configuration _instance;
        private static MainMenuConfiguration _mainMenuConfiguration;
        private static InputConfiguration _inputConfiguration;

        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<Configuration>();
                return _instance;
            }
        }

        public InputConfiguration InputConfiguration => GetInputConfiguration();
        public MainMenuConfiguration MainMenuConfiguration => GetMainMenuConfiguration();
        public SelectLatticeTypeMenuConfiguration SelectLatticeTypeMenuConfiguration => GetSelectLatticeTypeMenuConfiguration();
        public SelectFileMenuConfiguration SelectFileMenuConfiguration => GetSelectFileMenuConfiguration();

        public TutorialMenuConfiguration TutorialMenuConfiguration => GetTutorialMenuConfiguration();

        public SelectScaleDiagramsMenuConfiguration SelectScaleDiagramsMenuConfiguration => GetSelectScaleDiagramsMenuConfiguration();

        private MainMenuConfiguration GetMainMenuConfiguration()
        {
            if (_mainMenuConfiguration == null)
                _mainMenuConfiguration = new MainMenuConfiguration
                {
                    MainMenu = MainMenuTransform,
                    SelectLattice = SelectLatticeButton,
                    Exit = ExitButton
                };
            return _mainMenuConfiguration;
        }

        private InputConfiguration GetInputConfiguration()
        {
            if (_inputConfiguration == null)
                _inputConfiguration = new InputConfiguration
                {
                    Player = Player
                };
            return _inputConfiguration;
        }

        private SelectLatticeTypeMenuConfiguration GetSelectLatticeTypeMenuConfiguration()
        {
            return new SelectLatticeTypeMenuConfiguration
            {
                FormalContextButton = FormalContextButton,
                ScaleContextButton = ScaleContextButton,
                LifetracksContextButton = LifetracksContextButton,
                Back = SelectLatticeBackButton,
                SelectFileMenuTransform = SelectFileMenuTransform
            };
        }

        private SelectFileMenuConfiguration GetSelectFileMenuConfiguration()
        {
            return new SelectFileMenuConfiguration
            {
                BackButton = SelectFileBackButton,
                SelectFileSizer = SelectFileSizer,
                ScrollViewContentTransform = ScrollViewContentTransform,
                SelectScaleDiagramsMenuTransform = SelectScaleDiagramsMenuTransform
            };
        }

        private SelectScaleDiagramsMenuConfiguration GetSelectScaleDiagramsMenuConfiguration()
        {
            return new SelectScaleDiagramsMenuConfiguration
            {
                PrimaryLocalMenuText = SelectScaleDiagramsPrimaryTitle,
                PrimaryLocalMenuTransform = SelectScaleDiagramsPrimaryTransform,
                SecondaryLocalMenuText = SelectScaleDiagramsSecondaryTitle,
                SecondaryLocalMenuTransform = SelectScaleDiagramsSecondaryTransform,
                SelectedMenuTransform = SelectScaleDiagramsSecondaryTransform,
                NotSelectedMenuTransform = SelectScaleDiagramsPrimaryTransform,
                NotSelectedDiagramsContent = NotSelectedDiagramsContentTransform,
                SelectedDiagramsContent = SelectedDiagramsContentTransform,
                BackButton = SelectScaleDiagramsBackButton,
                StartButton = SelectScaleDiagramsStartButton
            };
        }

        private TutorialMenuConfiguration GetTutorialMenuConfiguration()
        {
            return new TutorialMenuConfiguration
            {
                TutorialMenuTransform = TutorialMenuTransform,
                AskToggle = TutorialMenuAskToggle,
                YesButton = TutorialMenuYesButton,
                NoButton = TutorialMenuNoButton
            };
        }

#pragma warning disable CS0649
        [Header("Runtime settings")]
        public GameObject RightHand;
        public GameObject LeftHand;
        public GameObject CameraRig;

        [Range(1, 5)]
        public float WarpSeconds = 3f;

        [Header("Systems")]
        public MenuController MenuSwitcher;
        public EventManager EventManager;

        [Header("Input settings")]
        public GameObject Player;
        public Pivot Pivot;

        [Header("Dynamic settings")]
        public GameObject SelectedNode;

        [Header("Prefabs")]
        public GameObject ScrollViewItemPrefab;
        public GameObject ArrowPrefab;

        [Header("Main Menu Configuration")]
        public GameObject MainMenuTransform;
        public GameObject SelectLatticeButton;
        public GameObject ServerButton;
        public GameObject ConnectButton;
        public GameObject ExitButton;

        [Header("Select Lattice Type Menu Configuration")]
        public GameObject SelectLatticeTypeMenu;
        public GameObject FormalContextButton;
        public GameObject ScaleContextButton;
        public GameObject LifetracksContextButton;
        public GameObject SelectLatticeBackButton;

        [Header("Select File Configuration")]
        public GameObject SelectFileMenu;
        public GameObject SelectFileBackButton;
        public ScrollContainerSizer SelectFileSizer;
        public Transform SelectFileMenuTransform;
        public Transform ScrollViewContentTransform;

        [Header("Game Menu Configuration")]

        [Header("Tutorial Prompt Configuration")]
        public Transform TutorialMenuTransform;
        public GameObject TutorialMenuYesButton;
        public GameObject TutorialMenuNoButton;
        public GameObject TutorialMenuAskToggle;

        [Header("Select Scale Diagrams Menu Configuration")]
        public Transform SelectScaleDiagramsMenuTransform;
        public GameObject SelectScaleDiagramsPrimaryTitle;
        public GameObject SelectScaleDiagramsSecondaryTitle;
        public Transform SelectScaleDiagramsPrimaryTransform;
        public Transform SelectScaleDiagramsSecondaryTransform;
        public Transform NotSelectedDiagramsContentTransform;
        public Transform SelectedDiagramsContentTransform;
        public GameObject SelectScaleDiagramsBackButton;
        public GameObject SelectScaleDiagramsStartButton;
#pragma warning restore CS0649
    }
}
