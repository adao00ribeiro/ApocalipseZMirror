using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Apocalipse;
[CustomEditor(typeof(WeaponManager))]
public class UpdateWeaponManager : Editor
{

		WeaponManager manager;
		Sway swayTransform;

		private void OnEnable()
		{
			manager = FindObjectOfType<WeaponManager>();
			swayTransform = FindObjectOfType<Sway>();
		}

		public override void OnInspectorGUI()
		{
			Editor editor = Editor.CreateEditor(manager);
			editor.DrawDefaultInspector();

			if (GUILayout.Button("Update Weapons"))
			{
				manager.weapons.Clear();
				manager.weapons = GetAllWeapons();
			}
		}

	List<DarkTreeFPS.Weapon> GetAllWeapons()
		{
			List<DarkTreeFPS.Weapon> weaponsInScene = new List<DarkTreeFPS.Weapon>();

			foreach (DarkTreeFPS.Weapon weapon in swayTransform.GetComponentsInChildren<DarkTreeFPS.Weapon>(true))
			{
				weaponsInScene.Add(weapon);
			}
			return weaponsInScene;
		}
}