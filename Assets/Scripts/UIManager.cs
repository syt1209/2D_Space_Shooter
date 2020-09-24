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

    [SerializeField]
    private Text _gameOvertxt;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOvertxt.gameObject.SetActive(false);

    }

    public void CurrentScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void CurrentLife(int playerLife)
    {
        _livesImg.sprite = _livesSprites[playerLife];

        if (playerLife < 1)
        {
            _gameOvertxt.gameObject.SetActive(true);

            StartCoroutine(GameOverFlickerRoutine());
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true) 
        {
            _gameOvertxt.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOvertxt.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
