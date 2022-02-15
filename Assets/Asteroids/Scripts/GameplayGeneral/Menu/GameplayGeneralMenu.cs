using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayGeneralMenu : MonoBehaviour
{
    [SerializeField] private InputsReader inputsReader;

    [SerializeField] private CanvasGroup canvasGroup;

    void Awake()
    {
        inputsReader.OpenMenu += ShowMenu;

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    private void ShowMenu()
    {
        if (canvasGroup.blocksRaycasts)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }
    }

    private void ShowPanel()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    private void HidePanel()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void ButtonClose()
    {
        HidePanel();
    }

    private void OnDestroy()
    {
        inputsReader.OpenMenu -= ShowMenu;
    }
}
