using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), CreateAssetMenu(fileName = "New Weapon", menuName = "GalagAM/Weapons/Create Weapon")]
public class Weapon : ScriptableObject, IWeapon
{
	#region MEMBERS
	#pragma warning disable 0649 

	[SerializeField]
	private AudioClip shotAudioClip;

	[SerializeField]
	private Projectile projectile;

	[SerializeField]
	private float shotCooldown;

	[SerializeField]
	private float shotSpeed;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	private AudioClip ShootAudioClip => shotAudioClip;
	private Projectile Projectile => projectile;
	private float ShootCooldown => shotCooldown;
	private float ShotSpeed => shotSpeed;

	#endregion

	#region FUNCTIONS

	public virtual void Shoot(Vector2 origin, Vector2 direction, LayerMask projectileLayerMask)
	{
		Projectile newProjectile = SpawnProjectile(origin, direction);
		newProjectile.SetSpeed(ShotSpeed);
		newProjectile.gameObject.layer = projectileLayerMask.value;
		Destroy(newProjectile.gameObject, 10f);
	}

	public bool CanShoot(float previousShotTime)
	{
		return previousShotTime == 0f || Time.time - previousShotTime > ShootCooldown;
	}

	public AudioClip GetShootAudioClip()
	{
		return ShootAudioClip;
	}

	protected Projectile SpawnProjectile(Vector2 origin, Vector2 direction)
	{
		Projectile newProjectile;

		newProjectile = GameObject.Instantiate(Projectile, origin, Quaternion.identity);
		newProjectile.transform.LookAtDirection2D(direction);

		return newProjectile;
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
