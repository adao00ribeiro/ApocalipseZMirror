using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
	public Weapon storedWeapon;
	public GameObject storedDropObject;

	public bool IsFree()
	{
		if (!storedWeapon)
			return true;
		else
			return false;
	}
}

