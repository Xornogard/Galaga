using System;
using UnityEngine;

public class PlayerSpaceship : Spaceship
{
    #region MEMBERS
    #pragma warning disable 0649

    private event Action OnPlayerDeath = delegate { };

    [SerializeField]
    private float widthBoundaries;

    #pragma warning restore 0649
    #endregion

    #region PROPERTIES

    private float WidthBoundaries => widthBoundaries;

    #endregion

    #region MONOBEHAVIOUR_CALLBACKS

    protected override void Update()
    {
        base.Update();

        if(IsWithinBoundaries() == false)
        {
            AdjustToBoundaries();
        }
    }

    private void FixedUpdate()
    {
        Rigidbody.velocity = GetInput() * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IKillable killable = collision.GetInterface<IKillable>();

        if(killable != null)
        {
            killable.Kill();
        }

        Kill();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Vector2 leftBoundaries = new Vector2(-widthBoundaries, transform.position.y);
        Vector2 rightBoundaries = new Vector2(widthBoundaries, transform.position.y);

        Gizmos.color = Color.red;

        float boundariesHeight = 0.5f;
        Gizmos.DrawLine(leftBoundaries + Vector2.down * boundariesHeight*0.5f, leftBoundaries + Vector2.up* boundariesHeight * 0.5f);
        Gizmos.DrawLine(rightBoundaries + Vector2.down * boundariesHeight * 0.5f, rightBoundaries + Vector2.up * boundariesHeight * 0.5f);
    }

    #endregion

    #region REGISTER_CALLBACKS

    public void RegisterOnPlayerDeath(Action onPlayerDeathAction)
    {
        OnPlayerDeath += onPlayerDeathAction;
    }

    public void UnregisterOnPlayerDeath(Action onPlayerDeathAction)
    {
        OnPlayerDeath -= onPlayerDeathAction;
    }

    #endregion

    #region FUNCTIONS


    public void SetGodMode(bool godMode)
    {
        GodMode = godMode;
    }

    public override void Kill()
    {
        base.Kill();

        if(GodMode == false)
        {
            OnPlayerDeath();
        }
    }

    protected override Vector2 GetInput()
    {
        Vector2 input = Vector2.zero;

        if(IsDead == false)
        {
            input.x = Input.GetAxisRaw(Constants.INPUT_HORIZONTAL_AXIS);
        }

        return input;
    }

    protected override bool ShouldShoot()
    {
        return Input.GetAxisRaw(Constants.INPUT_SHOOT_AXIS) != 0;
    }

    private bool IsWithinBoundaries()
    {
        return Mathf.Abs(transform.position.x) < Mathf.Abs(widthBoundaries);
    }

    private void AdjustToBoundaries()
    {
        Vector3 position = transform.position;

        if (position.x < 0)
        {
            position.x = Mathf.Min(-WidthBoundaries, WidthBoundaries);
        }
        else
        {
            position.x = Mathf.Max(WidthBoundaries, -WidthBoundaries);
        }

        transform.position = position;
    }

    #endregion

    #region CLASS_ENUMS

    #endregion
}
