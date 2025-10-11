using System;
using System.Collections;
using UnityEngine;


class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _arriveDistance = 0.1f;

    private Transform _target;
    private Vector3 _targetPosition => _target.position;

    private Coroutine _coroutine;

    public event Action ReachTarget;

    public void SetTarget(Transform target)
    {
        Reset();

        _target = target;

        _coroutine = StartCoroutine(ExecuteMove());
    }

    private IEnumerator ExecuteMove()
    {
        while (enabled)
        {
            MoveTowardsTarget();

            if (Reached())
            {
                ReachTarget?.Invoke();
                Reset();

                yield break;
            }

            yield return null;
        }
    }

    private void MoveTowardsTarget()
    {
        if (_target != null)
        {
            float step = _speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);

            transform.LookAt(_target);
        }
    }

    private bool Reached()
    {
        return (_targetPosition - transform.position).sqrMagnitude <= _arriveDistance * _arriveDistance;
    }

    public void Reset()
    {
        _target = null;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = null;
        ReachTarget = null;
    }
}
