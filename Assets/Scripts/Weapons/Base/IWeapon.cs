using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Shoot(Vector2 origin, Vector2 direction, LayerMask projectileLayerMask);
    bool CanShoot(float previousShotTime);

    AudioClip GetShootAudioClip();
}
