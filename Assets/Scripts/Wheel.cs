using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Wheel : MonoBehaviour, IWheel
{
    public delegate void Delegats();
    public event Delegats event_ChooseIsEnd;

    [Header("Components")]
    [SerializeField] private Rigidbody rigidBody;
    [Space(10)]

    [Header("Need to work objects")]
    [SerializeField] private Transform targetForChoseeSide;
    [SerializeField] private Transform[] posOfCart = new Transform[4];
    [Space(10)]

    [Header("Charackteristics")]
    [SerializeField] private float[] _distanceOfCarts = new float[4];
    public int _numOfChoosenWheel;

    [SerializeField] private float _forceOfCurrent = 0.05f; // сила для выравнивания колеса
    [SerializeField] private float _distanceOfCentre = 0.001f; // расстояние к которому наужно стрмится колесу

    private float speedOfRotate;
    public void AddTorqueImpulse()
    {
        _numOfChoosenWheel = int.MaxValue;

        speedOfRotate = Random.Range(10.0f, 10.91f);

        rigidBody.angularDrag = Random.Range(.05f, 0.21f);
        rigidBody.AddTorque(new Vector3(speedOfRotate, 0, 0), ForceMode.Impulse);
    }

    public void StopRotate()
    {
        rigidBody.angularDrag = Random.Range(1.01f, 1.21f);

        StartCoroutine(ChoseeSide());
    }

    private IEnumerator ChoseeSide()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            for (int i = 0; i < posOfCart.Length; i++)
                _distanceOfCarts[i] = Vector3.Distance(posOfCart[i].position, targetForChoseeSide.position);

            if (rigidBody.angularVelocity.x < 0.5f)
            {
                float minDistance = float.MaxValue;

                for (int i = 0; i < _distanceOfCarts.Length; i++)
                {
                    if (_distanceOfCarts[i] < minDistance)
                    {
                        _numOfChoosenWheel = i;
                        minDistance = _distanceOfCarts[i];
                    }
                }

                rigidBody.angularDrag = 2f;

                while (true)
                {
                    yield return new WaitForSeconds(.01f);

                    if (posOfCart[_numOfChoosenWheel].position.y > targetForChoseeSide.position.y) //                     
                        rigidBody.AddTorque(new Vector3(-_forceOfCurrent, 0, 0), ForceMode.Impulse);
                    else if (posOfCart[_numOfChoosenWheel].position.y < targetForChoseeSide.position.y) //                     
                        rigidBody.AddTorque(new Vector3(_forceOfCurrent, 0, 0), ForceMode.Impulse);

                    if (targetForChoseeSide.position.y + _distanceOfCentre > posOfCart[_numOfChoosenWheel].position.y &&
                        targetForChoseeSide.position.y - _distanceOfCentre < posOfCart[_numOfChoosenWheel].position.y)
                    {
                        rigidBody.isKinematic = true;
                        break;
                    }
                    // доводка колеса

                }

                break;
            }
        }

        // on default
        rigidBody.isKinematic = false;
        event_ChooseIsEnd?.Invoke();
    }
}
