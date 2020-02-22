using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Stage settings", menuName = "GalagAM/Settings/Create new stage settings")]
public class GameStageSettings : ScriptableObject
{
	#region MEMBERS
	#pragma warning disable 0649

	[SerializeField]
	private List<GameStageWave> alienStageWaves;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	private List<GameStageWave> AlienStageWaves => alienStageWaves;

	#endregion

	#region FUNCTIONS

	public IEnumerable<GameStageWave> GetStageWaves()
	{
		for (int i = 0; i < AlienStageWaves.Count; i++)
		{
			yield return AlienStageWaves[i];
		}
	}

	public bool HasBoss()
	{
		return false;
	}

    public int GetStageWavesCount()
    {
		return AlienStageWaves.Count;
    }

    #endregion

    #region CLASS_ENUMS

    #endregion
}
