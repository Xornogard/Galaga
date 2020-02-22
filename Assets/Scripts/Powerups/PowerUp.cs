using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : ScriptableObject, IPowerUp
{
	#region MEMBERS

	#endregion

	#region PROPERTIES

	#endregion

	#region FUNCTIONS

	public abstract void GivePowerUp(PlayerManager playerManager);

	#endregion

	#region CLASS_ENUMS

	#endregion
}
