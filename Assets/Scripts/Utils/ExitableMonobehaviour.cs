using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitableMonobehaviour : MonoBehaviour
{
    protected bool exitStateImmediately = false;
    protected bool stopGeneratingImmediately = false;

    public virtual void ExitStateImmediately()
    {
        exitStateImmediately = true;
    }

    public void ResetExitStateImmediately()
    {
        exitStateImmediately = false;
    }

    public virtual void StopGeneratingImmediately()
    {
        stopGeneratingImmediately = true;
    }

    public void ResetStopGeneratingImmediately()
    {
        stopGeneratingImmediately = false;
    }
}
