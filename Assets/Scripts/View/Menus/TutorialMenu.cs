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
    class TutorialMenu : Menu
    {
        private TutorialMenuConfiguration Configuration;

        public TutorialMenu(TutorialMenuConfiguration configuration)
        {
            Configuration = configuration;
            RegisterActionEvents();
            RegisterNavigationEvents();
        }

        protected override void RegisterActionEvents()
        {
            RegisterActionEvent(Configuration.YesButton, YesAction);
        }

        protected override void RegisterNavigationEvents()
        {
            RegisterNavigationEvent(Configuration.NoButton);
        }

        private void YesAction()
        {
            var shouldAskAgain = Configuration.AskToggle.GetComponent<Toggle>().isOn ?
                "no" :
                "yes";

            PlayerPrefs.SetString("tutorial", shouldAskAgain);
            PlayerPrefs.Save();
        }
    }
}
