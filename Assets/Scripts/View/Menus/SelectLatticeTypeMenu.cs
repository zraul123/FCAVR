using Assets.Scripts.Configuration.Menus;
using InputSystem;

namespace Assets.Scripts.View.Menus
{
    class SelectLatticeTypeMenu : Menu
    {
        SelectLatticeTypeMenuConfiguration Configuration;
        EventManager EventManager;

        public SelectLatticeTypeMenu(SelectLatticeTypeMenuConfiguration configuration)
        {
            EventManager = Scripts.Configuration.Configuration.Instance.EventManager;
            Configuration = configuration;

            RegisterNavigationEvents();
            RegisterActionEvents();
        }

        #region Event Registration

        protected override void RegisterNavigationEvents()
        {
            RegisterNavigationEvent(Configuration.Back);
        }

        protected override void RegisterActionEvents()
        {
            RegisterActionEvent(Configuration.FormalContextButton, FormalContextSelected);
            RegisterActionEvent(Configuration.ScaleContextButton, ScaleContextSelected);
            RegisterActionEvent(Configuration.LifetracksContextButton, LifetracksContextSelected);
        }

        #endregion

        #region Actions

        private void FormalContextSelected()
        {
            EventManager.SelectLatticeTypeMenu_FormalContextSelected();
            MenuSwitcher.SwitchMenuState(Configuration.SelectFileMenuTransform);
        }

        private void ScaleContextSelected()
        {
            EventManager.SelectLatticeTypeMenu_ScaleContextSelected();
            MenuSwitcher.SwitchMenuState(Configuration.SelectFileMenuTransform);
        }

        private void LifetracksContextSelected()
        {
            EventManager.SelectLatticeTypeMenu_LifetracksContextSelected();
            MenuSwitcher.SwitchMenuState(Configuration.SelectFileMenuTransform);
        }

        #endregion
    }
}
