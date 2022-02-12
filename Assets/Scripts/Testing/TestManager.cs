using Assets.Scripts.Scenario;
using Assets.Scripts.Scenario.Context;
using FCA.Repository;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    Scenario scenario;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
    }

    public void startLifetrack(string subjectDiagramName, string timeDiagramName, string searchedDiagramName)
    {
        if (scenario is LifetracksScenario)
        {
            //TODO: WOW
            //((LifetracksScenario)scenario).buildLifetrack(subjectDiagramName, timeDiagramName, searchedDiagramName);
        }
    }

    public void startScala(List<string> diagramList)
    {
        if (scenario is ScalaScenario)
        {
            foreach (string diagramName in diagramList)
            {
                ((ScalaScenario)scenario).addDiagram(diagramName);
            }
        }
    }
    
    /*
    public void resetSelected()
    {
        scenario.resetSelected();
    }

    public void setSelected(GameObject node)
    {
        scenario.setSelected(node);
    }

    public void longSelect(GameObject node)
    {
        scenario.longSelect(node);
    }

    public void touchpadPress()
    {
        scenario.touchpadPress();
    }
    */
}
