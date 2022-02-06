using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNickname : MonoBehaviour
{
    [SerializeField] private TextMesh nicknameTextMesh;

    [SerializeField] private bool hideText;

    private float alphaCurrentValue;

    private void Start()
    {
        nicknameTextMesh = GetComponent<TextMesh>();
        alphaCurrentValue = nicknameTextMesh.color.a;
    }

    void Update()
    {
        if (hideText)
        {
            nicknameTextMesh.color = new Color(1, 1, 1, alphaCurrentValue);
            alphaCurrentValue -= Time.deltaTime / 2;

            if (alphaCurrentValue <= 0.05f)
            {
                gameObject.SetActive(false);
                hideText = false;
            }
        }
    }

    public void ShowNickname(string nickname)
    {
        alphaCurrentValue = 1;
        nicknameTextMesh.text = nickname;
        gameObject.SetActive(true);
    }

    public void HideNickname()
    {
        hideText = true;
    }
}
