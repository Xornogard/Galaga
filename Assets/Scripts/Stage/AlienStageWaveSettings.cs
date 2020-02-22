using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AlienStageWave Settings", menuName = "GalagAM/Settings/Create new AlienStageWave settings")]
public class AlienStageWaveSettings : ScriptableObject
{
	#region MEMBERS

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
