using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ComponentList<T>
{

    private List<T> CList = new List<T>();

    public List<T> getList()
    {
        return CList;
    }

    public void refreshList()
    {
        foreach (T iComponent in CList)
        {
            if (iComponent == null) CList.Remove(iComponent);
        }
    }

    public void add(T iComponent)
    {
        refreshList();
        CList.Add(iComponent);
    }

    
}