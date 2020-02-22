using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienEnemy : MonoBehaviour, IDamageable, IKillable
{
	#region MEMBERS

	private event Action<AlienEnemy> OnAlienDeath = delegate { };

	[SerializeField]
	private int health;

	[SerializeField]
	private int scoreOnKill;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip deathAudioClip;

	[SerializeField]
	private AudioClip damageAudioClip;

	[SerializeField]
	private AlienSpeedSettings speedSettings;

	[SerializeField]
	private EnemyDiveInSettings diveInSettings;

	[SerializeField]
	private EnemyShootingSettings shootingSettings;

	[SerializeField]
	private Animator animator;

	#endregion

	#region PROPERTIES

	public int ScoreOnKill => scoreOnKill;

	private int Health {
		get => health;
		set => health = value;
	}

	private AudioSource AudioSource => audioSource;
	private AudioClip DeathAudioClip => deathAudioClip;
	private AudioClip DamageAudioClip => damageAudioClip;

	private AlienSpeedSettings SpeedSettings => speedSettings;
	private EnemyDiveInSettings DiveInSettings => diveInSettings;
	private EnemyShootingSettings ShootingSettings => shootingSettings;

	private Animator Animator => animator;

	private AlienStates AlienState { get; set; } = AlienStates.None;
	private Vector2 FormationPosition { get; set; }
	private float CurrentMovementSpeed { get; set; }
	private float LastDiveInTime { get; set; }
	private float LastShootTime { get; set; }
	private float CurrentLivingTime { get; set; }
	private Coroutine DiveInCheckCoroutine { get; set; }
	private List<Vector3> SampledPath { get; set; } = null;


	#endregion

	#region MONOBEHAVIOUR_CALLBACKS

	private void Start()
	{
		if(ShootingSettings != null)
		{
			StartCoroutine(WeaponShootCheck());
		}
	}

	private void Update()
	{
		CurrentLivingTime += Time.deltaTime;
		UpdateState();
	}

	private void OnDrawGizmosSelected()
	{
		Path path = GetDiveInPath();

		List<Vector3> sampledPath = Application.isPlaying == true ? SampledPath : path.SamplePath();

		Gizmos.color = Color.cyan;
		for (int i = 0; i + 1 < sampledPath.Count; i++)
		{
			Gizmos.DrawLine(sampledPath[i], sampledPath[i + 1]);
		}

		Gizmos.color = Color.red;
		for (int i = 0; i < sampledPath.Count; i++)
		{
			Gizmos.DrawSphere(sampledPath[i], 0.04f);
		}

	}

	#endregion

	#region REGISTER_CALLBACKS

	public void RegisterOnAlienDeathAction(Action<AlienEnemy> onAlienDeath)
	{
		OnAlienDeath += onAlienDeath;
	}

	public void UnregisterOnAlienDeathAction(Action<AlienEnemy> onAlienDeath)
	{
		OnAlienDeath -= onAlienDeath;
	}

	#endregion

	#region FUNCTIONS

	public void Kill()
	{
		if (Animator != null)
		{
			Animator.SetTrigger(Constants.ANIMATION_DEATH_TRIGGER);
		}
		else
		{
			Destroy();
		}

		OnAlienDeath(this);
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}

	public void Damage()
	{
		Health--;

		if(Health <= 0)
		{
			SetAlienState(AlienStates.Death);
			AudioSource.PlayOneShot(DeathAudioClip);
		}
		else
		{
			AudioSource.PlayOneShot(DamageAudioClip);
		}
	}

	public void SetAlienState(AlienStates alienState)
	{
		if (AlienState != alienState)
		{
			AlienState = alienState;
			HandleStateChange();
		}
	}

	public void SetFormationPosition(Vector2 formationPosition)
	{
		FormationPosition = formationPosition;
	}

	private void UpdateState()
	{
		switch (AlienState)
		{
			case AlienStates.Appear:
				UpdateAppearState();
				break;
			case AlienStates.DiveIn:
				UpdateDiveInState();
				break;
		}
	}

	private void UpdateAppearState()
	{
		UpdatePath(() => SetAlienState(AlienStates.InFormation));
	}

	private void UpdateDiveInState()
	{
		UpdatePath(() => StartCoroutine(ReapperDeffered()));
	}

	private void UpdatePath(Action onPathFinishedCallback)
	{
		bool isFlying = SampledPath.HasAnyElement() == true;

		Animator?.SetBool(Constants.ANIMATION_IS_FLYING_BOOL, isFlying == false);

		if (isFlying == true)
		{
			transform.MoveAlongPath(SampledPath, CurrentMovementSpeed);
		}
		else
		{
			onPathFinishedCallback();
		}
	}

	private void Reappear()
	{
		transform.position = EnemySpawner.GetSpawnPosition();
		SetAlienState(AlienStates.Appear);
	}

	private void HandleStateChange()
	{
		switch (AlienState)
		{
			case AlienStates.Appear:
				Appear();
				break;
			case AlienStates.InFormation:
				HandleStateChangedToInFormation();
				break;
			case AlienStates.DiveIn:
				DiveIn();
				break;
			case AlienStates.Death:
				Kill();
				break;
		}
	}

	private void Appear()
	{
		CurrentMovementSpeed = SpeedSettings.AppearSpeed;
		SampledPath = GetAppearPath().SamplePath();
	}

	private void HandleStateChangedToInFormation()
	{
		if (DiveInCheckCoroutine == null)
		{
			DiveInCheckCoroutine = StartCoroutine(DiveInCheck());
		}
		transform.LookAtDirection2D(Vector2.down);
	}

	private void DiveIn()
	{
		CurrentMovementSpeed = SpeedSettings.DiveInSpeed;
		SampledPath = GetDiveInPath().SamplePath();
		AudioSource.PlayOneShot(DiveInSettings.DiveInAudioClip);
	}

	private IEnumerator DiveInCheck()
	{
		while(AlienState == AlienStates.InFormation)
		{
			if(ShouldDiveIn() == true)
			{
				SetAlienState(AlienStates.DiveIn);
				LastDiveInTime = Time.time;
			}

			yield return new WaitForSeconds(DiveInSettings.ProbabilitySettings.CheckInterval);
		}

		DiveInCheckCoroutine = null;
	}

	private IEnumerator WeaponShootCheck()
	{
		while (Health > 0 && ShootingSettings != null)
		{
			if (ShouldShoot() == true && ShootingSettings.Weapon.CanShoot(LastShootTime))
			{
				ShootingSettings.Weapon.Shoot(transform.position, Vector2.down, gameObject.layer);
				LastShootTime = Time.time;
			}

			yield return new WaitForSeconds(ShootingSettings.ProbabilitySettings.CheckInterval);
		}
	}

	private bool ShouldDiveIn()
	{
		return DoesMeetsProbabilitySettings(DiveInSettings.ProbabilitySettings, LastDiveInTime);
	}

	private bool ShouldShoot()
	{
		return ShootingSettings != null && (AlienState == AlienStates.InFormation || AlienState == AlienStates.DiveIn) && DoesMeetsProbabilitySettings(ShootingSettings.ProbabilitySettings, LastShootTime);
	}

	private bool DoesMeetsProbabilitySettings(ProbabilitySettings probabilitySettings, float previousConditionsMetTime)
	{
		return Time.time - previousConditionsMetTime > probabilitySettings.Cooldown && Random.Range(0, 100) < probabilitySettings.GetProbability(CurrentLivingTime);
	}

	private IEnumerator ReapperDeffered()
	{
		yield return new WaitForSeconds(DiveInSettings.ReappearCooldown);
		Reappear();
	}

	private Path GetAppearPath()
	{
		return PathFactory.CreateAppearPath(transform.position, FormationPosition);
	}

	private Path GetDiveInPath()
	{
		return PathFactory.CreateDiveInPath(transform.position, DiveInSettings.Amplitude);
	}

	#endregion

	#region CLASS_ENUMS

	public enum AlienStates
	{
		None,
		Appear,
		InFormation,
		DiveIn,
		Death
	}

	#endregion
}
