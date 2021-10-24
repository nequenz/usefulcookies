using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InstanceBuilder
{


    public static InstanceBuilderResult buildDynamic(BParent iInstance)
    {

        //Unity components system

        CMover Mover = iInstance.GetComponent<CMover>();

        CRender Render = iInstance.GetComponent<CRender>();

        CCollider Collider = iInstance.GetComponent<CCollider>();

        //Game components system

        if (iInstance.attachComponent(Mover) && iInstance.attachComponent(Render) && iInstance.attachComponent(Collider))
        {

            Mover.Speed = 20.0f;


            Render.attachUnityComponents();

            Render.resetMesh();

            Render.setOwnMesh();


            Collider.OnCollide = (Hit H) => { Debug.Log("default collision"); };

            Collider.IsGizmosRendering = false;
        }

        return null;
    }

    public static InstanceBuilderResult buildDestructable(BParent iInstance)
    {

        //Unity components system

        CDestructable Surface = iInstance.GetComponent<CDestructable>();


        //Game components system

        if ( iInstance.attachComponent(Surface) )
        {
            Surface.AttachedCMover=iInstance.GetComponent<CMover>();

            Surface.AttachedCCollider=iInstance.GetComponent<CCollider>() ;

            Surface.AttachedCRender=iInstance.GetComponent<CRender>() ;
        }
        

        return null;
    }

    public static InstanceBuilderResult buildPlayer(BParent iInstance)
    {

        buildDynamic(iInstance);

        buildDestructable(iInstance);

        iInstance.GetComponent<CCollider>().OnCollide = (Hit H) => { Debug.Log("Player update"); };

        CDestructable iSurface = iInstance.GetComponent<CDestructable>();

        iSurface.newBody(GameTextures.TexTest00,true);

        iSurface.calculateColliderRect();

        CSpawnAnimator iAnimator = iInstance.GetComponent<CSpawnAnimator>();

        if( iInstance.attachComponent(iAnimator))
        {

            iAnimator.Behaviour = iInstance;

            iAnimator.AttachedMover = iInstance.GetComponent<CMover>();

            iAnimator.AttachedRender = iInstance.GetComponent<CRender>();

            iAnimator.AttachedSurface = iInstance.GetComponent<CDestructable>();

        }

        CActionBinder Controller = iInstance.GetComponent<CActionBinder>();

        CGunHandler Gun = iInstance.GetComponent<CGunHandler>();



        //that what does player a player

        iAnimator.spawn();


        /*
        MyController.bind(new BindedAction(KeyCode.W, BindedAction.Mode.REPEAT, OnMoveUp));
        MyController.bind(new BindedAction(KeyCode.S, BindedAction.Mode.REPEAT, OnMoveDown));
        MyController.bind(new BindedAction(KeyCode.A, BindedAction.Mode.REPEAT, OnMoveLeft));
        MyController.bind(new BindedAction(KeyCode.D, BindedAction.Mode.REPEAT, OnMoveRight));
        MyController.bind(new BindedAction(0, BindedAction.Mode.MOUSE_PRESSED, OnShooting));
        */

        return null;
    }


    public static InstanceBuilderResult buildProjectle(CParent iInstance)
    {


        return null;
    }

}