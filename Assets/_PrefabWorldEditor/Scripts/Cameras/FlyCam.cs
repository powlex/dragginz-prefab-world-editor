//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class FlyCam : MonoSingleton<FlyCam>
	{
		public Material matLineBounds;

		private static float movementSpeed = 0.15f;
        private static float mouseWheelSpeed = 5.0f;

        private Transform _player;
		private Vector3 _initialPos;
		private Vector3 _initialRotation;

		private Vector3 playerEuler;
		private Vector3 camOffset;
        
        private bool _mouseRightIsDown;
        private bool _mouseWheelIsDown;

        public bool drawWireframe;

        #region Getters

        public Transform player {
			get { return _player; }
		}

		#endregion

		void Awake()
		{
			_player = transform.parent;

			_initialPos = _player.position;
			_initialRotation = _player.eulerAngles;

			playerEuler = _player.eulerAngles;

            _mouseRightIsDown = false;
            _mouseWheelIsDown = false;

            drawWireframe = false;
        }

		//
		void Update ()
		{
            if (PweMainMenu.Instance.popup.isVisible ()) {
                return;
            }

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

            if (!_mouseWheelIsDown) {
                if (Input.GetMouseButtonDown (2)) {
                    _mouseWheelIsDown = true;
                }
            }
            else {
                if (Input.GetMouseButtonUp (2)) {
                    _mouseWheelIsDown = false;
                }
            }

            // Looking around with the mouse
            if (_mouseRightIsDown) {
				_player.Rotate(-2f * Input.GetAxis("Mouse Y"), 2f * Input.GetAxis("Mouse X"), 0);
				playerEuler = _player.eulerAngles;
				playerEuler.z = 0;
				_player.eulerAngles = playerEuler;
			}
            else if (_mouseWheelIsDown) {
                _player.Translate (-0.2f * Input.GetAxis ("Mouse X"), -0.2f * Input.GetAxis ("Mouse Y"), 0);
            }

			_player.position += (transform.right * Input.GetAxis ("Horizontal") + transform.forward * Input.GetAxis ("Vertical") + transform.up * Input.GetAxis ("Depth")) * movementSpeed;
		}

        void OnPreRender()
        {
            GL.wireframe = drawWireframe;
        }
        void OnPostRender()
		{
            GL.wireframe = false;
            GLTools.drawBoundingBox (LevelController.Instance.selectedElementBounds, matLineBounds);
		}

		//
        public void mouseWheelMove(float value)
        {
            _player.position += transform.forward * value * mouseWheelSpeed;
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