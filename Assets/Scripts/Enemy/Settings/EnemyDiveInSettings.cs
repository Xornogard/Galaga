using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DiveIn Settings", menuName ="GalagAM/AI/Create new DiveIn settings")]
public class EnemyDiveInSettings : ScriptableObject
{
	#region MEMBERS

	[SerializeField]
	private ProbabilitySettings probabilitySettings;

	[SerializeField, Range(0, 1.5f)]
	private float amplitude;

	[SerializeField]
	private float reappearCooldown;

	[SerializeField]
	private AudioClip diveInAudioClip;

	#endregion

	#region PROPERTIES

	public float Amplitude => amplitude;
	public float ReappearCooldown => reappearCooldown;
	public AudioClip DiveInAudioClip => diveInAudioClip;

	public ProbabilitySettings ProbabilitySettings => probabilitySettings;

	#endregion
}