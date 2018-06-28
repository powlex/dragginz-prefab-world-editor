//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class FlyCam : MonoSingleton<FlyCam>
	{
		public Material matLineBounds;

		private static float movementSpeed = 0.15f;

		//private Camera _myCam;

		private Transform _player;
		private Vector3 _initialPos;
		private Vector3 _initialRotation;

		private Vector3 playerEuler;
		private Vector3 camOffset;
        
		//private float _time;
		//private float _nextPosUpdate;

        private bool _mouseRightIsDown;
        //private bool _move;

		#region Getters

		public Transform player {
			get { return _player; }
		}

		#endregion

		void Awake()
		{
			//_myCam = GetComponent<Camera> ();

			_player = transform.parent;

			_initialPos = _player.position;
			_initialRotation = _player.eulerAngles;

			playerEuler = _player.eulerAngles;

			//_time = 0;
			//_nextPosUpdate = 0;

            _mouseRightIsDown = false;
            //_move = false;
		}

		//
		void Update ()
		{
			//_time = Time.realtimeSinceStartup;
			//_move = Input.GetAxis ("Horizontal") != 0.0f || Input.GetAxis ("Vertical") != 0.0f || Input.GetAxis ("Depth") != 0.0f;

			if (!_mouseRightIsDown) {
				if (Input.GetMouseButtonDown (1)) {
					_mouseRightIsDown = true;
				}
			}
			else {
				if (Input.GetMouseButtonUp (1)) {
					_mouseRightIsDown = false;
				}
			}

			// Looking around with the mouse
			if (_mouseRightIsDown) {
				//Debug.Log ("mouse is down - axis x: " + Input.GetAxis ("Mouse X"));
				_player.Rotate(-2f * Input.GetAxis("Mouse Y"), 2f * Input.GetAxis("Mouse X"), 0);
				playerEuler = _player.eulerAngles;
				playerEuler.z = 0;
				_player.eulerAngles = playerEuler;
			}

			_player.position += (transform.right * Input.GetAxis ("Horizontal") + transform.forward * Input.GetAxis ("Vertical") + transform.up * Input.GetAxis ("Depth")) * movementSpeed;

            //Debug.Log(transform.forward);
		}

		void OnPostRender()
		{
			GLTools.drawBoundingBox (LevelController.Instance.selectedElementBounds, matLineBounds);
		}

		//

		public void setNewInitialPosition(Vector3 newPos, Vector3 newRot)
		{
			_initialPos = newPos;
			_initialRotation = newRot;

			_player.position = _initialPos;
			_player.eulerAngles = _initialRotation;
		}
	}
}