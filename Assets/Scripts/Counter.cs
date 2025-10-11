using UnityEngine;
using UnityEngine.UI;


class Counter : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private Text _textHeader;
    [SerializeField] private Text _textCount;

    private int _count;

    private void OnEnable()
    {
        _base.ResourceReceived += Increase;
    }

    private void OnDisable()
    {
        _base.ResourceReceived -= Increase;
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