using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class KeySymbolUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _pickUpColor;

    public Image Image => _image;
    public Color StartColor => _startColor;

    private void Start() => _image.color = _startColor;

    public void CollectSymbolAnim()
    {
        _image.DOColor(_pickUpColor, 1f);
        _image.transform.DOScale(1.5f, 0.35f).SetLoops(2, LoopType.Yoyo);
    }
}
