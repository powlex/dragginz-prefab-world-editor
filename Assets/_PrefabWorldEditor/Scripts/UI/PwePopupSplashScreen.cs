﻿//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;

using UnityEngine;
using UnityEngine.UI;

namespace PrefabWorldEditor
{
    public class PwePopupSplashScreen : MonoBehaviour
    {
        public GameObject blocker;
        public Text txtHeader;
        public Text txtMessage;
		public Text txtOverlayMessage;

		public Text txtInputCaption;
		public Text txtInputPlaceHolder;
		public InputField inputField;

        public GameObject btnOkay;
        public GameObject btnYes;
        public GameObject btnNo;

        private Action<int> _callback;

		#region Getters

		public string inputText {
			get { return inputField.text; }
		}

		#endregion

        //
        // System methods
        //
        void Awake() {

        }

        //
        // Public methods
        //
		public void showPopup(Globals.PopupMode mode, string header, string message, Action<int> callback = null)
        {
			txtHeader.text         = "";
			txtMessage.text        ="";
			txtOverlayMessage.text = "";

			if (header != null) {
                txtHeader.text = header;
            }

			if (message != null) {
				if (mode == Globals.PopupMode.Overlay) {
					setOverlayText (message);
				} else if (mode != Globals.PopupMode.NewLevel) {
					txtMessage.text = message;
				}
            }

			txtInputCaption.text = "";
			inputField.gameObject.SetActive(false);

            showButton(btnOkay, false);
            showButton(btnYes, false);
            showButton(btnNo, false);

			if (mode == Globals.PopupMode.Confirmation) {
                showButton(btnYes, true);
                showButton(btnNo, true);
            }
			else if (mode == Globals.PopupMode.Input) {
				showButton(btnYes, true);
				showButton(btnNo, true);
			}
			else if (mode == Globals.PopupMode.Notification) {
                showButton(btnOkay, true);
            }
			else if (mode == Globals.PopupMode.Overlay) {
				//
			}

            _callback = callback;
            if (blocker) {
                blocker.SetActive(true);
            }
            gameObject.SetActive(true);
        }

		//
		public void showPopup(Globals.PopupMode mode, string header, string caption, string placeholder, Action<int> callback = null) {

			showPopup (mode, header, "", callback);

			inputField.gameObject.SetActive(true);

			txtInputCaption.text = caption;
			if (LevelData.Instance.lastLevelName == Globals.defaultLevelName) {
				txtInputPlaceHolder.text = placeholder;
				inputField.text = "";
			} else {
				txtInputPlaceHolder.text = "";
				inputField.text = LevelData.Instance.lastLevelName;
			}
		}

		//
        public void hide() {

            _callback = null;
            if (blocker) {
                blocker.SetActive(false);
            }
            gameObject.SetActive(false);
        }

		//
		public bool isVisible()
		{
			return (gameObject.activeSelf);
		}

		//
		public void setOverlayText (string message)
		{
			txtOverlayMessage.text = message;
		}

        //
        // Private methods
        //
        private void showButton(GameObject btn, bool active) {
            if (btn) {
                btn.SetActive(active);
            }
        }

        //
        // button click handlers
        //
        public void onBtnOkayClick() {

            Debug.Log ("_callback: "+ _callback);

            if (_callback != null) {
				_callback.Invoke (0);
			} else {
				hide ();
			}
        }

        public void onBtnYesClick() {

            if (_callback != null) {
                _callback.Invoke(1);
			} else {
				hide ();
            }
        }

        public void onBtnNoClick() {

            if (_callback != null) {
                _callback.Invoke(2);
			} else {
				hide ();
            }
        }
    }
}