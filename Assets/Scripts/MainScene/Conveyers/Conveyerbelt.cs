using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Conveyerbelt : MonoBehaviour
{
    [SerializeField]
    [FormerlySerializedAs("speed")]
    private float _speed;

    [SerializeField]
    private Rigidbody _rbody;
    [SerializeField]
    private Transform _capsule1;
    [SerializeField]
    private Transform _capsule2;

    private float _magicNumber = 2.5f;

    private Vector3 _position;

    // Update is called once per frame
    void FixedUpdate()
    {
        _position = _rbody.position;
        _rbody.position += transform.right * _speed * Time.fixedDeltaTime;
        _rbody.MovePosition(_position);

        _capsule1.Rotate(Vector3.forward, _speed, Space.Self);
        _capsule2.Rotate(Vector3.forward, _speed, Space.Self);
    }


    public void SetSpeed(float speed)
    {
        _speed = speed;

        // The magic number to divide and get the correct UV speed
        // This was found by trial and error
        ConveyerbeltUVMoverSingle.Instance.SetSpeed(_speed / _magicNumber);
    }
}
