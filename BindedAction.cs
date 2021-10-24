using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindedAction
{

    public enum Mode
    {
        REPEAT,
        PRESSED,
        RELEASED,

        MOUSE_REPEAT,
        MOUSE_PRESSED,
        MOUSE_RELEASED,

        TOUCH_REPEAT,
        TOUCH_PRESSED,
        TOUCH_RELEASED

    }

    public Events.OnAction Action = delegate { };

    public KeyCode Key = 0;

    public Mode KeyMode = Mode.REPEAT;

    public BindedAction(KeyCode KC, BindedAction.Mode M, Events.OnAction iAction)
    {

        Key = KC;
        KeyMode = M;
        Action = iAction;

        if (Action == null) Action = delegate { };


    }

}