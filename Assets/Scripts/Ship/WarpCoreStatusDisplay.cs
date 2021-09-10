using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarpCoreStatusDisplay : MonoBehaviour
{
    private Ship _ship;
    private Slider _statusSlider;
    private TMP_Text _statusText;

    void Start()
    {
        _ship = FindObjectOfType<Ship>();
        _statusSlider = GetComponentInChildren<Slider>();
        _statusText = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        _statusSlider.value = _ship._warpCoreCharge / _ship._maxWarpCoreCharge;
        _statusText.text = $"Warp Core: {_statusSlider.value * 100f:F1}%";
    }
}