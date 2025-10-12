using UnityEngine;
using UnityEngine.UI;


class Counter : MonoBehaviour
{
    [SerializeField] private DropPoint _dropPoint;
    [SerializeField] private Text _textHeader;
    [SerializeField] private Text _textCount;

    private int _count;

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

        UpdateCount();
    }
}