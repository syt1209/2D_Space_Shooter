using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private Sprite[] _livesSprites;

    void Start()
    {
        _scoreText.text = "Score: " + 0;

    }

    public void CurrentScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void CurrentLife(int playerLife)
    {
        _livesImg.sprite = _livesSprites[playerLife];
    }
}
