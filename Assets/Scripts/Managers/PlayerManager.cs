using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	#region MEMBERS
	#pragma warning disable 0649

	private event Action OnPlayerDeath = delegate { };
	private event Action OnPlayerGameOver = delegate { };
	private event Action<float> OnPlayerSpecialWeaponChargeChange = delegate { };
	private event Action<int> OnPlayerLifeCountChange = delegate { };

	[SerializeField]
	private int playerLifesCount;

	[SerializeField]
	private int killCountForSpecialWeapon;

	[SerializeField]
	private float specialWeaponDuration;

	[SerializeField]
	private PlayerSpaceship playerPrefab;

	[SerializeField]
	private Vector2 playerSpawnPoint;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	public int PlayerLifesCount {
		get => playerLifesCount;
		private set => playerLifesCount = value;
	}
	public Vector2 PlayerSpawnPoint => playerSpawnPoint;

	public int PlayerScore { get; private set; }
	private float SpecialWeaponDuration => specialWeaponDuration;
	private int KillCountForSpecialWeapon => killCountForSpecialWeapon;

	private PlayerSpaceship PlayerPrefab => playerPrefab;
	private int KillCount { get; set; }

	private PlayerSpaceship CurrentPlayerSpaceship {
		get;
		set;
	}

	private Coroutine SpecialWeaponUseCoroutine {
		get;
		set;
	}

	#endregion

	#region MONOBEHAVIOUR_CALLBACKS

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawCube(PlayerSpawnPoint, Vector3.one * 0.3f);
	}

    public void SetPlayerGodMode(bool godMode)
    {
		killCountForSpecialWeapon = int.MaxValue;
		CurrentPlayerSpaceship.SetGodMode(true);
		CurrentPlayerSpaceship.SetUseSpecialWeapon(true);
	}

	#endregion

	#region REGISTER_CALLBACKS

	public void RegisterOnPlayerDeathAction(Action onPlayerDeath)
	{
		OnPlayerDeath += onPlayerDeath;
	}

	public void RegisterOnGameOverAction(Action onGameOver)
	{
		OnPlayerGameOver += onGameOver;
	}

	public void RegisterOnPlayerSpecialWeaponChargeChangeAction(Action<float> onPlayerSpecialWeaponChargeChange)
	{
		OnPlayerSpecialWeaponChargeChange += onPlayerSpecialWeaponChargeChange;
	}

	public void RegisterOnPlayerLifeCountChangeAction(Action<int> onPlayerLifeCountChange)
	{
		OnPlayerLifeCountChange += onPlayerLifeCountChange;
	}

	#endregion

	#region FUNCTIONS

	public void ForceSpecialWeapon()
	{
		if(SpecialWeaponUseCoroutine != null)
		{
			StopCoroutine(SpecialWeaponCoroutine());
		}


		KillCount = KillCountForSpecialWeapon;
		CheckForSpecialWeapon();
	}

	public void AddLife()
	{
		PlayerLifesCount++;
		OnPlayerLifeCountChange(PlayerLifesCount);
	}

	public void AddPlayerKillCount()
	{
		if(SpecialWeaponUseCoroutine == null)
		{
			KillCount++;
			CheckForSpecialWeapon();
		}
	}

	public void ResetPlayerKillCount()
	{
		KillCount = 0;
		CheckForSpecialWeapon();
	}

	public void SpawnPlayer()
	{
		CurrentPlayerSpaceship = Instantiate(PlayerPrefab, PlayerSpawnPoint, Quaternion.identity);
		CurrentPlayerSpaceship.RegisterOnPlayerDeath(HandlePlayerDeath);
	}

	private void CheckForSpecialWeapon()
	{
		if(KillCountForSpecialWeapon != 0)
		{
			if (KillCount != 0 && KillCount % KillCountForSpecialWeapon == 0)
			{
				if(SpecialWeaponUseCoroutine != null)
				{
					StopCoroutine(SpecialWeaponUseCoroutine);
				}

				SpecialWeaponUseCoroutine = StartCoroutine(SpecialWeaponCoroutine());
			}
			else
			{
				OnPlayerSpecialWeaponChargeChange((float) KillCount / KillCountForSpecialWeapon);
			}
		}
	}

	private void HandlePlayerDeath()
	{
		CurrentPlayerSpaceship.UnregisterOnPlayerDeath(HandlePlayerDeath);
		PlayerLifesCount--;
		OnPlayerLifeCountChange(PlayerLifesCount);
		ResetPlayerKillCount();
		if(SpecialWeaponUseCoroutine != null)
		{
			StopCoroutine(SpecialWeaponUseCoroutine);
		}

		if (PlayerLifesCount > 0)
		{
			OnPlayerDeath();
		}
		else
		{
			OnPlayerGameOver();
		}
	}

	public void AddPlayerScore(int scoreOnKill)
	{
		PlayerScore += scoreOnKill;
	}

	private IEnumerator SpecialWeaponCoroutine()
	{
		CurrentPlayerSpaceship.SetUseSpecialWeapon(true);
		float specialWeaponUseTime = 0.0f;
		while(specialWeaponUseTime < SpecialWeaponDuration)
		{
			yield return new WaitForEndOfFrame();
			specialWeaponUseTime += Time.deltaTime;
			OnPlayerSpecialWeaponChargeChange(1.0f - specialWeaponUseTime / SpecialWeaponDuration);
		}

		SpecialWeaponUseCoroutine = null;
		ResetPlayerKillCount();
		CurrentPlayerSpaceship.SetUseSpecialWeapon(false);
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
