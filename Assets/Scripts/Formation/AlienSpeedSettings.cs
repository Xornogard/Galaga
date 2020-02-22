using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AlienSpeedSettings
{
	#region MEMBERS
	#pragma warning disable 0649

	[SerializeField]
	private float appearSpeed;

	[SerializeField]
	private float diveInSpeed;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	public float AppearSpeed => appearSpeed;
	public float DiveInSpeed => diveInSpeed;

	#endregion
}
