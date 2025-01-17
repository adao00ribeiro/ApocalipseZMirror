﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ApocalipseZ
{

    public class InteractObjects : MonoBehaviour, IInteractObjects
    {
        [Tooltip("The distance within which you can pick up item")]
        public float distance = 1.5f;

        [SerializeField] private IInteract interact;

        [SerializeField] UiFpsScopeCursorReticles PUiFpsScopeCursorReticles;
        public UiFpsScopeCursorReticles UiFpsScopeCursorReticles
        {
            get
            {
                if (PUiFpsScopeCursorReticles == null)
                {
                    PUiFpsScopeCursorReticles = GameController.Instance.CanvasFpsPlayer.GetUiFpsScopeCursorReticles();
                }
                return PUiFpsScopeCursorReticles;
            }
        }
        public LayerMask layer;
        private InputManager InputManager;

        void Awake()
        {
            InputManager = GameController.Instance.InputManager;
        }

        // Update is called once per frame
        public void UpdateInteract()
        {

            RaycastHit hit;


            if (Physics.Raycast(transform.position, transform.forward, out hit, distance, layer))
            {
                interact = hit.collider.gameObject.GetComponent<IInteract>();

                if (interact != null)
                {

                    UiFpsScopeCursorReticles.EnableCursor();
                    UiFpsScopeCursorReticles.SetUseText(interact.GetTitle());
                    if (InputManager.GetUse())
                    {
                        interact.CmdInteract();
                        interact = null;
                        UiFpsScopeCursorReticles.SetUseText("");
                    }

                }
                else
                {
                    UiFpsScopeCursorReticles.DisableCursor();
                    UiFpsScopeCursorReticles.SetUseText("");
                }

            }
            else
            {
                UiFpsScopeCursorReticles.DisableCursor();
                UiFpsScopeCursorReticles.SetUseText("");
            }
        }



    }
}