using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProbabilitySettings
{

	#region MEMBERS

	[SerializeField, Range(0, 100)]
	public float baseProbability;

	[SerializeField]
	public AnimationCurve probabilityChangeOverTime;

	[SerializeField]
	public float probabilityChangeOverTimeMultiplier;

	[SerializeField]
	public float timeToMaximumProbability;

	[SerializeField]
	public float checkInterval;

	[SerializeField]
	public float cooldown;

	#endregion

	#region PROPERTIES

	private float BaseProbability => baseProbability;
	private AnimationCurve ProbabilityChangeOverTime => probabilityChangeOverTime;
	private float ProbabilityChangeOverTimeMultiplier => probabilityChangeOverTimeMultiplier;
	private float TimeToMaximumProbability => timeToMaximumProbability;
	public float CheckInterval => checkInterval;
	public float Cooldown => cooldown;

	#endregion

	#region FUNCTIONS

	public float GetProbability(float time)
	{
		float probability = 0.0f;

		float maximumProbabilityTimeNormalized = TimeToMaximumProbability != 0f ? time / TimeToMaximumProbability : 1.0f;
		maximumProbabilityTimeNormalized = Mathf.Clamp01(maximumProbabilityTimeNormalized);

		probability = BaseProbability + ProbabilityChangeOverTime.Evaluate(maximumProbabilityTimeNormalized) * ProbabilityChangeOverTimeMultiplier;

		return probability;
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
