using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStageController : MonoBehaviour
{
	#region MEMBERS

	private event Action<AlienEnemy> OnAlienEnemyKilled = delegate { };
	private event Action<int> OnNewStagePrepared = delegate { };
	private event Action<int, int> OnNewWavePrepared = delegate { };
	private event Action OnGameWon = delegate { };

	[SerializeField]
	private List<GameStageSettings> stages;

	[SerializeField]
	private EnemySpawner enemySpawner;

	#endregion

	#region PROPERTIES

	private int CurrentStageNumber { get; set; }
	private int CurrentWaveNumber { get; set; }

	private List<GameStageSettings> Stages => stages;

	private EnemySpawner EnemySpawner => enemySpawner;

	private List<AlienEnemy> CurrentWaveEnemies { get; set; } = new List<AlienEnemy>();

	private List<GameStageWave> CurrentStageWaves { get; set; } = new List<GameStageWave>();

	private GameStageSettings CurrentStage { get; set; }

	private GameStageWave CurrentWave { get; set; }

	#endregion

	#region REGISTER_CALLBACKS

	public void RegisterOnAlienKilled(Action<AlienEnemy> onAlienEnemyKilled)
	{
		OnAlienEnemyKilled += onAlienEnemyKilled;
	}

	public void RegisterOnNewStagePreparedAction(Action<int> onNewStagePrepared)
	{
		OnNewStagePrepared += onNewStagePrepared;
	}

	public void RegisterOnNewWavePreparedAction(Action<int, int> onNewWavePrepared)
	{
		OnNewWavePrepared += onNewWavePrepared;
	}

	public void RegisterOnGameWonAction(Action onGameWon)
	{
		OnGameWon += onGameWon;
	}

	#endregion

	#region FUNCTIONS

	public void StartGame()
	{
		PrepareNextStage();
	}

	public void StartCurrentStage()
	{
		CurrentStageWaves.Clear();
		CurrentStageWaves.AddRange(CurrentStage.GetStageWaves());
		PrepareNewWave();
	}

	public void StartCurrentWave()
	{
		if (CurrentWave != null)
		{
			EnemySpawner.ClearSpawnCoroutines();
			float spawnStartDelay = 0.0f;

			AlienStageWaveSettings[] alienStageWaveSettings = CurrentWave.GetAlienStageSettings();
			for (int i = 0; i < alienStageWaveSettings.Length; i++)
			{
				spawnStartDelay += alienStageWaveSettings[i].SpawnStartDelay;

				EnemySpawner.SpawnAlienEnemies(alienStageWaveSettings[i], HandleNewAlienCreated, spawnStartDelay);
				spawnStartDelay += alienStageWaveSettings[i].AlienSpawnDelay * alienStageWaveSettings[i].AlienCount;
			}
		}
	}

	public int GetOverallWavesCount()
	{
		return CurrentStage != null ? CurrentStage.GetStageWavesCount() : 0;
	}

	public void StopEnemies()
	{
		EnemySpawner.StopSpawning();
		for (int i = 0; i < CurrentWaveEnemies.Count; i++)
		{
			CurrentWaveEnemies[i].SetAlienState(AlienEnemy.AlienStates.None);
		}
	}

	public void RepeatStage()
	{
		CurrentWaveNumber = 0;
		EnemySpawner.StopSpawning();
		CurrentWaveEnemies.DestroyAll();
		
		if(CurrentStage != null)
		{
			StartCurrentStage();
		}
	}

	private void PrepareNextStage()
	{
		CurrentWaveNumber = 0;

		CurrentStage = GetNextStage();

		OnNewStagePrepared(++CurrentStageNumber);
	}

	private GameStageSettings GetNextStage()
	{
		GameStageSettings nextStage = null;

		if(Stages.HasAnyElement() == true)
		{
			nextStage = Stages.PopFirstElement();
		}

		return nextStage;
	}

	private GameStageWave GetNextGameStageWave()
	{
		GameStageWave nextStageWave = null;

		if(CurrentStageWaves.HasAnyElement() == true)
		{
			nextStageWave = CurrentStageWaves.PopFirstElement();
		}

		return nextStageWave;
	}

	private void HandleNewAlienCreated(AlienEnemy newAlien)
	{
		CurrentWaveEnemies.Add(newAlien);

		newAlien.RegisterOnAlienDeathAction(HandleAlienDeath);
	}

	private void HandleAlienDeath(AlienEnemy killedAlien)
	{
		CurrentWaveEnemies.Remove(killedAlien);
		killedAlien.UnregisterOnAlienDeathAction(HandleAlienDeath);

		OnAlienEnemyKilled(killedAlien);

		if (IsAnyAlienLeft() == false)
		{
			if(CurrentStageWaves.HasAnyElement() == true)
			{
				ResetFormationGrid();
				PrepareNewWave();
			}
			else
			{
				HandleStageFinished();
			}
		}
	}

	private bool IsAnyAlienLeft()
	{
		return CurrentWaveEnemies.Count > 0;
	}

	private void PrepareNewWave()
	{
		ResetFormationGrid();
		CurrentWave = GetNextGameStageWave();
		OnNewWavePrepared(++CurrentWaveNumber, GetOverallWavesCount());
	}

	private void ResetFormationGrid()
	{
		EnemySpawner.ResetFormationGrid();
	}

	private void HandleStageFinished()
	{
		if(CurrentStage.HasBoss() == true)
		{
			StartBossFight();
		}
		else if(Stages.HasAnyElement() == false)
		{
			HandleGameWon();
		}
		else
		{
			PrepareNextStage();
		}
	}

	private void StartBossFight()
	{

	}

	private void HandleGameWon()
	{
		OnGameWon();
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
