using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AlienFormationSettings
{
	#region MEMBERS

	[SerializeField]
	private int rowsCount;

	#endregion

	#region PROPERTIES

	public int RowsCount => rowsCount;

	#endregion
}
