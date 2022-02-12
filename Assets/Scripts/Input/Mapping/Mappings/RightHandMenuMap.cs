using Assets.Scripts.Input.Pointers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.VR.Extras;

namespace Assets.Scripts.Input.Mapping.Mappings
{
    class RightHandMenuMap : AbstractInputMap
    {

        public override void ShortGripPress()
        {
        }

        public override void LongGripPress()
        {
        }

        public override void MenuPress()
        {
            EventManager.ToggleMenuLocalStateOrClose();
        }

        public override void TouchpadPress()
        {
        }

        public override void TriggerPressed()
        {

        }

        public override void TriggerPressDown()
        {
            
        }
    }
}
