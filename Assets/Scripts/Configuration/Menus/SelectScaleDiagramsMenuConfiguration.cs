using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Configuration.Menus
{
    class SelectScaleDiagramsMenuConfiguration : DoubleMenuConfiguration
    {
        public Transform NotSelectedMenuTransform { get; set; }

        public Transform SelectedMenuTransform { get; set; }

        public Transform NotSelectedDiagramsContent { get; set; }

        public Transform SelectedDiagramsContent { get; set; }

        public GameObject BackButton { get; set; }

        public GameObject StartButton { get; set; }
    }
}
