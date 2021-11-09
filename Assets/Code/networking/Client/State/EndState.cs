using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : State
{
    EndView endview;

    protected override void Awake()
    {
        base.Awake();
        endview = GetComponent<EndView>();
      //  SendGameLoadedMessage();
    }
}
