using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class PowerUpHolder : MonoBehaviour, IDamageable
{
	#region MEMBERS
	#pragma warning disable 0649

	public Action<IPowerUp> OnHit = delegate { };

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private float speed;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	private AudioSource AudioSource => audioSource;
	private float Speed => speed;
	public IPowerUp PowerUp { get; set; } 
	private Vector3 MovementDirection { get; set; }

	#endregion

	#region MONOBEHAVIOUR_CALLBACKS

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, transform.position + MovementDirection, Speed * Time.deltaTime);
	}

	#endregion

	#region FUNCTIONS

	public void SetPowerUp(IPowerUp powerUp)
	{
		PowerUp = powerUp;
	}

	public void SetMovementDirection(Vector3 movementDirection)
	{
		MovementDirection = movementDirection;
	}

	public void Damage()
	{
		OnHit(PowerUp);

		this.AudioSource.Play();
		this.AudioSource.transform.parent = null;
		Destroy(this.AudioSource.gameObject, 0.5f);
		Destroy(gameObject);
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
