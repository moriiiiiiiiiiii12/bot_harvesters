using System;
using UnityEngine;
using UnityEngine.UI;


public class CounterResource : MonoBehaviour
{
    [SerializeField] private DropPoint _dropPoint;
    [SerializeField] private Text _textHeader;
    [SerializeField] private Text _textCount;

    private int _count;
    public int Count => _count;

    public event Action CountChanged;

    private void Awake()
    {
        _count = 0;
    }

    private void OnEnable()
    {
        _dropPoint.ResourceReceived += Increase;
    }

    private void OnDisable()
    {
        _dropPoint.ResourceReceived -= Increase;
    }

    private void Start()
    { 
        _textHeader.text = "Ресурсов: ";
    }

    private void UpdateCount()
    {
        _textCount.text = _count.ToString();
    }

    private void Increase()
    {
        _count++;

        CountChanged?.Invoke();

        UpdateCount();
    }

    public void Decrease(int count)
    {
        _count -= count;

        CountChanged?.Invoke();

        UpdateCount();
    }
}