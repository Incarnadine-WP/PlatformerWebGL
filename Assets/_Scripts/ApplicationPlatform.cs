using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ApplicationPlatform : MonoBehaviour
{
    [SerializeField] private GameObject _androidInput;
    [SerializeField] private TextMeshProUGUI _checkPlatform;

    private void Awake()
    {

        if (Application.platform == RuntimePlatform.Android)
            _androidInput.SetActive(true);
        else
            _androidInput.SetActive(false);

        _checkPlatform.text = Application.platform.ToString();
    }

}
