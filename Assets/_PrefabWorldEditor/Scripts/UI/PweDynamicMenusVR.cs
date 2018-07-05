//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class PweDynamicMenusVR : MonoSingleton<PweDynamicMenusVR>
    {
		private enum MenuOption
		{
			None,
			MainMenu,
			BuildMenu,
            AssetTypeMenu,
			EditorModesSubMenu,
			AssetTypesSubMenu,
            AssetsSubMenu
		};

		private struct MenuSettings
		{
			public string header;
			public string[] options;
			public Color[] colors;
		};

		//

		public Transform trfmVRCamera;

		public UIMenuPanel panelLeft;
		public UIMenuPanel panelRight;

		//

		private MenuOption _curMenuOptionLeft;
		private MenuOption _curMenuOptionRight;

		private int _iSelectedOptionLeft;
		private int _iSelectedOptionRight;

        private int _iSelectedAssetTypeIndex;

        private MenuSettings _menuMain;
		private MenuSettings _menuBuild;
        private MenuSettings _menuAssetType;
        private MenuSettings _subMenuEditorModes;
		private MenuSettings _subMenuBuildAssetTypes;
        private MenuSettings[] _aSubMenuBuildAssets;

        private Vector3 _curMenuPos;

        //

        #region Getters

        public Vector3 curMenuPos {
            get { return _curMenuPos; }
        }

        public bool panelAssetTypesVisible {
			get { return panelLeft.gameObject.activeSelf; }
		}

		public bool panelAssetsVisible {
			get { return panelRight.gameObject.activeSelf; }
		}

		#endregion

        //

		#region PublicMethods

		// ------------------------------------------------------------------------
		public void init()
		{
            _curMenuOptionLeft = MenuOption.None;
            _curMenuOptionRight = MenuOption.None;

            _iSelectedOptionLeft = -1;
            _iSelectedOptionRight = -1;

            _iSelectedAssetTypeIndex = 0;

            Color reddish = new Color(1F, 0.8F, 0.8F, 1F);

            _menuMain = new MenuSettings();
            _menuMain.header = "Main Menu";
            _menuMain.options = new string[] { "Editor Modes", "Clear Level", "Load Test Level" };
            _menuMain.colors = new Color[] { Color.white, Color.white, Color.white };

            _menuBuild = new MenuSettings();
            _menuBuild.header = "Build Menu";
            _menuBuild.options = new string[] { "Main Menu" };
            _menuBuild.colors = new Color[] { reddish };

            _menuAssetType = new MenuSettings();
            _menuAssetType.header = "";
            _menuAssetType.options = new string[] { "Build Menu" };
            _menuAssetType.colors = new Color[] { reddish };

            _subMenuEditorModes = new MenuSettings();
            _subMenuEditorModes.header = "Editor Modes";
            _subMenuEditorModes.options = new string[] { "Play", "Build", "Select", "Clear" };
            _subMenuEditorModes.colors = new Color[] { Color.white, Color.white, Color.white, Color.white };

            _subMenuBuildAssetTypes = new MenuSettings();
            _subMenuBuildAssetTypes.header = "Select Asset Type";

            // get asset types
            string[] aAssetTypeNames = System.Enum.GetNames(typeof(Globals.AssetType));
            int i, len = aAssetTypeNames.Length;

            _subMenuBuildAssetTypes.options = new string[len];
            _subMenuBuildAssetTypes.colors = new Color[len];
            _aSubMenuBuildAssets = new MenuSettings[len];

            int j, len2;
            for (i = 0; i < len; ++i) {
                _subMenuBuildAssetTypes.options[i] = aAssetTypeNames[i];
                _subMenuBuildAssetTypes.colors[i] = Color.white;

                List<PrefabLevelEditor.Part> assets = PrefabLevelEditor.Instance.assetTypeList[(Globals.AssetType)i];
                len2 = assets.Count;
                _aSubMenuBuildAssets[i].options = new string[len2];
                _aSubMenuBuildAssets[i].colors = new Color[len2];
                for (j = 0; j < len2; ++j) {
                    _aSubMenuBuildAssets[i].options[j] = assets[j].name;
                    _aSubMenuBuildAssets[i].colors[j] = Color.white;
                }
            }

            _curMenuPos = Vector3.zero;

            panelLeft.init();
            panelLeft.clickHandler += onLeftPanelButtonClick;

            panelRight.init();
            panelRight.clickHandler += onRightPanelButtonClick;

            showMenuPanels(false, false);
        }

		// ------------------------------------------------------------------------
		public void showMenuPanels(bool state, bool setEditPart = true)
		{
			if (state) {
				setPanels (MenuOption.MainMenu, MenuOption.None);
			} else {
				setPanels (MenuOption.None, MenuOption.None);
                if (setEditPart) {
                    VREditor.Instance.hideEditPart();
                }
			}
		}

		#endregion

		//

		#region PrivateMethods

		// ------------------------------------------------------------------------
		private void setPanels(MenuOption left, MenuOption right)
		{
            // adjust camera
            _curMenuPos = trfmVRCamera.position + (4f * trfmVRCamera.forward);
            _curMenuPos.y = 2.25f;
			transform.position = _curMenuPos;
			transform.rotation = Quaternion.LookRotation(transform.position - trfmVRCamera.position);
							
			_curMenuOptionLeft = left;
			panelLeft.gameObject.SetActive (left != MenuOption.None);

			if (left != MenuOption.None) {
				if (left == MenuOption.MainMenu) {
					populatePanel (panelLeft, _menuMain, _iSelectedOptionLeft);
				} else if (left == MenuOption.BuildMenu) {
					populatePanel (panelLeft, _menuBuild, _iSelectedOptionLeft);
                } else if (left == MenuOption.AssetTypeMenu) {
                    populatePanel(panelLeft, _menuAssetType, _iSelectedOptionLeft);
                }
            }
            else {
				_iSelectedOptionLeft = -1;
			}

			_curMenuOptionRight = right;
			panelRight.gameObject.SetActive (right != MenuOption.None);

			if (right != MenuOption.None)
			{
				if (right == MenuOption.EditorModesSubMenu) {
					populatePanel (panelRight, _subMenuEditorModes, _iSelectedOptionRight);
				}
				else if (right == MenuOption.AssetTypesSubMenu) {
					populatePanel (panelRight, _subMenuBuildAssetTypes, _iSelectedOptionRight);
				}
                else if (right == MenuOption.AssetsSubMenu) {
                    populatePanel(panelRight, _aSubMenuBuildAssets[_iSelectedAssetTypeIndex], _iSelectedOptionRight);
                }
            }
            else {
				_iSelectedOptionRight = -1;
			}
		}

		private void populatePanel(UIMenuPanel panel, MenuSettings menu, int selectedOption)
		{
			panel.populate (menu.header, menu.options, menu.colors, selectedOption);
		}

		#endregion

		//

		#region EventHandlers

		// ------------------------------------------------------------------------
		private void onLeftPanelButtonClick(int index)
		{
			_iSelectedOptionLeft = index;

			if (_curMenuOptionLeft == MenuOption.MainMenu)
			{
				if (index == 0) {
					_iSelectedOptionRight = -1;
					setPanels (MenuOption.MainMenu, MenuOption.EditorModesSubMenu);
                    PrefabLevelEditor.Instance.trfmWalls.gameObject.SetActive(true);
                }
                else if (index == 1) {
                    PrefabLevelEditor.Instance.clearLevelConfirm(1);
                    PrefabLevelEditor.Instance.trfmWalls.gameObject.SetActive(true);
                }
                else if (index == 2) {
                    PweMainMenu.Instance.loadTestLevel(1);
                    showMenuPanels(false);
                    PrefabLevelEditor.Instance.trfmWalls.gameObject.SetActive(false);
                }
            }
            else if (_curMenuOptionLeft == MenuOption.BuildMenu)
			{
				if (index == 0) {
					_iSelectedOptionLeft  = -1;
					_iSelectedOptionRight = -1;
					setPanels (MenuOption.MainMenu, MenuOption.None);
				}
			}
            else if (_curMenuOptionLeft == MenuOption.AssetTypeMenu)
            {
                _iSelectedOptionLeft = -1;
                _iSelectedOptionRight = -1;
                VREditor.Instance.hideEditPart();
                setPanels(MenuOption.BuildMenu, MenuOption.AssetTypesSubMenu);
            }
        }

        // ------------------------------------------------------------------------
        public void onRightPanelButtonClick(int index)
		{
			_iSelectedOptionRight = index;

            if (_curMenuOptionRight == MenuOption.EditorModesSubMenu) {

                if (index == 1) {
                    _iSelectedOptionLeft = -1;
                    _iSelectedOptionRight = -1;
                    setPanels(MenuOption.BuildMenu, MenuOption.AssetTypesSubMenu);
                }
            }
            else if (_curMenuOptionRight == MenuOption.AssetTypesSubMenu) {

                _iSelectedAssetTypeIndex = index;
                _menuAssetType.header = _subMenuBuildAssetTypes.options[_iSelectedAssetTypeIndex];
                VREditor.Instance.setAssetType(_iSelectedAssetTypeIndex, 0);

                _iSelectedOptionLeft = -1;
                _iSelectedOptionRight = -1;
                setPanels(MenuOption.AssetTypeMenu, MenuOption.AssetsSubMenu);
            }
            else if (_curMenuOptionRight == MenuOption.AssetsSubMenu) {

                VREditor.Instance.setAsset(_iSelectedAssetTypeIndex, index);
            }
        }

        #endregion
    }
}