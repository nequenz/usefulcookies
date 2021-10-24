using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BParent : MonoBehaviour
{

    private List<CParent> ComponentList = new List<CParent>();

    public int ID { get; private set; }



    public bool  attachComponent(CParent iComponent)
    {
        if (iComponent == null) return false;


        foreach(CParent iiComponent in ComponentList)
        {
            if (iiComponent.GetType() == iComponent.GetType()) return false;
        }

        iComponent.Behaviour = this; 
        ComponentList.Add(iComponent);

        return true;
    }


    public CParent getComponent<T>()
    {
        foreach(CParent iComponent in ComponentList)
        {
            if (iComponent is T) return iComponent;
        }

        return null;
    }


    protected virtual void Start()
    {
        ID++;
    }


}