using System.Collections.Generic;
using UnityEngine;


namespace FCA.UI
{
    public interface LabelStrategy
    {
        void setupLabels(GameObject latticeObject);
        void updateCondition(List<string> conditions);
    }
}
