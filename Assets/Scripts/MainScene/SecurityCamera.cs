using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject _lookAtObj;
    private GameObject _closestObj;

    [SerializeField]
    private float _speed;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("WorkPart"))
        {
            if (_closestObj == null)
            {
                _closestObj = other.gameObject;
            } else
            {
                float dist1 = Vector3.Distance(transform.position, _closestObj.transform.position);
                float dist2 = Vector3.Distance(transform.position, other.gameObject.transform.position);

                if (dist2 < dist1)
                {
                    _closestObj = other.gameObject;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _closestObj)
        {
            _closestObj = null;
        }
    }

    public void Update()
    {
        if (_closestObj == null)
            return;

        _lookAtObj.transform.position = Vector3.Lerp(
            _lookAtObj.transform.position,
            _closestObj.transform.position,
            Time.deltaTime * _speed);
    }
}
