using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource), typeof(Animator))]
public abstract class Spaceship : MonoBehaviour, IDamageable, IKillable
{
    #region MEMBERS
    #pragma warning disable 0649

    [SerializeField]
    private bool godMode;

    [SerializeField]
    private bool useSpecialWeapon;

    [SerializeField]
    private int health;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip deathAudioClip;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Vector2 shootPositionOffset;

    [SerializeField]
    private Weapon weapon;

    [SerializeField]
    private Weapon specialWeapon;

	[SerializeField]
	private new Rigidbody2D rigidbody;

    [SerializeField]
    private float speed = 10f;

    #pragma warning restore 0649
    #endregion

    #region PROPERTIES

    private bool UseSpecialWeapon {
        get => useSpecialWeapon;
        set => useSpecialWeapon = value;
    }

    protected bool GodMode {
        get => godMode;
        set => godMode = value;
    }

    protected int Health {
        get => health;
        set => health = value;
    }

    protected Animator Animator => animator;
    protected Rigidbody2D Rigidbody => rigidbody;
    protected float Speed => speed;
    private AudioSource AudioSource => audioSource;
    private AudioClip DeathAudioClip => deathAudioClip;
    private Vector2 ShootPositionOffset => shootPositionOffset;
    private IWeapon Weapon => weapon;
    protected float PreviousShootTime { get; set; } = 0f;
    protected bool IsDead { get; set; }

    #endregion

    #region MONOBEHAVIOUR_CALLBACKS

    protected virtual void Update()
    {
        if(IsDead == false)
        {
            IWeapon currentWeapon = GetCurrentWeapon();

            if (ShouldShoot() == true && currentWeapon.CanShoot(PreviousShootTime) == true)
            {
                Shoot(currentWeapon);
            }

            UpdateSize();
        }
    }

    protected virtual void Reset()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(GetShootPosition(), Vector3.one * 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }

    #endregion

    #region FUNCTIONS

    public void SetUseSpecialWeapon(bool useSpecialWeapon)
    {
        UseSpecialWeapon = useSpecialWeapon;
    }

    public virtual void Kill()
    {
        if(GodMode == false)
        {
            Animator.SetTrigger(Constants.ANIMATION_DEATH_TRIGGER);
            AudioSource.PlayOneShot(DeathAudioClip);
            IsDead = true;
        }
    }

    public virtual void Damage()
    {
        Health--;

        if(Health <= 0)
        {
            Kill();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject, DeathAudioClip.length);
    }

    protected abstract Vector2 GetInput();
    protected abstract bool ShouldShoot();

    protected virtual void Shoot(IWeapon weapon)
    {
        if(Weapon != null)
        {
            weapon.Shoot(transform.position, transform.up, gameObject.layer);
            AudioSource?.PlayOneShot(weapon.GetShootAudioClip());

            PreviousShootTime = Time.time;
        }
    }

    private IWeapon GetCurrentWeapon()
    {
        return UseSpecialWeapon == true ? specialWeapon : Weapon;
    }

    private void UpdateSize()
    {
        transform.localScale = GetSize();
    }

    private Vector2 GetShootPosition()
    {
        Vector2 shootPosition = transform.position;

        shootPosition += ShootPositionOffset;

        return shootPosition;
    }

    private Vector2 GetSize()
    {
        return Vector2.Lerp(Vector2.one, new Vector2(0.85f, 1.0f), Mathf.Abs(GetInput().x));
    }

    #endregion

    #region CLASS_ENUMS

    #endregion
}
