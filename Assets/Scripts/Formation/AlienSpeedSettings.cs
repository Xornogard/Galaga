using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AlienSpeedSettings
{
	#region MEMBERS

	[SerializeField]
	private float appearSpeed;

	[SerializeField]
	private float diveInSpeed;

	#endregion

	#region PROPERTIES

	public float AppearSpeed => appearSpeed;
	public float DiveInSpeed => diveInSpeed;

	#endregion
}
