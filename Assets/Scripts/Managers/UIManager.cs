using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
	#region MEMBERS

	private int MAXIMUM_POSSIBLE_SCORE = 99999;

	[SerializeField, Header("Sound FX")]
	private AudioSource audioSource;
	[SerializeField]
	private AudioClip buttonClickAudioClip;
	[SerializeField]
	private AudioClip countDownStartAudioClip;
	[SerializeField]
	private AudioClip countDownAudioClip;
	[SerializeField]
	private AudioClip countDownFinishAudioClip;

	[SerializeField, Header("Game HUD")]
	private Text countDownText;
	[SerializeField]
	private Text playerScore;
	[SerializeField]
	private Text playerLifes;
	[SerializeField]
	private Text stageNumber;
	[SerializeField]
	private Text waveNumber;
	[SerializeField]
	private Image specialWeaponCharge;

	[SerializeField, Header("Panels")]
	private GameObject startMenuPanel;

	[SerializeField]
	private GameObject gameHUDPanel;

	[SerializeField]
	private GameObject gameOverPanel;

	[SerializeField]
	private GameObject gameWonPanel;

	[SerializeField]
	private GameObject pauseGamePanel;

	#endregion

	#region PROPERTIES

	//TODO: Move all that to seperate class
	private AudioSource AudioSource => audioSource;
	private AudioClip ButtonClickAudioClip => buttonClickAudioClip;
	private AudioClip CountDownStartAudioClip => countDownStartAudioClip;
	private AudioClip CountDownAudioClip => countDownAudioClip;
	private AudioClip CountDownFinishAudioClip => countDownFinishAudioClip;

	private GameObject StartMenuPanel => startMenuPanel;
	private GameObject GameHUDPanel => gameHUDPanel;
	private GameObject GameOverPanel => gameOverPanel;
	private GameObject GameWonPanel => gameWonPanel;
	private GameObject PauseGamePanel => pauseGamePanel;


	//TODO: Move all that to seperate class
	private Text CountDownText => countDownText;
	private Text PlayerScore => playerScore;
	private Text PlayerLifes => playerLifes;
	private Text StageNumber => stageNumber;
	private Text WaveNumber => waveNumber;

	private Image SpecialWeaponCharge => specialWeaponCharge;



	#endregion

	#region MONOBEHAVIOUR_CALLBACKS

	private void Awake()
	{
		DisableAllPanels();
	}

	#endregion

	#region FUNCTIONS
	public void ShowPauseScreen()
	{
		PauseGamePanel.SetActive(true);
	}

	public void HidePauseScreen()
	{
		PauseGamePanel.SetActive(false);
	}

	public void ShowStartGameScreen()
	{
		StartMenuPanel.SetActive(true);
	}

	public void ShowGameHUD()
	{ 
		GameHUDPanel.SetActive(true);
	}

	public void ShowGameOverScreen()
	{
		DisableAllPanels();
		GameOverPanel.SetActive(true);
	}

	public void ShowGameWonScreen()
	{
		DisableAllPanels();
		GameWonPanel.SetActive(true);
	}

	public void SetPlayerScore(int playerScore)
	{
		PlayerScore.text = (Mathf.Min(playerScore, MAXIMUM_POSSIBLE_SCORE)).ToString();
	}

	public void SetStageNumber(int stageNumber)
	{
		StageNumber.text = string.Format("Stage: {0}", stageNumber);
	}

	public void SetWaveNumber(int waveNumber, int overallWaveCount)
	{
		WaveNumber.text = string.Format("Wave: {0}/{1}", waveNumber, overallWaveCount);
	}

	public void SetPlayerLifes(int playerLifes)
	{
		PlayerLifes.text = string.Format("Life: {0}", playerLifes);
	}

	public void SetSpecialWeaponCharge(float specialWeaponCharge)
	{
		SpecialWeaponCharge.fillAmount = specialWeaponCharge;
	}

	public void PlayButtonClickSound()
	{
		AudioSource.PlayOneShot(ButtonClickAudioClip);
	}

	public void StartCountDown(string countDownStartLabel, int countDownDuration, Action onCountDownFinishCallback, bool playSoundOnEnd = true)
	{
		StartCoroutine(CountDownCoroutine(countDownStartLabel, countDownDuration, onCountDownFinishCallback, playSoundOnEnd));
	}

	private IEnumerator CountDownCoroutine(string countDownStartLabel, int countDownDuration, Action onCountDownFinishCallback, bool playSoundOnEnd)
	{
		CountDownText.gameObject.SetActive(true);
		AnimateCountDownText(countDownStartLabel);

		AudioSource.PlayOneShot(CountDownStartAudioClip);
		yield return new WaitForSeconds(1.0f);

		while (countDownDuration > 0)
		{
			AnimateCountDownText(countDownDuration);
			AudioSource.PlayOneShot(CountDownAudioClip);
			yield return new WaitForSeconds(1.0f);
			countDownDuration--;
		}

		if(playSoundOnEnd == true)
		{
			AudioSource.PlayOneShot(CountDownFinishAudioClip);
		}

		CountDownText.gameObject.SetActive(false);

		onCountDownFinishCallback();
	}

	private void AnimateCountDownText<T>(T label, float duration = 0.5f)
	{
		CountDownText.transform.localScale = Vector3.zero;
		CountDownText.text = label.ToString();
		CountDownText.transform.DOScale(Vector3.one, duration);
	}

    private void SetWavesCount(int wavesCount)
    {
        throw new NotImplementedException();
    }

    private void DisableAllPanels()
	{
		StartMenuPanel.SetActive(false);
		GameHUDPanel.SetActive(false);
		GameOverPanel.SetActive(false);
		GameWonPanel.SetActive(false);
		PauseGamePanel.SetActive(false);
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
