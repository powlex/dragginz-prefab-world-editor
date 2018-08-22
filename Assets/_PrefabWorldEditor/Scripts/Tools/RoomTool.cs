﻿//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class RoomTool
    {
		public enum RoomPattern {
			None,
			Default
		};

		private static bool _initialised = false;
		private static bool _rebuildOnUpdate = true;

		protected static RoomPattern _roomPattern;

		protected static GameObject _container;

		protected static List<LevelController.LevelElement> _roomElements;

		protected static Part _curPart;

		protected static int _width;
		protected static int _depth;
		protected static int _height;

		protected static float _floorSize = 4.0f;
		protected static float _wallSize  = 1.0f;

		//

		#region Getters

		public RoomPattern roomPattern {
			get { return _roomPattern; }
		}

		public Part curPart {
			get { return _curPart; }
		}

		public int width {
			get { return _width; }
		}

		public int depth {
			get { return _depth; }
		}

		public int height {
			get { return _height; }
		}

		public List<LevelController.LevelElement> roomElements {
			get { return _roomElements; }
		}

		#endregion

		//
		// CONSTRUCTOR
		//
		public RoomTool(GameObject container)
		{
			if (!_initialised)
			{
				_initialised = true;

				_container = container;

				_roomElements = new List<LevelController.LevelElement> ();

				reset ();
			}
        }

		// ------------------------------------------------------------------------
		// Public Methods
		// ------------------------------------------------------------------------
		public void reset()
		{
			foreach (Transform childTransform in _container.transform) {
				GameObject.Destroy(childTransform.gameObject);
			}

			_roomElements.Clear ();

			_width  = 2;
			_depth  = 2;
			_height = 2;

			setRoomPattern (RoomPattern.None);
		}

		// ------------------------------------------------------------------------
		public void activate(RoomPattern pattern, Part part)
		{
			reset (); // just in case

			_curPart = part;

			setRoomPattern (pattern);

			update (-1, -1); // force update
		}

		// ------------------------------------------------------------------------
		public void activateAndCopy(RoomPattern pattern, Part part, int w, int h, int d)
		{
			_rebuildOnUpdate = false;

			reset (); // just in case

			_curPart = part;

			setRoomPattern (pattern);

			PweRoomTools.Instance.setWidthValue (w, pattern);
			PweRoomTools.Instance.setHeightValue (h, pattern);

			_rebuildOnUpdate = true;

			update (-1, -1); // force update
		}

		// ------------------------------------------------------------------------
		public void update(int valueId, int value)
		{
			if (valueId == 0) {
				_width = value;
			} else if (valueId == 1) {
				_depth = value;
			} else if (valueId == 2) {
				_height = value;
			}

			if (_rebuildOnUpdate) {
				removeAll ();
				createObjects ();
			}
		}

		// ------------------------------------------------------------------------
		public void updatePart(Part part)
		{
			_curPart = part;

			removeAll ();
			createObjects ();
		}

		// ------------------------------------------------------------------------
		public virtual void createObjects()
		{
			// OVERRIDE ME
		}

		// ------------------------------------------------------------------------
		public void customUpdate(Vector3 posOrigin)
		{
			if (_roomPattern != RoomPattern.None) {
				_container.transform.position = posOrigin;
			}
		}

		// ------------------------------------------------------------------------
		// Private Methods
		// ------------------------------------------------------------------------
		private void setRoomPattern(RoomPattern pattern)
		{
			if (pattern != _roomPattern) {

				_roomPattern = pattern;

				PweRoomTools.Instance.showToolPanels (pattern);
			}
		}

		// ------------------------------------------------------------------------
		// Protected Methods
		// ------------------------------------------------------------------------
		protected void removeAll ()
		{
			foreach (Transform childTransform in _container.transform) {
				GameObject.Destroy(childTransform.gameObject);
			}

			_roomElements.Clear ();
		}
	}
}