using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Stage settings", menuName = "GalagAM/Settings/Create new stage settings")]
public class GameStageSettings : ScriptableObject
{
	#region MEMBERS

	[SerializeField]
	private List<GameStageWave> alienStageWaves;

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
