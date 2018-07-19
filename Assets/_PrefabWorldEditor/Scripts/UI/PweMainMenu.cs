//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//using System.Collections.Generic;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
using System;

using AssetsShared;

//using RTEditor;

namespace PrefabWorldEditor
{
	public class PweMainMenu : MonoSingleton<PweMainMenu>
    {
		public GameObject goTransformSelection;
		public GameObject goAssetTypeSelection;
		public GameObject goPlacementToolButtons;
		public GameObject goDungeonToolButtons;
		public GameObject goRoomToolButtons;

		public Transform panelTools;
        public Transform panelFileMenu;
		public Transform panelLevelMenu;
        public Transform panelLightToggles;

        public Transform panelAssetInfo;
		public UIAssetInfo assetInfo;

        public Transform panelInstanceInfo;
        public UIInstanceInfo instanceInfo;

        public Transform blocker;
        public Transform panelPopup;

		public Button btnModePlay;
		public Button btnModeBuild;
		public Button btnModeSelect;
		public Button btnModeClear;
		public Button btnUNDO;

        public Toggle toggleAmbientLight;
        public Toggle toggleSpotLight;

        //public Image imgMaterialHilight;
        //public RawImage imgSelectedItem;

        public Button btnAssetFloors;
		public Button btnAssetWalls;
		public Button btnAssetChunks;
		public Button btnAssetProps;
		public Button btnAssetDungeons;
        public Button btnAssetInteractables;

        public Button btnPlacementToolCircle;
		public Button btnPlacementToolQuad;
		public Button btnPlacementToolMount;
		public Button btnPlacementToolCube;

		public Button btnDungeonToolRoom;
		public Button btnDungeonToolMaze;
		public Button btnDungeonToolRandom;
		public Button btnDungeonToolStaircase;

		public Button btnRoomToolDefault;

		public Text txtFileInfo;
		public Text txtLevelName;
		public Text txtCubeCount;
		public Text txtInstructions;
		public Text txtAssetName;
		public Text txtSpecialHelp;

        public Image imgMove;
        public Image imgRotate;
        public Image imgScale;
        public Image imgVolume;

        //private EditorGizmoSystem _gizmoSystem;
            
        private Dropdown _trfmDropDownFile = null;
		private Text _txtPanelFile = null;
        private int _iDropDownFileOptions = 0;

		private Dropdown _trfmDropDownLevel = null;
		private Text _txtPanelLevel = null;
		private int _iDropDownLevelOptions = 0;
		//private int _iSelectedLevel = -1;

        private int _iSelectedTool = -1;
		private int _iSelectedAssetType = -1;

		private Vector3 _v3DigSettings = new Vector3(1,1,1);
		public Vector3 v3DigSettings {
			get { return _v3DigSettings; }
		}

		//private float _lastMouseWheelUpdate;

		private int _iSelectedMaterial = 0;
		public int iSelectedMaterial {
			get { return _iSelectedMaterial; }
		}

		private PwePopup _popup;
		public PwePopup popup {
            get { return _popup; }
        }

		public int iSelectedTool {
			get { return _iSelectedTool; }
		}

		public int iSelectedAssetType {
			get { return _iSelectedAssetType; }
		}

		#region SystemMethods

        void Awake() {

			if (txtFileInfo) {
				txtFileInfo.text = Globals.version;
			}

            if (blocker) {
                blocker.gameObject.SetActive(false);
            }
            if (panelPopup) {
                panelPopup.gameObject.SetActive(false);
				_popup = panelPopup.GetComponent<PwePopup>();
            }

			Transform trfmMenu;
			Transform trfmText;

            if (panelFileMenu != null) {
				trfmText = panelFileMenu.Find("Text");
				if (trfmText != null) {
					_txtPanelFile = trfmText.GetComponent<Text> ();
				}
                trfmMenu = panelFileMenu.Find("DropdownFile");
                if (trfmMenu) {
                    _trfmDropDownFile = trfmMenu.GetComponent<Dropdown>();
                    if (_trfmDropDownFile) {
                        _iDropDownFileOptions = _trfmDropDownFile.options.Count;
						//Debug.Log ("_iDropDownFileOptions: "+_iDropDownFileOptions);
                    }
                }
            }

			if (panelLevelMenu != null) {
				trfmText = panelLevelMenu.Find("Text");
				if (trfmText != null) {
					_txtPanelLevel = trfmText.GetComponent<Text> ();
				}
				trfmMenu = panelLevelMenu.Find("DropdownFile");
				if (trfmMenu) {
					_trfmDropDownLevel = trfmMenu.GetComponent<Dropdown>();
					if (_trfmDropDownLevel) {
						_iDropDownLevelOptions = _trfmDropDownLevel.options.Count;
						//Debug.Log ("_iDropDownLevelOptions: "+_iDropDownLevelOptions);
					}
				}
				//panelLevelMenu.gameObject.SetActive (false);
			}

            //_lastMouseWheelUpdate = 0;
        }

		#endregion

		#region PublicMethods

		public void init()
		{
			onSelectTransformTool(0);

			setLevelNameText ("New Level");
			setUndoButton (false);

			_txtPanelFile.color = Color.black;
			_trfmDropDownFile.interactable = true;

			_txtPanelLevel.color = Color.black;
			_trfmDropDownLevel.interactable = true;
		}

		//
		public void addLevelToMenu(string name) {
			if (_trfmDropDownLevel != null) {
				_trfmDropDownLevel.options.Add(new Dropdown.OptionData() { text = name });
				_iDropDownLevelOptions++;
			}
		}

		public void setLevelNameText(string name) {
			if (txtLevelName != null) {
				txtLevelName.text = name;
			}
		}

		public void setInstructionsText(string s) {
			if (txtInstructions != null) {
				txtInstructions.text = s;
			}
		}

		public void setAssetNameText(string s) {
			if (txtAssetName != null) {
				txtAssetName.text = s;
			}
		}

		public void setSpecialHelpText(string s) {
			if (txtSpecialHelp != null) {
				txtSpecialHelp.text = s;
			}
		}

		public void setCubeCountText(int count) {
			if (txtCubeCount != null) {
				txtCubeCount.text = "Objects: " + String.Format("{0:0,0}", count);
			}
		}
		public void setCubeCountText(string s) {
			if (txtCubeCount != null) {
				txtCubeCount.text = s;
			}
		}

        // ------------------------------------------------------------------------
        public void onButtonModePlayClicked() {
			PrefabLevelEditor.Instance.setEditMode(PrefabLevelEditor.EditMode.Play);
		}
		public void onButtonModeBuildClicked() {
			PrefabLevelEditor.Instance.setEditMode(PrefabLevelEditor.EditMode.Place);
		}
		public void onButtonModeSelectClicked() {
			PrefabLevelEditor.Instance.setEditMode(PrefabLevelEditor.EditMode.Transform);
		}
		public void onButtonModeClearClicked() {
			PrefabLevelEditor.Instance.clearLevel();
		}

        // ------------------------------------------------------------------------
        public void setModeButtons(PrefabLevelEditor.EditMode mode)
		{
			btnModePlay.interactable   = (mode != PrefabLevelEditor.EditMode.Play);
			btnModeBuild.interactable  = (mode != PrefabLevelEditor.EditMode.Place);
			btnModeSelect.interactable = (mode != PrefabLevelEditor.EditMode.Transform);
			btnModeClear.interactable  = (mode != PrefabLevelEditor.EditMode.Play);
		}

        // ------------------------------------------------------------------------
        public void setAssetTypeButtons(Globals.AssetType type)
		{
			btnAssetFloors.interactable   = (type != Globals.AssetType.Floor);
			btnAssetWalls.interactable    = (type != Globals.AssetType.Wall);
			btnAssetChunks.interactable   = (type != Globals.AssetType.Chunk);
			btnAssetProps.interactable    = (type != Globals.AssetType.Prop);
			btnAssetDungeons.interactable = (type != Globals.AssetType.Dungeon);
            btnAssetInteractables.interactable = (type != Globals.AssetType.Lights);
        }

        // ------------------------------------------------------------------------
        public void setPlacementToolButtons(PlacementTool.PlacementMode mode)
		{
			btnPlacementToolCircle.interactable = (mode != PlacementTool.PlacementMode.Circle);
			btnPlacementToolQuad.interactable   = (mode != PlacementTool.PlacementMode.Quad);
			btnPlacementToolMount.interactable  = (mode != PlacementTool.PlacementMode.Mount);
			btnPlacementToolCube.interactable   = (mode != PlacementTool.PlacementMode.Cube);
		}

        // ------------------------------------------------------------------------
        public void setDungeonToolButtons(DungeonTool.DungeonPreset preset)
		{
			btnDungeonToolRoom.interactable      = (preset != DungeonTool.DungeonPreset.Room);
			btnDungeonToolMaze.interactable      = (preset != DungeonTool.DungeonPreset.Maze);
			btnDungeonToolRandom.interactable    = (preset != DungeonTool.DungeonPreset.Random);
			btnDungeonToolStaircase.interactable = (preset != DungeonTool.DungeonPreset.Staircase);
		}

        // ------------------------------------------------------------------------
        public void setRoomToolButtons(RoomTool.RoomPattern pattern)
		{
			btnRoomToolDefault.interactable = (pattern != RoomTool.RoomPattern.Default);
		}

        // ------------------------------------------------------------------------
        public void onButtonUNDOClicked() {
			//PrefabLevelEditor.Instance.undoLastActions ();
		}
		public void setUndoButton(bool state) {
			if (btnUNDO != null) {
				btnUNDO.interactable = state;
			}
		}

        // ------------------------------------------------------------------------
        public void showTransformBox(bool state) {
			if (goTransformSelection != null) {
				goTransformSelection.SetActive (false);
			}
		}

		public void showPlacementToolBox(bool state) {
			if (goPlacementToolButtons != null) {
				goPlacementToolButtons.SetActive (state);
			}
		}

		public void showDungeonToolBox(bool state) {
			if (goDungeonToolButtons != null) {
				goDungeonToolButtons.SetActive (state);
			}
		}

		public void showRoomToolBox(bool state) {
			if (goRoomToolButtons != null) {
				goRoomToolButtons.SetActive (state);
			}
		}

		public void showAssetTypeBox(bool state) {
			if (goAssetTypeSelection != null) {
				goAssetTypeSelection.SetActive (state);
			}
		}

		public void showAssetInfoPanel(bool state) {
			if (panelAssetInfo != null) {
				panelAssetInfo.gameObject.SetActive(state);
			}
		}

        public void showInstanceInfoPanel(bool state) {
            if (panelInstanceInfo != null) {
                panelInstanceInfo.gameObject.SetActive(state);
            }
        }

        public void setAmbientLightToggle(bool state) {
            if (toggleAmbientLight != null) {
                toggleAmbientLight.isOn = state;
            }
        }

        // ------------------------------------------------------------------------
        public void showAssetInfo(PrefabLevelEditor.Part part)
        {
            PrefabLevelEditor editor = PrefabLevelEditor.Instance;

            if (editor.editMode == PrefabLevelEditor.EditMode.Place) {
                setAssetNameText((editor.assetTypeIndex[editor.assetType] + 1).ToString() + " / " + editor.assetTypeList[editor.assetType].Count.ToString());
            }
            else {
                setAssetNameText("");
            }
            assetInfo.init(part, LevelController.Instance.selectedElement);

            showAssetInstructions(part);
        }

        public void showInstanceInfo(PrefabLevelEditor.Part part)
        {
            setAssetNameText("");

            instanceInfo.init(part, LevelController.Instance.selectedElement);

            showAssetInstructions(part);
        }

        public void showAssetInstructions(PrefabLevelEditor.Part part)
        {
            PrefabLevelEditor editor = PrefabLevelEditor.Instance;

            string s = "";

            s = "Mousewheel + ";
            if (editor.toolsController.curDungeonTool != null) {
                s += "'1'/'2'/'3' = change preset settings";
            }
            else if (editor.toolsController.curPlacementTool != null) {
                s += "'1'/'2'/'3' = change pattern settings";
            }
            else if (editor.toolsController.curRoomTool != null) {
                s += "'1'/'2' = change size settings";
            }
            else if (editor.assetType == Globals.AssetType.Floor) {
                s = "Press left mouse button + shift key for a 'Floor Fill'!";
            }
            else if (editor.assetType == Globals.AssetType.Wall) {
                s = "Press left mouse button + shift key for a 'Wall Fill'!";
            }
            else {
                if (part.canRotate != Vector3Int.zero) {
                    s = "Mousewheel + ";
                    s += (part.canRotate.x == 1 ? "'X'" : "");
                    s += (part.canRotate.y == 1 ? "/ 'Y'" : "");
                    s += (part.canRotate.z == 1 ? "/ 'Z'" : "");
                    s += " = rotate object";
                }
                s += "\nMousewheel + 'C' = scale object";
            }

            setSpecialHelpText(s);
        }

        #endregion

        //

        #region PrivateMethods

        // ------------------------------------------------------------------------
        private void selectTool(int toolId) {

			if (_iSelectedTool != toolId) {

				setToolButtonImage (imgMove,   (toolId == 0 ? "Textures/Tools/icon-move-selected" : "Textures/Tools/icon-move"));
				setToolButtonImage (imgRotate, (toolId == 1 ? "Textures/Tools/icon-rotate-selected" : "Textures/Tools/icon-rotate"));
				setToolButtonImage (imgScale,  (toolId == 2 ? "Textures/Tools/icon-scale-selected" : "Textures/Tools/icon-scale"));
				setToolButtonImage (imgVolume, (toolId == 3 ? "Textures/Tools/icon-volume-selected" : "Textures/Tools/icon-volume"));

				_iSelectedTool = toolId;

				PrefabLevelEditor.Instance.selectTransformTool (_iSelectedTool);
			}
        }

        // ------------------------------------------------------------------------
        private void setToolButtonImage(Image img, string spriteName) {

            if (img) {
                img.sprite = Resources.Load<Sprite>(spriteName);
            }
        }

		// ---------------------------------------------------------------------------------------------
		private void selectAssetType(int value, Globals.AssetType type) {

			if (_iSelectedAssetType != value) {

				setAssetTypeButtons (type);

				_iSelectedAssetType = value;

				PrefabLevelEditor.Instance.selectAssetType (type);
			}
		}

		// ---------------------------------------------------------------------------------------------
		private void selectPlacementTool(PlacementTool.PlacementMode mode)
		{
			setPlacementToolButtons (mode);

			PrefabLevelEditor.Instance.selectPlacementTool (mode);
		}

		// ---------------------------------------------------------------------------------------------
		private void selectDungeonTool(DungeonTool.DungeonPreset preset)
		{
			setDungeonToolButtons (preset);

			PrefabLevelEditor.Instance.selectDungeonTool (preset);
		}

		// ---------------------------------------------------------------------------------------------
		private void selectRoomTool(RoomTool.RoomPattern pattern)
		{
			setRoomToolButtons (pattern);

			PrefabLevelEditor.Instance.selectRoomTool (pattern);
		}

		// ---------------------------------------------------------------------------------------------
		/// <summary>
		/// Hack to set selected option to an invalid value!
		/// </summary>
        private void resetDropDown(Dropdown dropDown) {

            if (dropDown) {
                dropDown.options.Add(new Dropdown.OptionData() { text = "" });
                int last = dropDown.options.Count - 1;
                dropDown.value = last;
                dropDown.options.RemoveAt(last);
            }
        }

		// ---------------------------------------------------------------------------------------------
		// New Level
		// ---------------------------------------------------------------------------------------------
		private void showNewLevelDialog()
		{
            PrefabLevelEditor.Instance.setEditMode(PrefabLevelEditor.EditMode.Transform);
            //EditorObjectSelection.Instance.ClearSelection(false);

            _popup.showPopup (Globals.PopupMode.NewLevel, "New Level", "", createNewLevel);
		}

		private void createNewLevel(int buttonId)
		{
			int w = int.Parse( (_popup.newLevelWidth != "" && _popup.newLevelWidth != null ? _popup.newLevelWidth : "0") );
			int h = int.Parse( (_popup.newLevelHeight != "" && _popup.newLevelHeight != null ? _popup.newLevelHeight : "0") );
			int d = int.Parse( (_popup.newLevelDepth != "" && _popup.newLevelDepth != null ? _popup.newLevelDepth : "0") );

			_popup.hide();

			if (buttonId == 1)
			{
				if (w >= 4 && w <= 36 && h >= 4 && h <= 36 && d >= 4 && d <= 36) {
					PrefabLevelEditor.Instance.newLevelWithDimensions (w, h, d);
				} else {
					_popup.showPopup (Globals.PopupMode.Notification, "Error", "Invalid Input!");
				}					
			}
		}

        // ---------------------------------------------------------------------------------------------
        // Load Test Level
        // ---------------------------------------------------------------------------------------------
        private void showLoadTestLevelDialog() {
            PrefabLevelEditor.Instance.setEditMode(PrefabLevelEditor.EditMode.Transform);
            _popup.showPopup(Globals.PopupMode.Confirmation, "Load Test Level", "Are you sure?\nAll unsaved changes will be lost!", loadTestLevel);
        }

        public void loadTestLevel(int buttonId) {
            _popup.hide();
            if (buttonId == 1) {
                TextAsset jsonAsset = Resources.Load<TextAsset>("Data/editor-level");
                if (jsonAsset != null) {
                    LevelData.Instance.loadLevelFromJson(jsonAsset.text);
                }
            }
        }
        
        // ---------------------------------------------------------------------------------------------
        // Load File
        // ---------------------------------------------------------------------------------------------
        private void showLoadFileDialog() {

			//EditorObjectSelection.Instance.ClearSelection(false);
			PrefabLevelEditor.Instance.setEditMode (PrefabLevelEditor.EditMode.Transform);

			#if UNITY_WEBGL
				_popup.showPopup(
					Globals.PopupMode.Notification,
					Globals.txtSorry,
                    Globals.txtWarningLoadingFiles,
					webGLPopupCallbackLoad
				);
				return;
			#endif

			_popup.showPopup (Globals.PopupMode.Confirmation, "Load Level", "Are you sure?\nAll unsaved changes will be lost!", showLoadFileBrowser);
        }

		//
		private void webGLPopupCallbackLoad(int buttonId) {
			_popup.hide ();
		}

		//
		private void showLoadFileBrowser(int buttonId) {

			_popup.hide();

			if (buttonId == 2) {
				return;
			}

			FileBrowser.OpenFilePanel("Open file Title", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), new string[] { "json" }, null, (bool canceled, string filePath) => {
				if (!canceled) {
					LevelData.Instance.loadLevelDataFromFile(filePath);
				}
			});
		}

		// ---------------------------------------------------------------------------------------------
		// Save Level
		// ---------------------------------------------------------------------------------------------
        private void showSaveFileDialog() {

			PrefabLevelEditor.Instance.setEditMode (PrefabLevelEditor.EditMode.Transform);

			#if UNITY_WEBGL
				_popup.showPopup(
					Globals.PopupMode.Notification,
                    Globals.txtSorry,
                    Globals.txtWarningSavingFiles,
                    webGLPopupCallbackSave
                );
				return;
			#endif

			_popup.showPopup(Globals.PopupMode.Input, "Save Level", "Level Name: (50 chars max)", "Enter Level Name...", showSaveFileDialogContinue);
        }

		//
		private void webGLPopupCallbackSave(int buttonId) {
			_popup.hide ();
		}

		//
		private void showSaveFileDialogContinue(int buttonId) {

			if (buttonId == 1)
			{
				string levelName = _popup.inputText;
				_popup.hide ();
				if (levelName == null || levelName == "") {
					levelName = Globals.defaultLevelName;
				}
				setLevelNameText (levelName);

				FileBrowser.SaveFilePanel ("Save Level", "Save Level", Environment.GetFolderPath (Environment.SpecialFolder.Desktop), levelName, new string[] { "json" }, null, (bool canceled, string filePath) => {
					if (!canceled) {
						LevelData.Instance.saveLevelData (filePath, levelName);
					}
				});
			}
			else
			{
				_popup.hide ();
			}
		}

		//
        /*private void onGameObjectClicked(GameObject clickedObject) {

            //Debug.Log("onGameObjectClicked "+clickedObject.name);
			if (clickedObject.GetComponent<Rigidbody> () != null) {
				clickedObject.GetComponent<Rigidbody> ().useGravity = false;
			}
        }*/

        #endregion

		//
        public void onSelectTransformTool(int value) {
            if (value == 0) {
                //_gizmoSystem.ActiveGizmoType = GizmoType.Translation;
                selectTool(0);
            }
            else if (value == 1) {
                //_gizmoSystem.ActiveGizmoType = GizmoType.Rotation;
                selectTool(1);
            }
            else if (value == 2) {
                //_gizmoSystem.ActiveGizmoType = GizmoType.Scale;
                selectTool(2);
            }
            else if (value == 3) {
                //_gizmoSystem.ActiveGizmoType = GizmoType.VolumeScale;
                selectTool(3);
            }
        }

		//
		public void onSelectAssetType(int value) {

            string[] aAssetTypeNames = System.Enum.GetNames(typeof(Globals.AssetType));
            Array aAssetTypeValues   = System.Enum.GetValues(typeof(Globals.AssetType));
            int i, len = aAssetTypeNames.Length;
            for (i = 0; i < len; ++i) {
                if (i == value) {
                    selectAssetType(value, (Globals.AssetType)aAssetTypeValues.GetValue(i));
                    break;
                }
            }
		}

        //
        public void onToggleAmbientLight(bool state) {
            PrefabLevelEditor.Instance.goLights.SetActive(state);
        }

        //
        public void onToggleSpotLight(bool state) {
            PrefabLevelEditor.Instance.setSpotLights(state);
        }

        //
        public void onSelectPlacementTool(int value) {

			if (value == 0) {
				selectPlacementTool(PlacementTool.PlacementMode.Circle);
			}
			else if (value == 1) {
				selectPlacementTool(PlacementTool.PlacementMode.Quad);
			}
			else if (value == 2) {
				selectPlacementTool(PlacementTool.PlacementMode.Mount);
			}
			else if (value == 3) {
				selectPlacementTool(PlacementTool.PlacementMode.Cube);
			}
		}

		//
		public void onSelectDungeonTool(int value) {

			if (value == 0) {
				selectDungeonTool(DungeonTool.DungeonPreset.Room);
			}
			else if (value == 1) {
				selectDungeonTool(DungeonTool.DungeonPreset.Maze);
			}
			else if (value == 2) {
				selectDungeonTool(DungeonTool.DungeonPreset.Random);
			}
			else if (value == 3) {
				selectDungeonTool(DungeonTool.DungeonPreset.Staircase);
			}
		}

		//
		public void onSelectRoomTool(int value) {

			if (value == 0) {
				selectRoomTool(RoomTool.RoomPattern.Default);
			}
		}

        // -------------------------------------------------------------------------------------
        /*public void onSliderSnowLevelChange(Single value) {

            LevelController.Instance.changeSnowLevel((float)value);
        }*/

        // -------------------------------------------------------------------------------------
        public void onPointerDownFile(BaseEventData data) {
            if (_trfmDropDownFile) {
                resetDropDown(_trfmDropDownFile);
                PrefabLevelEditor.Instance.setEditMode(PrefabLevelEditor.EditMode.Transform);
            }
        }

		public void onPointerDownLevel(BaseEventData data) {
			if (_trfmDropDownLevel) {
				resetDropDown(_trfmDropDownLevel);
                PrefabLevelEditor.Instance.setEditMode(PrefabLevelEditor.EditMode.Transform);
            }
		}

		//
        public void onDropDownFileValueChanged(int value) {
            if (_trfmDropDownFile && value < _iDropDownFileOptions) {
                if (value == 0) {
					showLoadFileDialog();
				} else if (value == 1) {
					showSaveFileDialog();
				}
            }
        }

		public void onDropDownLevelValueChanged(int value) {
            if (_trfmDropDownLevel && value < _iDropDownLevelOptions) {
                if (value == 0) {
                    showNewLevelDialog();
                }
                else if (value == 1) {
                    showLoadTestLevelDialog();
                }
            }
        }
    }
}