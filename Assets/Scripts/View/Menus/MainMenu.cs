using System;
using System.Collections.Generic;
using Assets.Scripts.Configuration;
using Assets.Scripts.View.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenu
{
    class MainMenu : Menu
    {
        MainMenuConfiguration Configuration;

        #region Initialization

        public MainMenu(MainMenuConfiguration configuration)
        {
            Configuration = configuration;

            RegisterNavigationEvents();
            RegisterActionEvents();
        }

        protected override void RegisterNavigationEvents()
        {
            RegisterNavigationEvent(Configuration.SelectLattice);
        }

        protected override void RegisterActionEvents()
        {
            RegisterActionEvent(Configuration.Exit, ExitAction);
        }

        #endregion

        #region Actions

        private void ExitAction()
        {
            System.Environment.Exit(0);
        }

        #endregion

        #region Exposed functions

        public void ShowMainMenu()
        {
            MenuSwitcher.SwitchMenuState(Configuration.MainMenu);
        }

        #endregion

        /*

        private void InitializeScalaRow()
        {
            var scalaRowTransform = Configuration.Hoverpanel.transform.GetChild(2);
            var files = DirectoryConfiguration.Instance.ScalaLattices;

            files.ForEach((fileName) => CreateMenuItemPrefab(scalaRowTransform, fileName, OnScalaConfigureSelected));
        }

        private void InitializeLifetrackRow()
        {
            var lifetracksRowTransform = Configuration.Hoverpanel.transform.GetChild(3);
            var files = DirectoryConfiguration.Instance.LifetrackLattices;

            files.ForEach((fileName) => CreateMenuItemPrefab(lifetracksRowTransform, fileName, OnLifetracksConfigureSelected));
        }
        */

        /*

       #region Events

       private void OnScalaStartSelected(IItemDataSelectable selectedItem)
       {
           Hide();
           var startList = new List<string>();
           foreach (Transform child in Configuration.DiagramPanel.transform.GetChild(0))
           {
               HoverItemDataCheckbox hoverItem = child.GetComponent<HoverItemDataCheckbox>();
               if (hoverItem != null)
               {
                   startList.Add(hoverItem.Label);
               }
           }
           var startupConfiguration = new ScalaScenarioStartupConfiguration { Diagrams = startList };

           MenuItemSelected?.Invoke(this, new MainMenuSelectionEventArgs { Option = MainMenuOption.ScaleStart, StartupConfiguration = startupConfiguration });
       }

       private void OnScalaConfigureSelected(IItemDataSelectable selectedItem)
       {
           _startHandler = OnScalaStartSelected;
           _transferHandler = TransferCheckbox;

           MenuItemSelected?.Invoke(this, new MainMenuSelectionEventArgs { Option = MainMenuOption.ScaleConfigure, Name = selectedItem.Label });
       }

       private void OnContextSelected(IItemDataSelectable selectedItem)
       {
           Hide();

           var startupConfiguration = new ContextScenarioStartupConfiguration { Name = selectedItem.Label };
           MenuItemSelected?.Invoke(this, new MainMenuSelectionEventArgs { Option = MainMenuOption.ContextStart, StartupConfiguration = startupConfiguration });
       }

       private void OnLifetracksStartSelected(IItemDataSelectable selectedItem)
       {
           Hide();
           var rowTx = Configuration.LifetrackPanel.transform.GetChild(0);
           string subjectDiagram = rowTx.GetChild(0).GetComponent<HoverItemDataCheckbox>().Label.Split(':')[1].Trim();
           string timeDiagram = rowTx.GetChild(1).GetComponent<HoverItemDataCheckbox>().Label.Split(':')[1].Trim();
           string searchedDiagram = rowTx.GetChild(2).GetComponent<HoverItemDataCheckbox>().Label.Split(':')[1].Trim();

           var startupConfiguration = new LifetracksScenarioStartupConfiguration
           {
               Subject = subjectDiagram,
               Time = timeDiagram,
               Searched = searchedDiagram
           };

           MenuItemSelected?.Invoke(this, new MainMenuSelectionEventArgs { Option = MainMenuOption.LifetrackStart, StartupConfiguration = startupConfiguration });
       }

       private void OnLifetracksConfigureSelected(IItemDataSelectable selectedItem)
       {
           _startHandler = OnLifetracksStartSelected;
           _transferHandler = transferLifetracks;

           MenuItemSelected?.Invoke(this, new MainMenuSelectionEventArgs { Option = MainMenuOption.LifetrackConfigure, Name = selectedItem.Label });
       }

       #endregion
       */


        /*
        #region Helpers

        private GameObject CreateRow(IEnumerable<string> items, SelectedHandler startHandler, SelectedHandler transferHandler)
        {
            var row = UnityEngine.Object.Instantiate(Configuration.RowPrefab);

            row.transform.GetChild(0).GetComponent<HoverpanelRowSwitchingInfo>().NavigateToRow = Configuration.Hoverpanel.transform.GetChild(0).GetComponent<HoverLayoutRectRow>();
            row.transform.GetChild(1).GetComponent<IItemDataSelectable>().OnSelected += startHandler;
            items.ForEach(item => CreateRowCheckbox(row.transform, item, transferHandler));
            row.transform.SetParent(Configuration.Hoverpanel.transform);
            row.transform.localScale = new Vector3(1, 1, 1);
            row.transform.position = Configuration.Hoverpanel.transform.position;

            return row;
        }

        private void CreateRowCheckbox(Transform parentRow, string label, SelectedHandler transferHandler)
        {
            var checkbox = UnityEngine.Object.Instantiate(Configuration.CheckboxPrefab);

            checkbox.transform.SetParent(parentRow);
            checkbox.transform.localScale = new Vector3(1, 1, 1);
            var hoverItemDataCheckbox = checkbox.GetComponent<HoverItemDataCheckbox>();
            hoverItemDataCheckbox.Label = label;
            hoverItemDataCheckbox.OnSelected += transferHandler;
        }

        private void Hide()
        {
            Configuration.Hoverpanel.SetActive(false);
            Configuration.DiagramPanel.SetActive(false);
            Configuration.LifetrackPanel.SetActive(false);
        }

        #endregion

        #region Handlers

        private void transferLifetracks(IItemDataSelectable pItem)
        {
            string newString;
            int numberChildren = Configuration.LifetrackPanel.transform.GetChild(0).childCount;
            switch (numberChildren)
            {
                case 0:
                    newString = "Subject Diagram : ";
                    break;
                case 1:
                    newString = "Time Diagram : ";
                    break;
                case 2:
                    newString = "Searched Diagram : ";
                    break;
                default:
                    return;
            }

            Transform parent = pItem.gameObject.transform.parent;
            Transform other = (parent == Configuration.Hoverpanel.transform.GetChild(4)) ? 
                Configuration.LifetrackPanel.transform.GetChild(0) : 
                Configuration.Hoverpanel.transform.GetChild(4);

            string lastLabel = pItem.gameObject.GetComponent<HoverItemDataCheckbox>().Label;
            Debug.Log(lastLabel);
            Debug.Log($"L: {(other == Configuration.LifetrackPanel.transform.GetChild(0))}");
            pItem.gameObject.GetComponent<HoverItemDataCheckbox>().Label = (other == Configuration.LifetrackPanel.transform.GetChild(0)) ? (newString + lastLabel) : (lastLabel.Split(':')[1].Trim());

            pItem.gameObject.transform.SetParent(other);
            pItem.gameObject.transform.position = other.transform.position;
            pItem.gameObject.transform.localScale = new Vector3(1, 1, 1);
            pItem.gameObject.GetComponent<HoverItemDataCheckbox>().Value = false;
            pItem.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        private void TransferCheckbox(IItemDataSelectable selectable)
        {
            Transform parent = selectable.gameObject.transform.parent;
            Transform other = (parent == Configuration.Hoverpanel.transform.GetChild(4)) ?
                Configuration.DiagramPanel.transform.GetChild(0) :
                Configuration.Hoverpanel.transform.GetChild(4);

            selectable.gameObject.transform.SetParent(other);
            selectable.gameObject.transform.position = other.transform.position;
            selectable.gameObject.transform.localScale = new Vector3(1, 1, 1);
            selectable.gameObject.GetComponent<HoverItemDataCheckbox>().Value = false;
            selectable.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        #endregion
    */
    }
}