using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyShooting Settings", menuName = "GalagAM/AI/Create new EnemyShooting settings")]
public class EnemyShootingSettings : ScriptableObject
{
	#region MEMBERS

	[SerializeField]
	private ProbabilitySettings probabilitySettings;

	[SerializeField]
	private Weapon weapon;

	#endregion

	#region PROPERTIES

	public ProbabilitySettings ProbabilitySettings => probabilitySettings;
	public IWeapon Weapon => weapon;

	#endregion
}
