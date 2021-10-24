using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CActionBinder : CParent
{

    public bool IsFollowMode = true;

    protected CMover AttachedMover = null;

    private List<BindedAction> BindedActionsList = new List<BindedAction>();


    public void bind(BindedAction Action)
    {
        BindedActionsList.Add(Action);
    }


    public void unbind(KeyCode KC, BindedAction.Mode Mode)
    {
        foreach (BindedAction iAction in BindedActionsList)
        {
            if(iAction.Key == KC && iAction.KeyMode == Mode)
            {
                BindedActionsList.Remove(iAction);

                return;
            }
        }
    }


    protected void followCurrentMover()
    {
        if( IsFollowMode && AttachedMover!=null )
        {
            Vector3 CapturedPos = Camera.main.transform.position;
            CapturedPos.x = AttachedMover.transform.position.x;
            CapturedPos.y = AttachedMover.transform.position.y;
            Camera.main.transform.position = CapturedPos;
        }
   
    }


    public void attachMover(CMover iMover)
    {
        AttachedMover = iMover;
    }


    private void Update()
    {
        //followCurrentMover();

        foreach(BindedAction iAction in BindedActionsList)
        {
            if (iAction.KeyMode == BindedAction.Mode.REPEAT)
            {

                if (Input.GetKey(iAction.Key)) iAction.Action();

            }
            else if (iAction.KeyMode == BindedAction.Mode.PRESSED)
            {

                if (Input.GetKeyDown(iAction.Key)) iAction.Action();

            }
            else if (iAction.KeyMode == BindedAction.Mode.RELEASED)
            {

                if (Input.GetKeyUp(iAction.Key)) iAction.Action();

            }else if( iAction.KeyMode == BindedAction.Mode.MOUSE_REPEAT)
            {

                if (Input.GetMouseButton((int)iAction.Key)) iAction.Action();

            }else if (iAction.KeyMode == BindedAction.Mode.MOUSE_PRESSED)
            {

                if (Input.GetMouseButtonDown((int)iAction.Key)) iAction.Action();

            }else if (iAction.KeyMode == BindedAction.Mode.MOUSE_RELEASED)
            {

                if (Input.GetMouseButtonUp((int)iAction.Key)) iAction.Action();

            }
        }
    }
    
}
