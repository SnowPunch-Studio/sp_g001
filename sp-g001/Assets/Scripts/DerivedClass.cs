using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DerivedClass : BaseClass
{
    public override void execute()
    {
        Debug.Log("It worked!");
    }
}
