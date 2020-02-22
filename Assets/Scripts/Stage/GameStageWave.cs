using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStageWave
{
	#region MEMBERS
	#pragma warning disable 0649

	[SerializeField]
	private List<AlienStageWaveSettings> alienSettings;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	private List<AlienStageWaveSettings> AlienSettings => alienSettings;

	#endregion

	#region FUNCTIONS

	public AlienStageWaveSettings[] GetAlienStageSettings()
	{
		return AlienSettings.ToArray();
	}

	#endregion
}
