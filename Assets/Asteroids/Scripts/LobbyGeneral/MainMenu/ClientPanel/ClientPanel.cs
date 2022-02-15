using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientPanel : MonoBehaviour
{
    [SerializeField] private SettingsChannel settinsChannel;

    [SerializeField] private InputField addressInputField;
    [SerializeField] private InputField portInputField;

    [SerializeField] private Button joinButton;


    void Start()
    {
        settinsChannel.networkAddress = "localhost";
        settinsChannel.port = 7777;
        addressInputField.text = settinsChannel.networkAddress;
        portInputField.text = settinsChannel.port.ToString();
        joinButton.interactable = true;


        portInputField.onValueChanged.AddListener(ValidateButton);
        addressInputField.onValueChanged.AddListener(ValidateButton);
    }

    private void ValidateButton(string value)
    {
        if (!string.IsNullOrEmpty(addressInputField.text) && !string.IsNullOrEmpty(portInputField.text))
        {
            joinButton.interactable = true;
        }
        else
        {
            joinButton.interactable = false;
        }
    }
}
