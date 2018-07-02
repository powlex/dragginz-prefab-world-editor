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
			EditorModesSubMenu,
			AssetTypesSubMenu
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

		private MenuSettings _menuMain;
		private MenuSettings _menuBuild;
		private MenuSettings _subMenuEditorModes;
		private MenuSettings _subMenuBuildAssets;

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

		#region SystemMethods

		// ------------------------------------------------------------------------
		void Awake()
		{
			_curMenuOptionLeft  = MenuOption.None;
			_curMenuOptionRight = MenuOption.None;

			_iSelectedOptionLeft  = -1;
			_iSelectedOptionRight = -1;

			Color reddish = new Color(1F, 0.8F, 0.8F, 1F);

			_menuMain = new MenuSettings ();
			_menuMain.header  = "Main Menu";
			_menuMain.options = new string[]{"Editor Modes"};
			_menuMain.colors  = new Color[]{Color.white};

			_menuBuild = new MenuSettings ();
			_menuBuild.header  = "Build Menu";
			_menuBuild.options = new string[]{"Main Menu"};
			_menuBuild.colors  = new Color[]{reddish};

			_subMenuEditorModes = new MenuSettings ();
			_subMenuEditorModes.header  = "Editor Modes";
			_subMenuEditorModes.options = new string[]{"Play", "Build", "Select", "Clear"};
			_subMenuEditorModes.colors  = new Color[]{Color.white, Color.white, Color.white, Color.white};

			_subMenuBuildAssets = new MenuSettings ();
			_subMenuBuildAssets.header  = "Select Asset Type";
			_subMenuBuildAssets.options = new string[]{"Floor", "Wall", "Chunk", "Prop", "Dungeon"};
			_subMenuBuildAssets.colors  = new Color[]{Color.white, Color.white, Color.white, Color.white, Color.white};

            _curMenuPos = Vector3.zero;

            panelLeft.init();
			panelLeft.clickHandler += onLeftPanelButtonClick;

			panelRight.init();
			panelRight.clickHandler += onRightPanelButtonClick;
		}
				
		#endregion

		//

		#region PublicMethods

		// ------------------------------------------------------------------------
		public void init()
		{
			showMenuPanels (false);
		}

		// ------------------------------------------------------------------------
		public void showMenuPanels(bool state)
		{
			if (state) {
				setPanels (MenuOption.MainMenu, MenuOption.None);
			} else {
				setPanels (MenuOption.None, MenuOption.None);
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
				}
			} else {
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
					populatePanel (panelRight, _subMenuBuildAssets, _iSelectedOptionRight);
				}
			} else {
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
		}

		// ------------------------------------------------------------------------
		public void onRightPanelButtonClick(int index)
		{
			_iSelectedOptionRight = index;

			if (_curMenuOptionRight == MenuOption.EditorModesSubMenu)
			{
				if (index == 1) {
					_iSelectedOptionLeft = -1;
					_iSelectedOptionRight = -1;
					setPanels (MenuOption.BuildMenu, MenuOption.AssetTypesSubMenu);
				}
			}
			else if (_curMenuOptionRight == MenuOption.AssetTypesSubMenu)
			{
				VREditor.Instance.setAssetType (index);
			}

			//LevelController.Instance.placeDungeonPrefab (index);
		}

		#endregion
    }
}