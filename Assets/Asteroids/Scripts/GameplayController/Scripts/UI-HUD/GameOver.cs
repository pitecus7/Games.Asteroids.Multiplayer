using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text winnerNicknameText;
    [SerializeField] private Text winnerPointsText;

    [SerializeField] private Text bigText;

    public void ShowPanel(string winnerNickname, string points, bool isWinner)
    {
        gameObject.SetActive(true);
        winnerNicknameText.text = winnerNickname;
        winnerPointsText.text = points;

        if (isWinner)
        {
            bigText.text = "You Win.";
        }
        else
        {
            bigText.text = "You Lose.";
        }
    }

    public void ShowSoloPanel(string points)
    {
        gameObject.SetActive(true);
        winnerNicknameText.text = "";
        winnerPointsText.text = points;

        bigText.text = "Game Over";

    }

    // Update is called once per frame
    void Update()
    {
        /*if (gameObject.activeInHierarchy && Input.GetKey(KeyCode.Return))
        {
            GameplayController.Instance.BackToLobby();
        }*/
    }
}
