using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private Text _gameOvertxt;

    [SerializeField]
    private Text _restartTxt;

    [SerializeField]
    private GameManager _gameManager;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15;
        _gameOvertxt.gameObject.SetActive(false);
        _restartTxt.gameObject.SetActive(false);

    }

    public void CurrentScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void CurrentAmmo(int ammoCount)
    {
        _ammoText.text = "Ammo: " + ammoCount.ToString();
    }

    public void CurrentLife(int playerLife)
    {
        _livesImg.sprite = _livesSprites[playerLife];

        if (playerLife < 1)
        {
            _gameManager.GameOver();
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameOvertxt.gameObject.SetActive(true);
        _restartTxt.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());

        
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
