using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStageWave
{
	#region MEMBERS

	[SerializeField]
	private List<AlienStageWaveSettings> alienSettings;

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
