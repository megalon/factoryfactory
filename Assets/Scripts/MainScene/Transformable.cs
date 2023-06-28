using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformable : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objectsToPick;

    private Transformation[] _transformations;
    private int _currentTransIndex;

    private Phases _currentPhase;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("ActivateTransformation"))
        {
            ActivateTransformation(_currentTransIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("ActivateTransformation"))
        {
            ActivateTransformation(_currentTransIndex);
        }
    }

    public void Init(int transIndex)
    {
        // Since we are using a pool, the object may already be in some other state when we init
        // Need to make sure to only change phase here if we are in the starting phase
        // Other phase changes are handled by the transformations
        if (_currentPhase == Phases.INVALID)
        {
            ChangeToPhase(Phases.BASIC);
        }

        _transformations = gameObject.GetComponents<Transformation>();
        _currentTransIndex = transIndex;
        ActivateAllTransformationsToCurrent();
    }

    private void ActivateAllTransformationsToCurrent()
    {
        if (_transformations == null)
            return;

        for (int i = 0; i < _transformations.Length; ++i)
        {
            if (_transformations[i].index < _currentTransIndex)
            {
                _transformations[i].enabled = true;
            } else
            {
                _transformations[i].enabled = false;
            }
        }
    }

    private void ActivateTransformation(int index)
    {
        if (_transformations == null)
            return;

        for (int i = 0; i < _transformations.Length; ++i)
        {
            if (_transformations[i].index == index)
            {
                _transformations[i].enabled = true;
                break;
            }
        }
    }

    /// <summary>
    /// Activates the gameobject for the phase and deactives all the others
    /// The order of the objects in the list in the editor MUST match the Phases
    /// </summary>
    /// <param name="pick"></param>
    public void ChangeToPhase(Phases pick)
    {
        _currentPhase = pick;

        // Subtract one because there is no object for the invalid phase state
        int pickIndex = ((int)_currentPhase) - 1;

        // Skip if there's no object to enable
        if (objectsToPick[pickIndex] == null)
        {
            return;
        }

        for (int i = 0; i < objectsToPick.Count; ++i)
        {
            if (objectsToPick[i] == null)
            {
                continue;
            }

            objectsToPick[i].SetActive(i == pickIndex);
        }
    }

    public Phases GetCurrentPhase()
    {
        return _currentPhase;
    }

    public enum Phases
    {
        INVALID,
        BASIC,
        CHOPPED,
        POWDER,
        PACKAGED
    }
}
