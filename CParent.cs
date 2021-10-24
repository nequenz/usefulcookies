using System;
using UnityEngine;

public abstract class CParent : MonoBehaviour
{



    public BParent Behaviour
    {
        set 
        {
            if (Behaviour == null)
            {
                Behaviour = value;

                attachNeighbors();
            }
                
        }
        protected get
        {
            return Behaviour;
        }
    }


    protected abstract void attachNeighbors();


    protected CParent getNeighborComponent<T>() => Behaviour.getComponent<T>();







    private void Start()
    {
        enabled = false;
    }

}