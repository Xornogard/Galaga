using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
	#region MEMBERS
	#pragma warning disable 0649

	[SerializeField]
	private PlayerManager playerManager;

	[SerializeField]
	private GameStageController stageController;

	[SerializeField]
	private MusicManager musicManager;

	[SerializeField]
	private UIManager uiManager;

	#warning TODO: Move to another manager.
	[SerializeField]
	private ProbabilitySettings powerUpProbability;

	[SerializeField]
	private List<PowerUp> availablePowerUps;

	[SerializeField]
	private PowerUpHolder powerUpHolderPrefab;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	private PlayerManager PlayerManager => playerManager;
	private GameStageController StageController => stageController;
	private MusicManager MusicManager => musicManager;
	private UIManager UIManager => uiManager;

	private ProbabilitySettings PowerUpProbability => powerUpProbability;
	private List<PowerUp> AvailablePowerUps => availablePowerUps;
	private PowerUpHolder PowerUpHolderPrefab => powerUpHolderPrefab;

	private GameStates GameState { get; set; }

	private bool WasCancelButtonPressed { get; set; }

	private float GameTime { get; set; }

	private Coroutine PowerUpCoroutine { get; set; }

	#endregion

	#region MONOBEHAVIOUR_CALLBACKS

	private void Update()
	{
		if(GameState == GameStates.InGame)
		{
			GameTime += Time.deltaTime;
		}

		if(Input.GetAxisRaw(Constants.INPUT_CANCEL_AXIS) > 0.0f && WasCancelButtonPressed == false)
		{
			WasCancelButtonPressed = true;
			HandleCancelButtoonPressed();
		}

		if(Input.GetAxisRaw(Constants.INPUT_CANCEL_AXIS) == 0)
		{
			WasCancelButtonPressed = false;
		}
	}

	private void Awake()
	{
		AttachEvents();
		PowerUpCoroutine = StartCoroutine(PowerUpDrawCoroutine());
	}

	private void Start()
	{
		UIManager.ShowStartGameScreen();
		MusicManager.PlayStartMenuMusic();
	}

	#endregion

	#region FUNCTIONS

	public void StartGameWithGodMode()
	{
		StartGame();

		PlayerManager.SetPlayerGodMode(true);
	}

	public void StartGame()
	{
		PlayerManager.SpawnPlayer();
		MusicManager.PlayInGameMusic();

		UIManager.StartCountDown("Defeat Waddlequacks!", 0, () =>
		{
			StageController.StartGame();
			SetGameState(GameStates.InGame);
			UIManager.ShowGameHUD();
		}, false);
	}

	public void PauseGame()
	{
		Time.timeScale = 0.0f;
		UIManager.ShowPauseScreen();
		SetGameState(GameStates.Paused);
	}

	public void UnpauseGame()
	{
		Time.timeScale = 1.0f;
		UIManager.HidePauseScreen();
		SetGameState(GameStates.InGame);
	}

	public void ShowStartGameScreen()
	{
		if(PowerUpCoroutine != null)
		{
			StopCoroutine(PowerUpCoroutine);
		}

		Time.timeScale = 1.0f;
		SceneManager.LoadScene(0);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	private void HandleCancelButtoonPressed()
	{
		switch (GameState)
		{
			case GameStates.InGame:
				PauseGame();
				break;
			case GameStates.Paused:
				UnpauseGame();
				break;
			case GameStates.WinScreen:
				ShowStartGameScreen();
				break;
			case GameStates.GameOver:
				ShowStartGameScreen();
				break;
			default:
				break;
		}
	}

	private void AttachEvents()
	{
		PlayerManager.RegisterOnPlayerDeathAction(HandlePlayerDeath);
		PlayerManager.RegisterOnGameOverAction(HandleGameOver);
		PlayerManager.RegisterOnPlayerSpecialWeaponChargeChangeAction(UIManager.SetSpecialWeaponCharge);
		PlayerManager.RegisterOnPlayerLifeCountChangeAction(UIManager.SetPlayerLifes);

		StageController.RegisterOnGameWonAction(HandleGameWon);
		StageController.RegisterOnNewStagePreparedAction(PrepareForNewStage);
		StageController.RegisterOnNewWavePreparedAction(PrepareForNewWave);
		StageController.RegisterOnAlienKilled(HandlePlayerKilledAlien);
	}

	private void HandlePlayerDeath()
	{
		StageController.StopEnemies();
		UIManager.StartCountDown("Ready for next try?!", 3, RepeatStage);
	}

	private void HandleGameOver()
	{
		MusicManager.PlayGameOverMusic();
		StageController.StopEnemies();
		UIManager.ShowGameOverScreen();
		SetGameState(GameStates.GameOver);
	}

	private void HandleGameWon()
	{
		MusicManager.PlayGameWonMusic();
		Time.timeScale = 0.0f;
		UIManager.ShowGameWonScreen();
		SetGameState(GameStates.WinScreen);
	}

	private void PrepareForNewStage(int stageNumber)
	{
		UIManager.SetWaveNumber(1, StageController.GetOverallWavesCount());
		UIManager.SetStageNumber(stageNumber);

		UIManager.StartCountDown(string.Format("Stage {0}", stageNumber), 3, () =>
		{
			StageController.StartCurrentStage();
			SetGameState(GameStates.InGame);
		}, false);
	}

	private void PrepareForNewWave(int waveNumber, int overallWaveCount)
	{
		UIManager.StartCountDown("Wave is coming!", 0, () =>
		{
			UIManager.SetWaveNumber(waveNumber, overallWaveCount);
			StageController.StartCurrentWave();
			SetGameState(GameStates.InGame);
		});
	}

	private void HandlePlayerKilledAlien(AlienEnemy alienEnemy)
	{
		PlayerManager.AddPlayerKillCount();
		PlayerManager.AddPlayerScore(alienEnemy.ScoreOnKill);
		UIManager.SetPlayerScore(PlayerManager.PlayerScore);
	}

	private void RepeatStage()
	{
		PlayerManager.SpawnPlayer();
		StageController.RepeatStage();
	}

	private void SetGameState(GameStates gameState)
	{
		GameState = gameState;
	}

	private IEnumerator PowerUpDrawCoroutine()
	{
		float powerUpIntervalTime = 0.0f;
		while (true)
		{
			if(GameState == GameStates.InGame)
			{
				powerUpIntervalTime += Time.deltaTime;

				if(powerUpIntervalTime > PowerUpProbability.CheckInterval)
				{
					powerUpIntervalTime = 0.0f;

					bool spawnPowerUp = PowerUpProbability.GetProbability(GameTime) > Random.Range(0,100);

					if(spawnPowerUp == true)
					{
  						Vector3 spawnPosition = EnemySpawner.GetSpawnPosition();
  						spawnPosition.y = Mathf.Max(PlayerManager.PlayerSpawnPoint.y + 2f, spawnPosition.y);

						Vector3 movementDirection = spawnPosition.x > 0 ? Vector3.left : Vector3.right;

						PowerUpHolder powerUpHolder = Instantiate(PowerUpHolderPrefab, spawnPosition, Quaternion.identity);
						powerUpHolder.SetMovementDirection(movementDirection);
						powerUpHolder.SetPowerUp(AvailablePowerUps.PickRandomElement());
						powerUpHolder.OnHit += GivePowerUpToPlayer;

						Destroy(powerUpHolder, 10f);
					}
				}
			}

			yield return new WaitForEndOfFrame();
		}
	}

	private void GivePowerUpToPlayer(IPowerUp powerUp)
	{
		powerUp.GivePowerUp(PlayerManager);
	}

	#endregion

	#region CLASS_ENUMS

	private enum GameStates
	{
		StartMenu,
		InGame,
		Paused,
		WinScreen,
		GameOver
	}

	#endregion
}
