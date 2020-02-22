using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), CreateAssetMenu(fileName = "New Multishot Weapon", menuName = "GalagAM/Weapons/Create Multhishot Weapon")]
public class MultiShotWeapon : Weapon
{
	#region MEMBERS

	private const int MIN_SHOTS_COUNT = 2;
	private const int MAX_SHOTS_COUNT = 6;

	[SerializeField, Range(MIN_SHOTS_COUNT, MAX_SHOTS_COUNT)]
	private int shotsCount = MIN_SHOTS_COUNT;

	[SerializeField]
	private float shotWidth;

	#endregion

	#region PROPERTIES

	private int ShotsCount => shotsCount;

	private float ShotsWidth => shotWidth;

	#endregion

	#region FUNCTIONS

	public override void Shoot(Vector2 origin, Vector2 direction, LayerMask projectileLayerMask)
	{
		float shotSpacing = GetShotSpacing();
		Vector2 firstShotPosition = GetFirstShotPosition(origin);

		for (int i = 0; i < ShotsCount; i++)
		{
			base.Shoot(firstShotPosition, direction, projectileLayerMask);
			firstShotPosition.x += shotSpacing;
		}
	}

	private float GetShotSpacing()
	{
		return ShotsCount != 1 ? ShotsWidth / (ShotsCount - 1) : 0.0f;
	}

	private Vector2 GetFirstShotPosition(Vector2 origin)
	{
		Vector2 firstShotPosition = origin;

		firstShotPosition.x -= ShotsWidth * 0.5f;

		return firstShotPosition;
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
