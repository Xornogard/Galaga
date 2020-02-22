using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyShooting Settings", menuName = "GalagAM/AI/Create new EnemyShooting settings")]
public class EnemyShootingSettings : ScriptableObject
{
	#region MEMBERS
	#pragma warning disable 0649

	[SerializeField]
	private ProbabilitySettings probabilitySettings;

	[SerializeField]
	private Weapon weapon;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	public ProbabilitySettings ProbabilitySettings => probabilitySettings;
	public IWeapon Weapon => weapon;

	#endregion
}
