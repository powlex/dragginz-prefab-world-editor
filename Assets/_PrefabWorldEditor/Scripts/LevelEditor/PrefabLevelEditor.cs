﻿//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

//using System;
//using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.XR;

//using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class PrefabLevelEditor : MonoSingleton<PrefabLevelEditor>
    {
		#region PublicProperties

        [HideInInspector]
		public Transform container;

		public GameObject goLights;

        public Transform playerEdit;
		public Transform playerPlay;
        public Transform playerMap;

        public GameObject playerEditSpotLight;
        public GameObject playerPlaySpotLight;

        [SerializeField]
        public Vector3Int levelSize;
        public float gridSize;

        public Transform trfmWalls;
        public Transform trfmBounds;

        public Material matElementMarker;

		public GizmoTranslateScript gizmoTranslateScript;
		public GizmoRotateScript gizmoRotateScript;

        public BoundsLineRenderer boundsLineRenderer;

		//

		public enum EditMode {
			None,
			Place,
			Transform,
			Play
		};

        [HideInInspector]
        public bool leftMouseButtonPressed;

        [HideInInspector]
        public bool leftMouseButtonReleased;

        #endregion

        #region PrivateProperties

        private AssetManager    _assetManager;
        private LevelController _levelController;
		private ToolsController _toolsController;

		private Transform _trfmMarkerX;
		private Transform _trfmMarkerY;
		private Transform _trfmMarkerZ;
		private Transform _trfmMarkerX2;
		private Transform _trfmMarkerY2;
		private Transform _trfmMarkerZ2;

        //private Dictionary<Globals.PartList, Part> _parts;
        //private Dictionary<Globals.AssetType, List<Part>> _assetManager.assetTypeList;
        //private Dictionary<Globals.AssetType, int> _assetManager.assetTypeIndex;

        private int _iCounter;

		private bool _groupEventHandlerSet;

		private Part _curEditPart;
        private GameObject _goEditPart;
        private LevelController.SelectedElementComponents _curEditElementComponents;
        private Vector3 _v3EditPartPos;

        private float _rayDistance;
        private Ray _ray;
        private RaycastHit _hit;
        private GameObject _goHit;

		private Globals.AssetType _assetType;
		private EditMode _editMode;

        private bool _bSpotLightsActive;
        private bool _bSnapToGrid;
        private bool _editorIsPaused;

        private float _mousewheel;
        private float _timer;
        private float _lastMouseWheelUpdate;

        #endregion

        #region Getters

        /*
        public Dictionary<Globals.PartList, Part> parts
        {
            get { return _parts; }
        }

        public Dictionary<Globals.AssetType, List<Part>> assetTypeList {
			get { return _assetManager.assetTypeList; }
		}

        public Dictionary<Globals.AssetType, int> assetTypeIndex {
            get { return _assetManager.assetTypeIndex; }
        }
        */
        
        public EditMode editMode {
			get { return _editMode; }
		}

        public Globals.AssetType assetType {
            get { return _assetType; }
        }

        public ToolsController toolsController {
            get { return _toolsController; }
        }

        #endregion

        // 

        #region SystemMethods

        // ------------------------------------------------------------------------
        public void init ()
        {
            _assetManager = AssetManager.Instance;
            _assetManager.init ();

            /*
            _parts = new Dictionary<Globals.PartList, Part>();

			createPart(Globals.PartList.Floor_1, Globals.AssetType.Floor, "MDC/Floors/Floor_1", 4.00f, 0.10f, 4.00f, Vector3Int.zero, false, "Floor 1");
			createPart(Globals.PartList.Floor_2, Globals.AssetType.Floor, "MDC/Floors/Floor_2", 4.00f, 0.10f, 4.00f, Vector3Int.zero, false, "Floor 2");
			createPart(Globals.PartList.Floor_3, Globals.AssetType.Floor, "MDC/Floors/Floor_3", 4.00f, 0.10f, 4.00f, Vector3Int.zero, false, "Floor 3");
            createPart(Globals.PartList.Floor_4, Globals.AssetType.Floor, "MDC/Floors/Floor_4", 4.00f, 0.10f, 4.00f, Vector3Int.zero, false, "Floor 4");

            createPart(Globals.PartList.Wall_Z,   Globals.AssetType.Wall,  "MDC/WallsZ/Wall_Z",   1.00f,  1.00f,  0.25f, Vector3Int.zero, false, "Wall Left",  "Z");
			createPart(Globals.PartList.Wall_X,   Globals.AssetType.Wall,  "MDC/WallsX/Wall_X",   0.25f,  1.00f,  1.00f, Vector3Int.zero, false, "Wall Right", "X");

            // Chunks
			createPart(Globals.PartList.Chunk_Rock_1,       Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Rock_1",        4.00f,  3.50f,  4.00f, Vector3Int.one, false, "Rock 1");
			createPart(Globals.PartList.Chunk_Rock_2,       Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Rock_2",        4.00f,  2.40f,  4.00f, Vector3Int.one, false, "Rock 2");
			createPart(Globals.PartList.Chunk_Rock_3,       Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Rock_3",        5.00f,  5.00f,  5.00f, Vector3Int.one, false, "Rock 3");
			createPart(Globals.PartList.Chunk_Rock_4,       Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Rock_4",        4.00f,  5.50f,  4.00f, Vector3Int.one, false, "Rock 4");
			createPart(Globals.PartList.Chunk_Stalagmite_1, Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Stalagmite_1",  2.75f,  4.50f,  2.75f, Vector3Int.one, false, "Stalagmite 1");
			createPart(Globals.PartList.Chunk_Stalagmite_2, Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Stalagmite_2",  4.30f,  6.00f,  3.60f, Vector3Int.one, false, "Stalagmite 2");
			createPart(Globals.PartList.Chunk_Stalagmite_3, Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Stalagmite_3",  7.25f,  8.80f,  6.25f, Vector3Int.one, false, "Stalagmite 3");
			createPart(Globals.PartList.Chunk_Cliff_1,      Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Cliff_1",       8.00f,  8.00f,  4.00f, Vector3Int.one, false, "Cliff 1");
			createPart(Globals.PartList.Chunk_Cliff_2,      Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Cliff_2",      10.00f,  8.00f,  7.00f, Vector3Int.one, false, "Cliff 2");

			createPart(Globals.PartList.Chunk_WallEdge,     Globals.AssetType.Chunk, "MDC/Chunks/Chunk_WallEdge",      0.25f,  3.00f,  0.30f, Vector3Int.one, false, "Wall Edge");
			createPart(Globals.PartList.Chunk_LargeBricks,  Globals.AssetType.Chunk, "MDC/Chunks/Chunk_LargeBricks",   6.00f,  0.75f,  0.75f, Vector3Int.one, false, "Large Bricks");
			createPart(Globals.PartList.Chunk_Block,        Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Block",         2.00f,  0.75f,  2.00f, Vector3Int.one, false, "Weird Block");
			createPart(Globals.PartList.Chunk_Corner,       Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Corner",        4.00f,  2.00f,  4.00f, Vector3Int.one, false, "Corner Chunk");
			createPart(Globals.PartList.Chunk_Base,         Globals.AssetType.Chunk, "MDC/Chunks/Chunk_Base",          4.00f,  2.00f,  4.00f, Vector3Int.one, false, "Rounded Base");

            // Props
			createPart(Globals.PartList.Prop_Toilet,    Globals.AssetType.Prop, "Props/Prop_Toilet",         0.50f,  1.00f,  0.74f, Vector3Int.one,  true,  "Dirty Toilet");
			createPart(Globals.PartList.Prop_BonePile,  Globals.AssetType.Prop, "MDC/Props/Prop_BonePile",   2.00f,  0.75f,  2.00f, Vector3Int.one,  false, "Bone Pile");
			createPart(Globals.PartList.Prop_Debris,    Globals.AssetType.Prop, "MDC/Props/Prop_Debris",     3.30f,  1.20f,  3.70f, Vector3Int.one,  false, "Debris");
			createPart(Globals.PartList.Prop_Grave_1,   Globals.AssetType.Prop, "MDC/Props/Prop_Grave_1",    1.00f,  0.88f,  3.00f, Vector3Int.one,  true,  "Grave");
			createPart(Globals.PartList.Prop_TombStone, Globals.AssetType.Prop, "MDC/Props/Prop_TombStone",  3.00f,  1.60f,  0.25f, Vector3Int.one,  true,  "Tomb Stone");

			createPart(Globals.PartList.Pillar_1,     Globals.AssetType.Prop, "MDC/Props/Pillar_1",      2.00f,  3.00f,  2.00f, Vector3Int.one,  true, "Pillar 1");
			createPart(Globals.PartList.Pillar_2,     Globals.AssetType.Prop, "MDC/Props/Pillar_2",      1.50f,  1.50f,  4.75f, Vector3Int.one,  true, "Pillar 2");
			createPart(Globals.PartList.Pillar_3,     Globals.AssetType.Prop, "MDC/Props/Pillar_3",      1.50f,  1.50f,  1.50f, Vector3Int.one,  true, "Pillar Base");

			// Dungeons
			createPart(Globals.PartList.Dungeon_Floor,     Globals.AssetType.Dungeon, "Dungeons/Dungeon_Floor",     2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Floor");
			createPart(Globals.PartList.Dungeon_Wall_L,    Globals.AssetType.Dungeon, "Dungeons/Dungeon_Wall_L",    2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Wall");
			createPart(Globals.PartList.Dungeon_Wall_LR,   Globals.AssetType.Dungeon, "Dungeons/Dungeon_Wall_LR",   2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Walls");
			createPart(Globals.PartList.Dungeon_Corner,    Globals.AssetType.Dungeon, "Dungeons/Dungeon_Corner",    2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Corner");
			createPart(Globals.PartList.Dungeon_DeadEnd,   Globals.AssetType.Dungeon, "Dungeons/Dungeon_DeadEnd",   2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Dead End");
			createPart(Globals.PartList.Dungeon_Turn,      Globals.AssetType.Dungeon, "Dungeons/Dungeon_Turn",      2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Turn");
			createPart(Globals.PartList.Dungeon_T,         Globals.AssetType.Dungeon, "Dungeons/Dungeon_T",         2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon T Intersection");
			createPart(Globals.PartList.Dungeon_Stairs_1,  Globals.AssetType.Dungeon, "Dungeons/Dungeon_Stairs_1",  2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Stairs Lower");
			createPart(Globals.PartList.Dungeon_Stairs_2,  Globals.AssetType.Dungeon, "Dungeons/Dungeon_Stairs_2",  2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Stairs Upper");
			createPart(Globals.PartList.Dungeon_Ramp_1,    Globals.AssetType.Dungeon, "Dungeons/Dungeon_Ramp_1",    2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Ramp Lower");
			createPart(Globals.PartList.Dungeon_Ramp_2,    Globals.AssetType.Dungeon, "Dungeons/Dungeon_Ramp_2",    2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Ramp Upper");
			createPart(Globals.PartList.Dungeon_Wall_L_NF, Globals.AssetType.Dungeon, "Dungeons/Dungeon_Wall_L_NF", 2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Wall No Floor");
			createPart(Globals.PartList.Dungeon_Corner_NF, Globals.AssetType.Dungeon, "Dungeons/Dungeon_Corner_NF", 2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Corner No Floor");

            // Lights
            createPart(Globals.PartList.Light_Lantern, Globals.AssetType.Lights, "Lights/Light_Lantern", 0.25f, 0.50f, 0.25f, Vector3Int.one, false, "Lantern");
            createPart(Globals.PartList.Light_Torch,   Globals.AssetType.Lights, "Lights/Light_Torch",   0.25f, 1.50f, 0.25f, Vector3Int.one, false, "Torch");

            // Misc
            createPart (Globals.PartList.Moving_Platform, Globals.AssetType.Misc, "3DGameKit/Misc/MovingPlatform", 2.00f, 0.50f, 2.00f, Vector3Int.one, false, "Moving Platform");
            createPart (Globals.PartList.Impling,         Globals.AssetType.Misc, "Enemies/Impling",               1.00f, 0.75f, 1.00f, Vector3Int.one, true,  "Impling");

            //

            createAssetTypeCount ();
            */

            container = new GameObject(Globals.levelChunkContainerName).transform;

			_levelController = LevelController.Instance;
			_levelController.init ();

            setWalls();

			_iCounter = 0;

			_groupEventHandlerSet = false;

            _curEditElementComponents = new LevelController.SelectedElementComponents ();
            _curEditElementComponents.reset ();
            _v3EditPartPos = Vector3.zero;

			_assetType = Globals.AssetType.Floor;
			_editMode = EditMode.None;

            _bSpotLightsActive = true;
            _bSnapToGrid = true;
            _editorIsPaused = false;

            GameObject toolContainer = new GameObject(Globals.toolsContainerName);

			_toolsController = ToolsController.Instance;
			_toolsController.init (toolContainer);

			PweMainMenu.Instance.init ();
            if (!AppController.Instance.editorIsInOfflineMode) {
                int i, len = LevelChunkManager.Instance.numLevels;
                for (i = 0; i < len; ++i) {
                    PweMainMenu.Instance.addLevelToMenu (LevelChunkManager.Instance.levelByIndex[i].name);
                }
            }
            else {
                PweChunkMap.Instance.gameObject.SetActive (false);
            }

            PwePlacementTools.Instance.init ();
			PweDungeonTools.Instance.init ();
			PweRoomTools.Instance.init ();

			setNewEditPart(AssetManager.Instance.assetTypeList[_assetType][0]);

            leftMouseButtonPressed  = false;
            leftMouseButtonReleased = false;

            if (XRSettings.enabled)
			{
                VREditor.Instance.init();
				PweWorldSpaceMenus.Instance.init ();
				PweDynamicMenusVR.Instance.init ();
				setEditMode (EditMode.Transform);
			}
			else
			{
                // TEST TEST TEST
                // VREditor.Instance.init();
                // PweDynamicMenusVR.Instance.init ();

				setEditMode (EditMode.Place);
			}
        }

        // ------------------------------------------------------------------------
        /*void Update()
		{
			_timer = Time.realtimeSinceStartup;

            leftMouseButtonPressed  = Input.GetMouseButtonDown (0);
            leftMouseButtonReleased = Input.GetMouseButtonUp (0);

            if (_editMode == EditMode.Play)
			{
				updatePlayMode ();
			}
			else
			{
				updateEditMode ();
			}
		}*/

        // ------------------------------------------------------------------------
        /*void LateUpdate()
		{
			if (!XRSettings.enabled) {
				return;
			}

			if (ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.Menu))
			{
				Debug.Log("ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.Menu)");
			}

			if (ViveInput.GetPressUpEx (HandRole.LeftHand, ControllerButton.Menu)) {
				Debug.Log("ViveInput.GetPressUpEx(HandRole.LeftHand, ControllerButton.Menu)");
			}
		}*/

        #endregion

        // ------------------------------------------------------------------------
        // Public Methods
        // ------------------------------------------------------------------------
        public void customUpdate(float t, float d)
		{
			_timer = t;

            leftMouseButtonPressed  = Input.GetMouseButtonDown (0);
            leftMouseButtonReleased = Input.GetMouseButtonUp (0);

            if (!_editorIsPaused) {
                if (_editMode == EditMode.Play) {
                    updatePlayMode ();
                }
                else {
                    updateEditMode ();
                }
            }
		}

        // ------------------------------------------------------------------------
        public void newLevelWithDimensions(int x, int y, int z)
		{
            if (_editorIsPaused) {
                showWorldMap (false);
            }

            levelSize.x = x;
			levelSize.y = y;
			levelSize.z = z;

			_levelController.clearLevel ();
			_iCounter = 0;
			setWalls ();

			setEditMode (EditMode.Transform);
		}

        // ------------------------------------------------------------------------
        public void showWorldMap(bool state)
        {
            _editorIsPaused = state;
            setEditMode ((_editorIsPaused ? EditMode.None : EditMode.Transform));

            container.gameObject.SetActive (!_editorIsPaused);
            trfmWalls.gameObject.SetActive (!_editorIsPaused);
            trfmBounds.gameObject.SetActive (!_editorIsPaused);

            PweChunkMap.Instance.showmapContainer (state);
            playerMap.gameObject.SetActive (state);

            if (state) {
                playerEdit.gameObject.SetActive (false);
                playerPlay.gameObject.SetActive (false);

                WorldMapController.Instance.init ();
            }
        }

        // ------------------------------------------------------------------------
        public void clearLevel()
		{
			setNewEditPart(_assetManager.assetTypeList[_assetType][0]); // reset to floor tile
			setEditMode(EditMode.None, true); // force reset

            PweMainMenu.Instance.popup.showPopup (Globals.PopupMode.Confirmation, "Clear Level", "Are you sure?", clearLevelConfirm);
		}

        public void clearLevelConfirm(int buttonId) {

			PweMainMenu.Instance.popup.hide();
			if (buttonId == 1)
			{
				_levelController.clearLevel ();
				_iCounter = 0;
			}
		}

        // ------------------------------------------------------------------------
        public void setSpotLights(bool state) {

            _bSpotLightsActive = state;

            if (playerEdit.gameObject.activeSelf) {
                playerEditSpotLight.SetActive(_bSpotLightsActive);
            }
            else if (playerPlay.gameObject.activeSelf) {
                playerPlaySpotLight.SetActive(_bSpotLightsActive);
            }
        }

        // ------------------------------------------------------------------------
        public void setSnapToGrid(bool state) {

            _bSnapToGrid = state;
        }

        // ------------------------------------------------------------------------
        public void setEditMode(EditMode mode, bool force = false)
		{
            //Debug.Log ("setEditMode "+mode);
            if (force || mode != _editMode) {

                _editMode = mode;

                PweSettings.Instance.setAmbientLightToggle(_editMode != EditMode.Play);

                trfmWalls.gameObject.SetActive(_editMode != EditMode.Play);

                _levelController.resetElementComponents();

                resetSelectedElement();
                resetCurPlacementTool();
                resetCurDungeonTool();
                resetCurRoomTool();

                PweMainMenu.Instance.setModeButtons(_editMode);
                PweMainMenu.Instance.showTransformBox(_editMode == EditMode.Transform);
                PweMainMenu.Instance.showAssetTypeBox(_editMode == EditMode.Place);
                PweMainMenu.Instance.showPlacementToolBox(_editMode == EditMode.Place && (_assetType == Globals.AssetType.Chunk || _assetType == Globals.AssetType.Prop));
                PweMainMenu.Instance.showDungeonToolBox(_editMode == EditMode.Place && _assetType == Globals.AssetType.Dungeon);
                PweMainMenu.Instance.showRoomToolBox(_editMode == EditMode.Place && (_assetType == Globals.AssetType.Floor || _assetType == Globals.AssetType.Wall));

                if (_editMode == EditMode.Place) {
                    PweMainMenu.Instance.setAssetTypeButtons(_assetType);
                    PweMainMenu.Instance.showAssetInfo(_curEditPart);
                } else {
                    PweMainMenu.Instance.setAssetNameText("");
                    PweMainMenu.Instance.setSpecialHelpText("");
                }
                PweMainMenu.Instance.showAssetInfoPanel(_editMode == EditMode.Place);
                PweMainMenu.Instance.showInstanceInfoPanel(false);

                playerMap.gameObject.SetActive (false);

                bool playerEditWasActive = playerEdit.gameObject.activeSelf;
                bool playerPlayWasActive = playerPlay.gameObject.activeSelf;

                if (!XRSettings.enabled) {
                    playerEdit.gameObject.SetActive(_editMode != EditMode.Play);
                    playerPlay.gameObject.SetActive(!playerEdit.gameObject.activeSelf);
                } else {
                    playerEdit.gameObject.SetActive(false);
                    playerPlay.gameObject.SetActive(false);
                }

                if (playerEdit.gameObject.activeSelf && !playerEditWasActive) {
                    playerEdit.position = playerPlay.position;
                }
                else if (playerPlay.gameObject.activeSelf && !playerPlayWasActive) {
                    playerPlay.position = playerEdit.position;
                }

                if (playerEdit.gameObject.activeSelf) {
                    playerEditSpotLight.SetActive(_bSpotLightsActive);
                } else if (playerPlay.gameObject.activeSelf) {
                    playerPlaySpotLight.SetActive(_bSpotLightsActive);
                }

                if (_goEditPart != null)
				{
					_goEditPart.SetActive (_editMode == EditMode.Place);
					setMarkerActive (_goEditPart.activeSelf);
					if (_goEditPart.activeSelf) {
						setMarkerScale (_curEditPart);
                        boundsLineRenderer.gameObject.SetActive (_editMode == EditMode.Place);
                    }
                }
				else {
					setMarkerActive (false);
				}

                // Instructions
                if (_editMode == EditMode.Place) {
					PweMainMenu.Instance.setInstructionsText ("Use Mousewheel to toggle through assets");
				} else if (_editMode == EditMode.Play) {
					PweMainMenu.Instance.setInstructionsText ("Press Esc to exit play mode");
				} else if (_editMode == EditMode.Transform) {
					PweMainMenu.Instance.setInstructionsText ("Click object to select");
					PweMainMenu.Instance.setSpecialHelpText ("Shift+Click = Select group of objects\nClick+'C' = Copy object\nShift+Click+'C' = Copy group of objects");
				} else {
					PweMainMenu.Instance.setInstructionsText ("");
				}
			}
		}

		// ------------------------------------------------------------------------
		public void selectTransformTool(int toolId)
		{
			gizmoTranslateScript.gameObject.SetActive (toolId == 0);
			gizmoRotateScript.gameObject.SetActive (toolId == 1);

			if (gizmoTranslateScript.gameObject.activeSelf && gizmoTranslateScript.translateTarget == null) {
				gizmoTranslateScript.gameObject.SetActive (false);
			}
			if (gizmoRotateScript.gameObject.activeSelf && gizmoRotateScript.rotateTarget == null) {
				gizmoRotateScript.gameObject.SetActive (false);
			}

			if (gizmoTranslateScript.gameObject.activeSelf) {
				if (!_groupEventHandlerSet) {
					gizmoTranslateScript.positionChanged += onSelectedObjectPositionChanged;
					_groupEventHandlerSet = true;
				}
			}
		}

		// ------------------------------------------------------------------------
		public void selectAssetType(Globals.AssetType type)
		{
			_assetType = type;

			PweMainMenu.Instance.showPlacementToolBox (_editMode == EditMode.Place && (_assetType == Globals.AssetType.Chunk || _assetType == Globals.AssetType.Prop));
			PweMainMenu.Instance.showDungeonToolBox (_editMode == EditMode.Place && _assetType == Globals.AssetType.Dungeon);
			PweMainMenu.Instance.showRoomToolBox (_editMode == EditMode.Place && (_assetType == Globals.AssetType.Floor || _assetType == Globals.AssetType.Wall));

			resetCurPlacementTool ();
			resetCurDungeonTool ();
			resetCurRoomTool ();

			int index = _assetManager.assetTypeIndex [_assetType];
			setNewEditPart(_assetManager.assetTypeList[_assetType][index]);
		}

        // ------------------------------------------------------------------------
        public void setNewEditPart(Part part)
        {
            _curEditPart = part;

            if (_goEditPart != null) {
                Destroy(_goEditPart);
            }
            _goEditPart = null;

            _goEditPart = createPartAt(_curEditPart.id, -10, -10, -10);
            setMarkerScale(_curEditPart);

            _levelController.setComponents (_goEditPart, false, false);

            _curEditElementComponents.init(_levelController.getChildrenRecursive (_goEditPart));
            boundsLineRenderer.updateBounds (_curEditElementComponents.bounds);
            
            PweMainMenu.Instance.showAssetInfo(_curEditPart);
        }

        // ------------------------------------------------------------------------
        public void selectPlacementTool(PlacementTool.PlacementMode mode)
		{
			if (_toolsController.curPlacementTool == null || _toolsController.curPlacementTool.placementMode != mode)
			{
				if (_editMode == EditMode.Transform)
				{
					if (_levelController.selectedElement.go != null) {

						activatePlacementTool (mode, _assetManager.parts[_levelController.selectedElement.partId]);
					}
				}
				else if (_goEditPart != null)
				{
					if (_curEditPart.type != Globals.AssetType.Floor && _curEditPart.type != Globals.AssetType.Wall)
					{
						activatePlacementTool (mode, _curEditPart);
					}
				}
			}
		}

		// ------------------------------------------------------------------------
		public void placementToolValueChange(int valueId, int value)
		{
			if (_toolsController.curPlacementTool != null) {
				_toolsController.curPlacementTool.update (valueId, value);
			}
		}

		// ------------------------------------------------------------------------
		public void selectDungeonTool(DungeonTool.DungeonPreset preset)
		{
			if (_toolsController.curDungeonTool == null || _toolsController.curDungeonTool.dungeonPreset != preset)
			{
				if (_editMode == EditMode.Place)
				{
					if (_curEditPart.type == Globals.AssetType.Dungeon)
					{
						setNewEditPart(_assetManager.assetTypeList[_assetType][0]); // reset to floor tile
						activateDungeonTool (preset);
					}
				}
			}
		}

		// ------------------------------------------------------------------------
		public void dungeonToolValueChange(int valueId, int value)
		{
			if (_toolsController.curDungeonTool != null) {
				_toolsController.curDungeonTool.update (valueId, value);
			}
		}

		// ------------------------------------------------------------------------
		public void selectRoomTool(RoomTool.RoomPattern pattern)
		{
			if (_toolsController.curRoomTool == null || _toolsController.curRoomTool.roomPattern != pattern)
			{
				if (_editMode == EditMode.Place)
				{
					if (_curEditPart.type == Globals.AssetType.Floor || _curEditPart.type == Globals.AssetType.Wall)
					{
						activateRoomTool (pattern, _curEditPart);
					}
				}
			}
		}

		// ------------------------------------------------------------------------
		public void roomToolValueChange(int valueId, int value)
		{
			if (_toolsController.curRoomTool != null) {
				_toolsController.curRoomTool.update (valueId, value);
			}
		}

		// ------------------------------------------------------------------------
		// Private Methods
		// ------------------------------------------------------------------------
		private void updatePlayMode()
		{
			if (Input.GetKeyDown (KeyCode.Escape)) {
				if (PweMainMenu.Instance.popup.isVisible ()) {
					PweMainMenu.Instance.popup.hide ();
				} else {
					setEditMode (EditMode.Transform);
				}
			}
		}

		// ------------------------------------------------------------------------
		private void updateEditMode()
		{
            if (Input.GetKeyDown (KeyCode.Escape))
            {
                if (PweMainMenu.Instance.popup.isVisible ()) {
                    PweMainMenu.Instance.popup.hide ();
                } else if (_toolsController.curPlacementTool != null) {
                    resetCurPlacementTool ();
                } else if (_toolsController.curDungeonTool != null) {
                    resetCurDungeonTool ();
                } else if (_toolsController.curRoomTool != null) {
                    resetCurRoomTool ();
                }
                else {
                    setEditMode (EditMode.Transform, true); // force reset
                }
            }
            else
            {
                if (PweMainMenu.Instance.popup.isVisible ()) {
                    return;
                }
            }

			if (Input.GetKeyDown (KeyCode.P)) {
				setEditMode (EditMode.Play);
			}
            else if (Input.GetKeyDown(KeyCode.LeftShift)) {
                FlyCam.Instance.drawWireframe = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift)) {
                FlyCam.Instance.drawWireframe = false;
            }

            if (Camera.main == null) {
				return;
			}

            // check gizmo first when left mouse is clicked
            if (leftMouseButtonPressed && gizmoTranslateScript.gameObject.activeSelf) {

                gizmoTranslateScript.mouseDown ();
            }
            else if (leftMouseButtonReleased && gizmoTranslateScript.gameObject.activeSelf) {

                gizmoTranslateScript.mouseUp ();
            }

            _rayDistance = 5f;
			_goHit = null;
            _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (_ray, out _hit, 100)) {
                _goHit = _hit.collider.gameObject;
                _rayDistance = _hit.distance;
            }

            _mousewheel = Input.GetAxis ("Mouse ScrollWheel");

            if (_editMode == EditMode.Transform)
			{
                if (leftMouseButtonPressed && !gizmoTranslateScript.handlePressed && _goHit != null)
				{
					if (!EventSystem.current.IsPointerOverGameObject ()) {
                        if (Input.GetKey(KeyCode.N)) {
                            proBuilderizeElement(_goHit.transform);
                        }
                        else if (Input.GetKey(KeyCode.M)) {
                            combineMeshesInElement(_goHit.transform);
                        }
                        else if (Input.GetKey(KeyCode.C)) {
                            copyElement(_goHit.transform);
                        }
                        else {
                            selectElement(_goHit.transform);
                        }
					}
				}
				if (_levelController.selectedElement.go != null) {
					if (Input.GetKeyDown (KeyCode.Delete)) {
						deleteSelectedElement ();
					} else {
						positionSelectedElement ();
					}
				}
                else {
                    if (_mousewheel != 0) {
                        FlyCam.Instance.mouseWheelMove (_mousewheel);
                    }
                }
			}
			else if (_editMode == EditMode.Place)
			{
				positionEditPart ();
			}
		}

		// ------------------------------------------------------------------------
		private void onSelectedObjectPositionChanged(Vector3 posChange)
		{
			if (posChange != Vector3.zero) {

                Vector3 lastPos = _v3EditPartPos = _levelController.selectedElement.go.transform.position;

                if (_bSnapToGrid) {

                    _v3EditPartPos -= posChange;

                    bool doSnap = false;
                    if (posChange.x != 0 && Mathf.Abs(posChange.x) >= gridSize) {
                        doSnap = true;
                    }
                    else if (posChange.y != 0 && Mathf.Abs (posChange.y) >= gridSize) {
                        doSnap = true;
                    }
                    else if (posChange.z != 0 && Mathf.Abs (posChange.z) >= gridSize) {
                        doSnap = true;
                    }

                    if (doSnap) {
                        Vector3 posChangeNormalized = posChange.normalized;
                        _v3EditPartPos += (posChangeNormalized * gridSize);
                    }
                }

                _v3EditPartPos = boundsCheck (_v3EditPartPos, _levelController.selectedElement.part);
                posChange -= (lastPos - _v3EditPartPos);

                _levelController.selectedElement.go.transform.position = _v3EditPartPos;
                _levelController.updatedSelectedObjectPosition (posChange);

                if (_levelController.iSelectedGroupIndex != -1) {
                    Bounds bounds = _levelController.aElementGroups[_levelController.iSelectedGroupIndex].updateBounds ();
                    boundsLineRenderer.updateBounds (bounds);
                }
                else {
                    boundsLineRenderer.updateBounds (_levelController.selectedElementBounds);
                }
            }
		}

        // ------------------------------------------------------------------------
        private Vector3 boundsCheck(Vector3 pos, Part part) {

            if (pos.x - part.w / 2 < 0) {
                pos.x = part.w / 2;
            }
            else if (pos.x + part.w / 2 > levelSize.x) {
                pos.x = levelSize.x - part.w / 2;
            }

            if (pos.y - part.h / 2 < 0) {
                pos.y = part.h / 2;
            }
            else if (pos.y + part.h / 2 > levelSize.y) {
                pos.y = levelSize.y - part.h / 2;
            }

            if (pos.z - part.d / 2 < 0) {
                pos.z = part.d / 2;
            }
            else if (pos.z + part.d / 2 > levelSize.z) {
                pos.z = levelSize.z - part.d / 2;
            }

            return pos;
        }

		// ------------------------------------------------------------------------
		// PLACEMENT TOOL
		// ------------------------------------------------------------------------
		private void resetCurPlacementTool()
		{
			_toolsController.resetCurPlacementTool ();
            PweMainMenu.Instance.showAssetInfo (_curEditPart);
		}

		// ------------------------------------------------------------------------
		private void activatePlacementTool(PlacementTool.PlacementMode mode, Part part)
		{
			_toolsController.setPlacementTool (mode, part);
            PweMainMenu.Instance.showAssetInfo (part);

			_toolsController.curPlacementTool.activate (mode, part);
		}

		private void activatePlacementTool(PlacementTool.PlacementMode mode, Part part, LevelController.ElementGroup elmGrp)
		{
			_toolsController.setPlacementTool (mode, part);
            PweMainMenu.Instance.showAssetInfo (part);

			_toolsController.curPlacementTool.activateAndCopy (mode, part, elmGrp.radius, elmGrp.interval, elmGrp.density, elmGrp.inverse);
		}

		// ------------------------------------------------------------------------
		// DUNGEON TOOL
		// ------------------------------------------------------------------------
		private void resetCurDungeonTool()
		{
			_toolsController.resetCurDungeonTool ();
            PweMainMenu.Instance.showAssetInfo (_curEditPart);
		}

		// ------------------------------------------------------------------------
		private void activateDungeonTool(DungeonTool.DungeonPreset preset)
		{
			_toolsController.setDungeonTool (preset);
			_toolsController.curDungeonTool.activate (preset);
		}

		private void activateDungeonTool(DungeonTool.DungeonPreset preset, LevelController.ElementGroup elmGrp)
		{
			_toolsController.setDungeonTool (preset);
			_toolsController.curDungeonTool.activateAndCopy (preset, elmGrp.width, elmGrp.height, elmGrp.depth, elmGrp.ceiling);
		}

		// ------------------------------------------------------------------------
		// ROOM TOOL
		// ------------------------------------------------------------------------
		private void resetCurRoomTool()
		{
			_toolsController.resetCurRoomTool ();
            PweMainMenu.Instance.showAssetInfo (_curEditPart);
		}

		// ------------------------------------------------------------------------
		private void activateRoomTool(RoomTool.RoomPattern pattern, Part part)
		{
			_toolsController.setRoomTool (pattern, part);
            PweMainMenu.Instance.showAssetInfo (part);

			_toolsController.curRoomTool.activate (pattern, part);
		}

		private void activateRoomTool(RoomTool.RoomPattern pattern, Part part, LevelController.ElementGroup elmGrp)
		{
			_toolsController.setRoomTool (pattern, part);
            PweMainMenu.Instance.showAssetInfo (part);

			_toolsController.curRoomTool.activateAndCopy (pattern, part, elmGrp.width, elmGrp.height, elmGrp.depth);
		}

		// ------------------------------------------------------------------------
		//
		// ------------------------------------------------------------------------
		private void positionEditPart()
		{
            bool partWasSnappedToOtherPart = false;

			float snap = gridSize;
			//if (_goEditPart != null && _curEditPart.type == Globals.AssetType.Dungeon) {
			//	snap = 1f;
			//}

			_v3EditPartPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _rayDistance));

			// snap dungeon pieces on top of each other!
			if (_goHit != null) {
				if (_curEditPart.type == Globals.AssetType.Dungeon && _toolsController.curDungeonTool == null)
				{
					Transform trfmParent = _goHit.transform;
					while (trfmParent.parent != null && trfmParent.tag != "PartContainer") {
						trfmParent = trfmParent.parent;
					}

					// not an asset?
					if (trfmParent.tag == "PartContainer") {
						if (_levelController.levelElements.ContainsKey(trfmParent.name)) {
                            _v3EditPartPos = trfmParent.position + (2f * _hit.normal);
                            partWasSnappedToOtherPart = true;
                        }
					}
				}
			}

            //
            // Positioning
            //
            if (!partWasSnappedToOtherPart)
            {
                if (_bSnapToGrid) {
                    _v3EditPartPos.x = Mathf.RoundToInt (_v3EditPartPos.x / snap) * snap;
                }
                if (_v3EditPartPos.x - _curEditPart.w / 2 < 0) {
                    _v3EditPartPos.x = _curEditPart.w / 2;
                }
                else if (_v3EditPartPos.x + _curEditPart.w / 2 > levelSize.x) {
                    _v3EditPartPos.x = levelSize.x - _curEditPart.w / 2;
                }

                if (_bSnapToGrid) {
                    _v3EditPartPos.y = Mathf.RoundToInt (_v3EditPartPos.y / snap) * snap;
                }
                if (_v3EditPartPos.y - _curEditPart.h / 2 < 0) {
                    _v3EditPartPos.y = _curEditPart.h / 2;
                }
                else if (_v3EditPartPos.y + _curEditPart.h / 2 > levelSize.y) {
                    _v3EditPartPos.y = levelSize.y - _curEditPart.h / 2;
                }

                if (_bSnapToGrid) {
                    _v3EditPartPos.z = Mathf.RoundToInt (_v3EditPartPos.z / snap) * snap;
                }
                if (_v3EditPartPos.z - _curEditPart.d / 2 < 0) {
                    _v3EditPartPos.z = _curEditPart.d / 2;
                }
                else if (_v3EditPartPos.z + _curEditPart.d / 2 > levelSize.z) {
                    _v3EditPartPos.z = levelSize.z - _curEditPart.d / 2;
                }
            }

			_goEditPart.transform.position = _v3EditPartPos;
			setMarkerPosition (_goEditPart.transform);

			//
			// Tools
			//
			if (_toolsController.curPlacementTool != null) {
				if (_editMode == EditMode.Transform && _levelController.selectedElement.go != null) {
					_toolsController.curPlacementTool.customUpdate (_levelController.selectedElement.go.transform.position);
				} else if (_goEditPart != null) {
					_toolsController.curPlacementTool.customUpdate (_goEditPart.transform.position);
				}
			}
			else if (_toolsController.curDungeonTool != null) {
				if (_goEditPart != null) {

					// check boundaries
					float half = _toolsController.curDungeonTool.cubeSize / 2f;
					int xStart = _toolsController.curDungeonTool.width / 2;
					if (_v3EditPartPos.x - (xStart * _toolsController.curDungeonTool.cubeSize + half) < 0) {
						_v3EditPartPos.x = (xStart * _toolsController.curDungeonTool.cubeSize + half);
					}
					int zStart = _toolsController.curDungeonTool.depth / 2;
					if (_v3EditPartPos.z - (zStart * _toolsController.curDungeonTool.cubeSize + half) < 0) {
						_v3EditPartPos.z = (zStart * _toolsController.curDungeonTool.cubeSize + half);
					}
					_goEditPart.transform.position = _v3EditPartPos;
					setMarkerPosition (_goEditPart.transform);

					_toolsController.curDungeonTool.customUpdate (_goEditPart.transform.position);
				}
			}
			else if (_toolsController.curRoomTool != null) {
				if (_goEditPart != null) {
					_toolsController.curRoomTool.customUpdate (_goEditPart.transform.position);
				}
			}

			//
			// Mousewheel
			//

			//_mousewheel = Input.GetAxis ("Mouse ScrollWheel");

			if (_mousewheel != 0)
			{
				if (_timer > _lastMouseWheelUpdate)
				{
					_lastMouseWheelUpdate = _timer + 0.2f;
					float dir = (_mousewheel > 0 ? 1 : -1);

					if (_toolsController.curDungeonTool != null) {
					
						if (Input.GetKey (KeyCode.Alpha1)) {
							PweDungeonTools.Instance.updateWidthValue ((int)dir, _toolsController.curDungeonTool.dungeonPreset);
						} else if (Input.GetKey (KeyCode.Alpha2)) {
							PweDungeonTools.Instance.updateDepthValue ((int)dir, _toolsController.curDungeonTool.dungeonPreset);
						} else if (Input.GetKey (KeyCode.Alpha3)) {
							PweDungeonTools.Instance.updateHeightValue ((int)dir, _toolsController.curDungeonTool.dungeonPreset);
						}
					}
					else if (_toolsController.curPlacementTool != null) {
						
						if (Input.GetKey (KeyCode.Alpha1)) {
							PwePlacementTools.Instance.updateRadiusValue ((int)dir, _toolsController.curPlacementTool.placementMode);
						} else if (Input.GetKey (KeyCode.Alpha2)) {
							PwePlacementTools.Instance.updateIntervalValue ((int)dir, _toolsController.curPlacementTool.placementMode);
						} else if (Input.GetKey (KeyCode.Alpha3)) {
							PwePlacementTools.Instance.updateDensityValue ((int)dir, _toolsController.curPlacementTool.placementMode);
						} else {
							toggleEditPart (_mousewheel);
							_toolsController.curPlacementTool.updatePart (_curEditPart);
						}
					}
					else if (_toolsController.curRoomTool != null) {

						if (Input.GetKey (KeyCode.Alpha1)) {
							PweRoomTools.Instance.updateWidthValue ((int)dir, _toolsController.curRoomTool.roomPattern);
						}
						else if (Input.GetKey (KeyCode.Alpha2)) {
							PweRoomTools.Instance.updateHeightValue ((int)dir, _toolsController.curRoomTool.roomPattern);
						} else {
							toggleEditPart (_mousewheel);
							_toolsController.curRoomTool.updatePart (_curEditPart);
						}
					}
					else {
						
						float multiply = (_curEditPart.type == Globals.AssetType.Dungeon ? 90f * dir : 15f * dir);

						if (Input.GetKey (KeyCode.X)) {
							if (_curEditPart.canRotate.x == 1) {
								_goEditPart.transform.Rotate (Vector3.right * multiply);
							}
						} else if (Input.GetKey (KeyCode.Y)) {
							if (_curEditPart.canRotate.y == 1) {
								_goEditPart.transform.Rotate (Vector3.up * multiply);
							}
						} else if (Input.GetKey (KeyCode.Z)) {
							if (_curEditPart.canRotate.z == 1) {
								_goEditPart.transform.Rotate (Vector3.forward * multiply);
							}
						}
						else if (Input.GetKey (KeyCode.C)) {
							Vector3 scale = _goEditPart.transform.localScale;
                            scale.x += (scale.x * .1f * dir);
                            scale.y += (scale.y * .1f * dir);
                            scale.z += (scale.z * .1f * dir);
                            _goEditPart.transform.localScale = scale;
						} else {
							toggleEditPart (_mousewheel);
						}
					}
				}
			}

            _curEditElementComponents.getMeshRendererBounds ();
            boundsLineRenderer.updateBounds (_curEditElementComponents.bounds);

            if (leftMouseButtonPressed)
			{
				if (!EventSystem.current.IsPointerOverGameObject ()) {
					if ((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) && _curEditPart.type == Globals.AssetType.Floor) {
						fillY (_goEditPart.transform.position);
					} else if ((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) && _curEditPart.extra == "Z") {
						fillZ (_goEditPart.transform.position);
					} else if ((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) && _curEditPart.extra == "X") {
						fillX (_goEditPart.transform.position);
					} else {
						placePart (_goEditPart.transform.position, _goEditPart.transform.localScale);
					}
				}
			}
        }

        // ------------------------------------------------------------------------
        private void proBuilderizeElement(Transform trfmHit)
        {
            Transform trfmParent = getParentTransform(trfmHit);
            if (trfmParent != null) {
                MakePrimitiveEditable.convertToProBuilderMesh(trfmHit.GetComponent<MeshFilter>());
            }
        }

        // ------------------------------------------------------------------------
        private void combineMeshesInElement(Transform trfmHit)
        {
            #if UNITY_EDITOR
                Transform trfmParent = getParentTransform(trfmHit);
                if (trfmParent != null) {
                    CombineMeshes.run(trfmParent.gameObject);
                }
            #endif
        }

        // ------------------------------------------------------------------------
        private void copyElement(Transform trfmHit)
		{
            Transform trfmParent = getParentTransform(trfmHit);
            if (trfmParent == null) {
                return;
            }

            if (_levelController.levelElements.ContainsKey (trfmParent.gameObject.name)) {

				bool isShift = (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift));
				int groupIndex = -1;

				// group select
				if (isShift) {
					groupIndex = _levelController.findElementInGroup (trfmParent.gameObject);
				}

				setEditMode (EditMode.Place);

				// copy placement/dungeon/room?
				if (groupIndex != -1)
				{
					_levelController.iSelectedGroupIndex = groupIndex;

					LevelController.ElementGroup elementGroup = _levelController.aElementGroups [_levelController.iSelectedGroupIndex];
					if (elementGroup.groupType == "dungeon")
					{
						selectAssetType (Globals.AssetType.Dungeon);
						setNewEditPart(_assetManager.assetTypeList[_assetType][0]); // reset to floor tile
						activateDungeonTool (elementGroup.dungeon, elementGroup);
					}
					else if (elementGroup.groupType == "placement")
					{
						selectAsset(elementGroup.part, trfmHit.rotation);
						activatePlacementTool (elementGroup.placement, _curEditPart, elementGroup);
					}
					else if (elementGroup.groupType == "room")
					{
						selectAsset(elementGroup.part, trfmHit.rotation);
						activateRoomTool (elementGroup.room, _curEditPart, elementGroup);
					}
				}
				else
				{
					LevelController.LevelElement element = _levelController.levelElements [trfmParent.gameObject.name];
					selectAsset(_assetManager.parts[element.partId], trfmHit.rotation);
				}
			}
		}

		// ------------------------------------------------------------------------
		private void selectElement(Transform trfmHit)
		{
            Transform trfmParent = getParentTransform(trfmHit);
            if (trfmParent == null) {
                return;
            }

            if (_levelController.levelElements.ContainsKey (trfmParent.gameObject.name))
			{
				bool isShift = (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift));

				// group select
				if (isShift) {
					_levelController.iSelectedGroupIndex = _levelController.findElementInGroup (trfmParent.gameObject);
                }

				// single object select
				if (_levelController.selectedElement.go != trfmParent.gameObject)
				{
					_levelController.resetElementComponents ();

					_levelController.selectElement (trfmParent.gameObject.name);

					Part part = _assetManager.parts [_levelController.selectedElement.partId];
					setMarkerScale (part);

                    setTransformGizmos (!_levelController.selectedElement.isLocked);

                    boundsLineRenderer.gameObject.SetActive (true);
                    if (_levelController.iSelectedGroupIndex != -1) {
                        boundsLineRenderer.updateBounds (_levelController.aElementGroups[_levelController.iSelectedGroupIndex].bounds);
                    }
                    else {
                        boundsLineRenderer.updateBounds (_levelController.selectedElementBounds);
                    }

                    PweMainMenu.Instance.showAssetInfoPanel (false);
                    PweMainMenu.Instance.showInstanceInfoPanel (true);

                    PweMainMenu.Instance.showInstanceInfo(_assetManager.parts[_levelController.selectedElement.partId]);

					PweMainMenu.Instance.showPlacementToolBox (false);
					PweMainMenu.Instance.showDungeonToolBox (false);
					PweMainMenu.Instance.showRoomToolBox (false);
				}
			}
			else
			{
				resetEditTools ();
			}

			setMarkerActive (_levelController.selectedElement.go != null);
		}

        // ------------------------------------------------------------------------
        private Transform getParentTransform(Transform trfmHit)
        {
            Transform trfmParent = trfmHit;

            if (trfmHit.gameObject.layer == 11) { // gizmo?
                return null;
            }

            _levelController.iSelectedGroupIndex = -1;

            while (trfmParent.parent != null && trfmParent.tag != "PartContainer") {
                trfmParent = trfmParent.parent;
            }

            // not an asset?
            if (trfmParent.tag != "PartContainer") {
                resetEditTools();
                return null;
            }

            return trfmParent;
        }

        // ------------------------------------------------------------------------
        private void selectAsset(Part part, Quaternion rotation)
        {
            selectAssetType(part.type);

            int index = 0;
            int i, len = _assetManager.assetTypeList[part.type].Count;
            for (i = 0; i < len; ++i) {
                if (_assetManager.assetTypeList[part.type][i].id == part.id) {
                    //Debug.Log ("    -> " + _assetManager.assetTypeList [part.type] [i].id);
                    index = i;
                    break;
                }
            }

            _assetManager.assetTypeIndex[part.type] = index;
            setNewEditPart(_assetManager.assetTypeList[part.type][index]);

            _goEditPart.transform.rotation = rotation;
        }

        // ------------------------------------------------------------------------
        private void resetEditTools()
		{
			_levelController.resetElementComponents ();

			resetSelectedElement ();
			resetCurPlacementTool ();
			resetCurDungeonTool ();
			resetCurRoomTool ();
		}

		// ------------------------------------------------------------------------
		private void positionSelectedElement()
		{
            if (_levelController.selectedElement.isLocked) {
                return;
            }
            
            if (_mousewheel != 0)
			{
				if (_timer > _lastMouseWheelUpdate)
				{
					Part part = _assetManager.parts [_levelController.selectedElement.partId];

					_lastMouseWheelUpdate = _timer + 0.2f;
					float dir = (_mousewheel > 0 ? 1 : -1);
                    float multiply = (part.type == Globals.AssetType.Dungeon ? 90f * dir : 15f * dir);

                    if (Input.GetKey (KeyCode.X)) {
						if (part.canRotate.x == 1) {
							_levelController.selectedElement.go.transform.Rotate (Vector3.right * multiply);
                            _levelController.updatedSelectedObjectBounds ();
                            boundsLineRenderer.updateBounds (_levelController.selectedElementBounds);
                        }
                    } else if (Input.GetKey (KeyCode.Y)) {
						if (part.canRotate.y == 1) {
							_levelController.selectedElement.go.transform.Rotate (Vector3.up * multiply);
                            _levelController.updatedSelectedObjectBounds ();
                            boundsLineRenderer.updateBounds (_levelController.selectedElementBounds);
                        }
                    } else if (Input.GetKey (KeyCode.Z)) {
						if (part.canRotate.z == 1) {
							_levelController.selectedElement.go.transform.Rotate (Vector3.forward * multiply);
                            _levelController.updatedSelectedObjectBounds ();
                            boundsLineRenderer.updateBounds (_levelController.selectedElementBounds);
                        }
                    }
					else if (Input.GetKey (KeyCode.C)) { // Scale
                        Vector3 scale = _levelController.selectedElement.go.transform.localScale;
                        scale.x += (scale.x * .1f * dir);
                        scale.y += (scale.y * .1f * dir);
                        scale.z += (scale.z * .1f * dir);
                        _levelController.selectedElement.go.transform.localScale = scale;
                        _levelController.updatedSelectedObjectBounds ();
                        boundsLineRenderer.updateBounds (_levelController.selectedElementBounds);
                    }
                    else {
						toggleSelectedElement (_mousewheel);
					}
				}
			}

            setMarkerPosition (_levelController.selectedElement.go.transform);
		}

		// ------------------------------------------------------------------------
		private void deleteSelectedElement()
		{
			_levelController.deleteSelectedElement ();

			PweMainMenu.Instance.setCubeCountText (_levelController.levelElements.Count);

			resetSelectedElement ();
			resetCurPlacementTool ();

			setMarkerActive (_goEditPart.activeSelf);
		}

		// ------------------------------------------------------------------------
		private void toggleSelectedElement(float mousewheel)
		{
			Part part = _assetManager.parts[_levelController.selectedElement.partId];

			int max = _assetManager.assetTypeList [part.type].Count;
			int i, index = 0;
			for (i = 0; i < max; ++i) {
				if (_assetManager.assetTypeList [part.type] [i].id == part.id) {
					index = i;
					break;
				}
			}

			if (mousewheel > 0) {
				if (++index >= max) {
					index = 0;
				}
			} else {
				if (--index < 0) {
					index = max - 1;
				}
			}

			Part newPart = _assetManager.assetTypeList [part.type] [index];

            PweMainMenu.Instance.showInstanceInfo (newPart);

            string name = _levelController.selectedElement.go.name;
			Vector3 pos = _levelController.selectedElement.go.transform.position;
			Quaternion rot = _levelController.selectedElement.go.transform.rotation;

			if (_levelController.levelElements.ContainsKey (name)) {
				
				Destroy (_levelController.selectedElement.go);

				LevelController.LevelElement element = _levelController.levelElements [name];
				element.partId = newPart.id;
				element.go = createPartAt (newPart.id, pos.x, pos.y, pos.z);
				element.go.transform.rotation = rot;
				element.go.name = name;

				_levelController.levelElements [name] = element;

				_levelController.selectElement (name);

				setMarkerScale (newPart);

                if (!_levelController.selectedElement.isLocked) {
                    gizmoTranslateScript.translateTarget = _levelController.selectedElement.go;
                    gizmoRotateScript.rotateTarget = _levelController.selectedElement.go;
                }

                _levelController.updatedSelectedObjectBounds ();
                boundsLineRenderer.updateBounds (_levelController.selectedElementBounds);
            }
        }

		// ------------------------------------------------------------------------
		private void toggleEditPart(float mousewheel)
		{
			int index = _assetManager.assetTypeIndex [_assetType];
			int max = _assetManager.assetTypeList [_assetType].Count;
						
			if (mousewheel > 0) {
				if (++index >= max) {
					index = 0;
				}
			} else {
				if (--index < 0) {
					index = max - 1;
				}
			}
			_assetManager.assetTypeIndex [_assetType] = index;

			setNewEditPart(_assetManager.assetTypeList[_assetType][index]);
        }

		// ------------------------------------------------------------------------
        private void placePart(Vector3 pos, Vector3 scale)
		{
			// Tools
			if (_toolsController.curPlacementTool != null && _toolsController.curPlacementTool.placementMode == PlacementTool.PlacementMode.Mount) {
				if (!_toolsController.curPlacementTool.inverse) {
					pos.y += _toolsController.curPlacementTool.interval * 0.5f;
				}
			}

			if (_toolsController.curPlacementTool == null && _toolsController.curDungeonTool == null && _toolsController.curRoomTool == null)
			{
				LevelController.LevelElement element = LevelController.Instance.createLevelElement(null, _curEditPart.id);
				element.go = createPartAt (_curEditPart.id, pos.x, pos.y, pos.z);
                element.go.transform.rotation = _goEditPart.transform.rotation;
                element.go.transform.localScale = scale;

                if (_curEditPart.type == Globals.AssetType.Floor) {
                    element.go.AddComponent<Teleportable>();
                }

                _levelController.setComponents (element.go, true, _curEditPart.usesGravity);

				_levelController.levelElements.Add (element.go.name, element);
			}

			// add tool objects
			if (_toolsController.curPlacementTool != null) {
				if (_toolsController.curPlacementTool.placementMode != PlacementTool.PlacementMode.None) {
					placePattern ();
				}
				resetCurPlacementTool ();
			}
			else if (_toolsController.curDungeonTool != null) {
				if (_toolsController.curDungeonTool.dungeonPreset != DungeonTool.DungeonPreset.None) {
					placeDungeon ();
				}
				resetCurDungeonTool ();
			}
			else if (_toolsController.curRoomTool != null) {
				if (_toolsController.curRoomTool.roomPattern != RoomTool.RoomPattern.None) {
					placeRoom ();
				}
				resetCurRoomTool ();
			}

			PweMainMenu.Instance.setCubeCountText (_levelController.levelElements.Count);
        }

		// ------------------------------------------------------------------------
		private void placePattern()
		{
			List<GameObject> aGOs = new List<GameObject> ();

			int i, len = _toolsController.curPlacementTool.elements.Count;
			for (i = 0; i < len; ++i) {

				GameObject go = _toolsController.curPlacementTool.elements [i].go;
				if (go != null) {
					go.transform.SetParent (container);
					go.name = "part_" + (_iCounter++).ToString ();

                    _levelController.setComponents (go, true, _curEditPart.usesGravity);

					LevelController.LevelElement elementTool = LevelController.Instance.createLevelElement(go, _curEditPart.id);

                    _levelController.levelElements.Add (go.name, elementTool);

					aGOs.Add (go);
				}
			}

			if (aGOs.Count > 0) {
				LevelController.ElementGroup elementGroup = new LevelController.ElementGroup ();
				elementGroup.groupType = "placement";
				elementGroup.part = _toolsController.curPlacementTool.curPart;
				elementGroup.placement = _toolsController.curPlacementTool.placementMode;
				elementGroup.gameObjects = new List<GameObject> ();
				len = aGOs.Count;
				for (i = 0; i < len; ++i) {
					elementGroup.gameObjects.Add (aGOs [i]);
				}
				elementGroup.radius   = _toolsController.curPlacementTool.radius;
				elementGroup.interval = _toolsController.curPlacementTool.interval;
				elementGroup.density  = _toolsController.curPlacementTool.density;
				elementGroup.inverse  = _toolsController.curPlacementTool.inverse;
                _levelController.setElementGroupBounds (ref elementGroup);
                _levelController.aElementGroups.Add (elementGroup);
			}
		}

		// ------------------------------------------------------------------------
		private void placeDungeon()
		{
			List<GameObject> aGOs = new List<GameObject> ();

			int i, len = _toolsController.curDungeonTool.dungeonElements.Count;
			for (i = 0; i < len; ++i) {

				GameObject go = _toolsController.curDungeonTool.dungeonElements [i].go;
				if (go != null) {
					go.transform.SetParent (container);
					go.name = "part_" + (_iCounter++).ToString ();

                    _levelController.setComponents (go, true, false);

					LevelController.LevelElement elementTool = LevelController.Instance.createLevelElement(go, _toolsController.curDungeonTool.dungeonElements [i].partId);
                    elementTool.isLocked = true;

                    _levelController.levelElements.Add (go.name, elementTool);

					aGOs.Add (go);
				}
			}

			if (aGOs.Count > 0) {
				LevelController.ElementGroup elementGroup = new LevelController.ElementGroup ();
				elementGroup.groupType = "dungeon";
				elementGroup.dungeon = _toolsController.curDungeonTool.dungeonPreset;
				elementGroup.gameObjects = new List<GameObject> ();
				len = aGOs.Count;
				for (i = 0; i < len; ++i) {
					elementGroup.gameObjects.Add (aGOs [i]);
				}
				elementGroup.width   = _toolsController.curDungeonTool.width;
				elementGroup.height  = _toolsController.curDungeonTool.height;
				elementGroup.depth   = _toolsController.curDungeonTool.depth;
				elementGroup.ceiling = _toolsController.curDungeonTool.ceiling;
                _levelController.setElementGroupBounds (ref elementGroup);
                _levelController.aElementGroups.Add (elementGroup);
			}
		}

		// ------------------------------------------------------------------------
		private void placeRoom()
		{
			List<GameObject> aGOs = new List<GameObject> ();

			int i, len = _toolsController.curRoomTool.roomElements.Count;
			for (i = 0; i < len; ++i) {

				GameObject go = _toolsController.curRoomTool.roomElements [i].go;
				if (go != null) {
					go.transform.SetParent (container);
					go.name = "part_" + (_iCounter++).ToString ();

                    _levelController.setComponents (go, true, _curEditPart.usesGravity);

					LevelController.LevelElement elementTool = LevelController.Instance.createLevelElement(go, _curEditPart.id);

                    _levelController.levelElements.Add (go.name, elementTool);

					aGOs.Add (go);
				}
			}

			if (aGOs.Count > 0) {
				LevelController.ElementGroup elementGroup = new LevelController.ElementGroup ();
				elementGroup.groupType = "room";
				elementGroup.part = _toolsController.curRoomTool.curPart;
				elementGroup.room = _toolsController.curRoomTool.roomPattern;
				elementGroup.gameObjects = new List<GameObject> ();
				len = aGOs.Count;
				for (i = 0; i < len; ++i) {
					elementGroup.gameObjects.Add (aGOs [i]);
				}
				elementGroup.width  = _toolsController.curRoomTool.width;
				elementGroup.height = _toolsController.curRoomTool.height;
                _levelController.setElementGroupBounds (ref elementGroup);
                _levelController.aElementGroups.Add (elementGroup);
			}
		}

		// ------------------------------------------------------------------------
		private void fillX(Vector3 pos)
		{
			int lenZ = (int)((float)levelSize.z / _curEditPart.d);
			int lenY = (int)((float)levelSize.y / _curEditPart.h);
			int z, y;
			for (z = 0; z < lenZ; ++z) {
				for (y = 0; y < lenY; ++y) {
					pos.z = _curEditPart.d / 2 + (z * _curEditPart.d);
					pos.y = (y * _curEditPart.h);
					placePart (pos, Vector3.one);
				}
			}
		}

        // ------------------------------------------------------------------------
        public void fillY(Vector3 pos)
		{
			int lenX = (int)((float)levelSize.x / _curEditPart.w);
			int lenZ = (int)((float)levelSize.z / _curEditPart.d);
			int x, z;
			for (x = 0; x < lenX; ++x) {
				for (z = 0; z < lenZ; ++z) {
					pos.x = _curEditPart.w / 2 + (x * _curEditPart.w);
					pos.z = _curEditPart.d / 2 + (z * _curEditPart.d);
					placePart (pos, Vector3.one);
				}
			}
		}

		// ------------------------------------------------------------------------
		private void fillZ(Vector3 pos)
		{
			int lenX = (int)((float)levelSize.x / _curEditPart.w);
			int lenY = (int)((float)levelSize.y / _curEditPart.h);
			int x, y;
			for (x = 0; x < lenX; ++x) {
				for (y = 0; y < lenY; ++y) {
					pos.x = _curEditPart.w / 2 + (x * _curEditPart.w);
					pos.y = (y * _curEditPart.h);
					placePart (pos, Vector3.one);
				}
			}
		}

		// ------------------------------------------------------------------------
		public GameObject createPartAt(Globals.PartList partId, float x, float y, float z)
		{
            GameObject go = null;

            if (!_assetManager.parts.ContainsKey(partId)) {
                return go;
            }

            if (_assetManager.parts[partId].prefab != null) {
                go = Instantiate(_assetManager.parts[partId].prefab);
                if (go != null) {
                    go.name = "part_" + (_iCounter++).ToString();
                    go.transform.SetParent(container);
                    go.transform.position = new Vector3(x, y, z);
                }
            }

            return go;
        }

        // ------------------------------------------------------------------------
        private void setWalls()
        {
			_trfmMarkerX  = trfmWalls.Find ("marker_x");
			_trfmMarkerY  = trfmWalls.Find ("marker_y");
			_trfmMarkerZ  = trfmWalls.Find ("marker_z");
			_trfmMarkerX2 = trfmWalls.Find ("marker_x2");
			_trfmMarkerY2 = trfmWalls.Find ("marker_y2");
			_trfmMarkerZ2 = trfmWalls.Find ("marker_z2");

            float w = (float)levelSize.x;
            float h = (float)levelSize.y;
            float d = (float)levelSize.z;

            setWall("wall_f", "bounds_f", new Vector3(w, h, 1), new Vector3(w / 2f, h / 2f, d), new Vector2(w, h));
            setWall("wall_b", "bounds_b", new Vector3(w, h, 1), new Vector3(w / 2f, h / 2f, 0), new Vector2(w, h));
            setWall("wall_l", "bounds_l", new Vector3(d, h, 1), new Vector3(0, h / 2f, d / 2f), new Vector2(d, h));
            setWall("wall_r", "bounds_r", new Vector3(d, h, 1), new Vector3(w, h / 2f, d / 2f), new Vector2(d, h));
            setWall("wall_u", "bounds_u", new Vector3(w, d, 1), new Vector3(w / 2f, h, d / 2f), new Vector2(w, d));
            setWall("wall_d", "bounds_d", new Vector3(w, d, 1), new Vector3(w / 2f, 0, d / 2f), new Vector2(w, d));
        }

        // ------------------------------------------------------------------------
        private void setWall(string name, string boundsName, Vector3 scale, Vector3 pos, Vector2 matScale) {

            Transform child = trfmWalls.Find(name);
            if (child != null) {
                child.localScale = scale;
                child.localPosition = pos;

                Renderer r = child.GetComponent<MeshRenderer>();
                if (r != null) {
                    r.material.mainTextureScale = matScale;
                }

                child.gameObject.isStatic = true;
            }

            child = trfmBounds.Find(boundsName);
            if (child != null) {
                child.localScale = scale;
                child.localPosition = pos;
                child.gameObject.isStatic = true;
            }
        }

		// ------------------------------------------------------------------------
		private void resetSelectedElement()
		{
			_levelController.iSelectedGroupIndex = -1;

			_levelController.resetSelectedElement ();

            setTransformGizmos (false);

            boundsLineRenderer.gameObject.SetActive (false);

			PweMainMenu.Instance.showAssetInfoPanel (false);
            PweMainMenu.Instance.showInstanceInfoPanel (false);

            PweMainMenu.Instance.setSpecialHelpText ("");
        }

        // ------------------------------------------------------------------------
        public void setTransformGizmos(bool state)
        {
            GameObject go = (state ? LevelController.Instance.selectedElement.go : null);

            gizmoTranslateScript.translateTarget = go;
            gizmoTranslateScript.gameObject.SetActive (state);

            if (state) {
                gizmoTranslateScript.init ();
                selectTransformTool (PweMainMenu.Instance.iSelectedTool);
            }
            else {
                gizmoTranslateScript.reset ();
                if (_groupEventHandlerSet) {
                    gizmoTranslateScript.positionChanged -= onSelectedObjectPositionChanged;
                    _groupEventHandlerSet = false;
                }
            }

            gizmoRotateScript.rotateTarget = null;
            gizmoRotateScript.gameObject.SetActive (false);
        }

        // ------------------------------------------------------------------------
        // Marker stuff
        // ------------------------------------------------------------------------
        private void setMarkerActive(bool state)
		{
			_trfmMarkerX.gameObject.SetActive (state);
			_trfmMarkerY.gameObject.SetActive (state);
			_trfmMarkerZ.gameObject.SetActive (state);

			_trfmMarkerX2.gameObject.SetActive (state);
			_trfmMarkerY2.gameObject.SetActive (state);
			_trfmMarkerZ2.gameObject.SetActive (state);
		}

		private void setMarkerScale(Part part)
		{
			_trfmMarkerX.localScale = new Vector3 (part.d, part.h, 1);
			_trfmMarkerY.localScale = new Vector3 (part.w, part.d, 1);
			_trfmMarkerZ.localScale = new Vector3 (part.w, part.h, 1);

			_trfmMarkerX2.localScale = new Vector3 (part.d, part.h, 1);
			_trfmMarkerY2.localScale = new Vector3 (part.w, part.d, 1);
			_trfmMarkerZ2.localScale = new Vector3 (part.w, part.h, 1);
		}

		private void setMarkerPosition(Transform trfm)
		{
			_trfmMarkerX.position = new Vector3 (0.01f, trfm.position.y, trfm.position.z);
			_trfmMarkerY.position = new Vector3 (trfm.position.x, 0.01f, trfm.position.z);
			_trfmMarkerZ.position = new Vector3 (trfm.position.x, trfm.position.y, 0.01f);

			_trfmMarkerX2.position = new Vector3 (levelSize.x - 0.01f, trfm.position.y, trfm.position.z);
			_trfmMarkerY2.position = new Vector3 (trfm.position.x, levelSize.y - 0.01f, trfm.position.z);
			_trfmMarkerZ2.position = new Vector3 (trfm.position.x, trfm.position.y, levelSize.z - 0.01f);
		} 
	}
}