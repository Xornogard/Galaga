using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
	#region MEMBERS

	[SerializeField]
	private bool dontDestroyOnHit;

	[SerializeField]
	private bool canHitMultipleTargets;

	[SerializeField]
	private Collider2D projectileCollider;

	[SerializeField]
	private Animator animator;

	#endregion

	#region PROPERTIES

	private bool DontDestroyOnHit => dontDestroyOnHit;

	private bool CanHitMultipleTargets => canHitMultipleTargets;

	private Collider2D ProjectileCollider => projectileCollider;

	private Animator Animator => animator;

	private float Speed { get; set; }


	private bool AlreadyHitTarget { get; set; }

	#endregion

	#region MONOBEHAVIOUR_CALLBACKS

	protected virtual void Update()
	{
		UpdateMovement();
	}

	private void Reset()
	{
		animator = GetComponent<Animator>();
		projectileCollider = GetComponent<Collider2D>();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, transform.position + transform.up);
	}

	#endregion

	#region FUNCTIONS

	public void SetSpeed(float speed)
	{
		Speed = speed;
	}

	public void DestroyProjectile()
	{
		Destroy(gameObject);
	}

	protected void Hit(IDamageable damageableObject)
	{
 		damageableObject.Damage();
		Animator.SetTrigger(Constants.ANIMATION_HIT_TRIGGER);

		if(DontDestroyOnHit == false)
		{
			Debug.Log("Boom");
			ProjectileCollider.enabled = false;
			Speed = 0f;
		}
	}

	protected virtual void UpdateMovement()
	{
		transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, Speed * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(AlreadyHitTarget == false || CanHitMultipleTargets == true)
		{
			IDamageable damageable = collision.GetInterface<IDamageable>();

			if (damageable != null)
			{
				Hit(damageable);
			}
		}
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
