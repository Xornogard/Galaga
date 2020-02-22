using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AlienStageWave Settings", menuName = "GalagAM/Settings/Create new AlienStageWave settings")]
public class AlienStageWaveSettings : ScriptableObject
{
	#region MEMBERS
	#pragma warning disable 0649

	[SerializeField]
	private AlienEnemy alienEnemy;

	[SerializeField]
	private int alienCount;

	[SerializeField]
	private float alienSpawnDelay;

	[SerializeField]
	private float spawnStartDelay;

	[SerializeField]
	private AlienFormationSettings formationSettings;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	public AlienEnemy AlienEnemy => alienEnemy;
	public int AlienCount => alienCount;
	public float AlienSpawnDelay => alienSpawnDelay;
	public float SpawnStartDelay => spawnStartDelay;
	public AlienFormationSettings FormationSettings => formationSettings;

	#endregion

	#region FUNCTIONS

	#endregion
}
