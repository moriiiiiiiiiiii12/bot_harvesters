using System;
using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _arriveDistance = 0.3f;

    private Transform _target;
    private Coroutine _coroutine;

    public event Action ReachTarget;

    public void MoveTo(Transform target)
    {
        Reset();

        _target = target;
        _coroutine = StartCoroutine(ExecuteMove());
    }

    private IEnumerator ExecuteMove()
    {
        while (enabled == true)
        {
            Move();

            if (Reached() == true)
            {
                _target = null;
                _coroutine = null;

                if (ReachTarget != null)
                {
                    ReachTarget.Invoke();
                }

                yield break;
            }

            yield return null;
        }
    }

    private void Move()
    {
        if (_target != null)
        {
            float step = _speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
            transform.LookAt(_target);
        }
    }

    private bool Reached()
    {
        return (_target.position - transform.position).sqrMagnitude <= _arriveDistance * _arriveDistance;
    }

    public void Reset()
    {
        _target = null;

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = null;
    }
}
