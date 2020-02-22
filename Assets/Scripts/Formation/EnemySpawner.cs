using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
	#region MEMBERS
	#pragma warning disable 0649

	[SerializeField]
	private FormationGrid formationGrid;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	private static Camera MainCamera { get; set; }

	private FormationGrid FormationGrid => formationGrid;

	private List<Coroutine> SpawnCoroutines { get; set; } = new List<Coroutine>();

	#endregion

	#region MONOBEHAVIOUR_CALLBACKS

	private void Awake()
	{
		MainCamera = Camera.main;
	}

	#endregion

	#region FUNCTIONS

	public static Vector3 GetSpawnPosition()
	{
		Vector3 spawnPosition;

		spawnPosition = MainCamera.ScreenToWorldPoint(new Vector3(0.0f, Random.Range(0.0f, MainCamera.pixelHeight)));
		spawnPosition += Vector3.left;
		spawnPosition.z = 0.0f;

		return spawnPosition;
	}

	public void ResetFormationGrid()
	{
		FormationGrid.ResetFormationGrid();
	}

	public void SpawnAlienEnemies(AlienStageWaveSettings alienStageSettings, Action<AlienEnemy> onAlienCreatedAction, float spawnStartDelay)
	{
		SpawnCoroutines.Add(StartCoroutine(SpawnEnemiesDeffered(alienStageSettings, onAlienCreatedAction, spawnStartDelay)));
	}

	public void StopSpawning()
	{
		for (int i = 0; i < SpawnCoroutines.Count; i++)
		{
			StopCoroutine(SpawnCoroutines[i]);
		}

		ClearSpawnCoroutines();
	}

	public void ClearSpawnCoroutines()
	{
		SpawnCoroutines.Clear();
	}

	private IEnumerator SpawnEnemiesDeffered(AlienStageWaveSettings alienStageSettings, Action<AlienEnemy> onAlienCreatedAction, float spawnStartDelay)
	{
		int firstRowIndex = FormationGrid.CurrentRowIndex;
		FormationGrid.ReserveRows(alienStageSettings.FormationSettings.RowsCount);

		yield return new WaitForSeconds(spawnStartDelay);

		Vector3 spawnPosition = GetSpawnPosition();

		for (int i = 0; i < alienStageSettings.AlienCount; i++)
		{
			Vector3 formationPosition = FormationGrid.EstimatePosition(i, firstRowIndex, alienStageSettings.FormationSettings.RowsCount);
			Vector3 currentSpawnPosition = ShouldInverseSpawnPosition(i) == true ? spawnPosition.InverseX() : spawnPosition;
			
			AlienEnemy newAlien = CreateAlienEnemy(alienStageSettings.AlienEnemy, currentSpawnPosition);

			newAlien.SetFormationPosition(formationPosition);
			newAlien.SetAlienState(AlienEnemy.AlienStates.Appear);

			onAlienCreatedAction(newAlien);
			if (i % 2 != 0)
			{
				yield return new WaitForSeconds(alienStageSettings.AlienSpawnDelay);
			}
		}
	}

	private AlienEnemy CreateAlienEnemy(AlienEnemy alienEnemyToSpawn, Vector3 spawnPosition)
	{
		AlienEnemy alienEnemy;

		alienEnemy = Instantiate(alienEnemyToSpawn, spawnPosition, Quaternion.identity, transform);

		return alienEnemy;
	}

	private bool ShouldInverseSpawnPosition(int index)
	{
		return index % 2 != 0;
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
