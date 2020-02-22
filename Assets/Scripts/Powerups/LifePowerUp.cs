using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Life Power Up", menuName = "GalagAM/PowerUps/Life Power Up")]
public class LifePowerUp : PowerUp
{
	#region FUNCTIONS

	public override void GivePowerUp(PlayerManager playerManager)
	{
		playerManager.AddLife();
	}

	#endregion
}
