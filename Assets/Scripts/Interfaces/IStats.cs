using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public interface IStats 
{
    public bool IsPlayerDead ( );
    public void RestoreLife ( int life );
    public void TakeDamage ( int damage );
  
    [Command ( requiresAuthority = false )]
    public void CmdTakeDamage ( int damage , NetworkConnectionToClient sender = null );

    float GetDamage ( );
}