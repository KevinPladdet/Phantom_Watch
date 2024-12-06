using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _objPage1, _objPage2, _objPage3, _objPage4;

    [Space(10)]
    [SerializeField] private AudioClip _pageFlipSFX;
    private int _currentPage = 0;

    public void Interact()
    {
        if (_currentPage == 0)
        {
            _objPage1.SetActive(false);
            _objPage2.SetActive(false);
            _objPage3.SetActive(true);
            _objPage4.SetActive(true);
            _currentPage = 1;
        }
        else if (_currentPage == 1)
        {
            _objPage1.SetActive(true);
            _objPage2.SetActive(true);
            _objPage3.SetActive(false);
            _objPage4.SetActive(false);
            _currentPage = 0;
        }
        AudioPool.Instance.PlaySound(_pageFlipSFX, 0.5f, true);
    }
}
