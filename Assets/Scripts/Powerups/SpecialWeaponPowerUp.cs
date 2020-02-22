using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Special Weapon Up", menuName = "GalagAM/PowerUps/Special Weapon Power Up")]
public class SpecialWeaponPowerUp : PowerUp
{
	#region FUNCTIONS

	public override void GivePowerUp(PlayerManager playerManager)
	{
		playerManager.ForceSpecialWeapon();
	}

	#endregion
}
