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

    [SerializeField]
    private Image _thrusterHUD;
 
    private Slider _slider;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15 + "/" + 15;
        _gameOvertxt.gameObject.SetActive(false);
        _restartTxt.gameObject.SetActive(false);
        _slider = _thrusterHUD.GetComponent<Slider>();
        _slider.value = 0;

    }

    public void CurrentScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void CurrentAmmo(int ammoCount)
    {
        _ammoText.text = "Ammo: " + ammoCount.ToString() + "/" + 15;
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

    public void SetSlider(float speed)
    {
        _slider.value = (speed - 3.5f) / 3.5f;
    }
}
