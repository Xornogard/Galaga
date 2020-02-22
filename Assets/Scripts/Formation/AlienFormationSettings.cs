using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AlienFormationSettings
{
	#region MEMBERS
	#pragma warning disable 0649
	
	[SerializeField]
	private int rowsCount;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	public int RowsCount => rowsCount;

	#endregion
}
