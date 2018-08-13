//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

using AssetsShared;

namespace PrefabWorldEditor
{
    public class SplashScreenController : MonoSingleton<SplashScreenController>
    {
		[SerializeField]
		private Text FileInfo;
		[SerializeField]
		private Text Message;
		[SerializeField]
		private Text Update;

		[SerializeField]
        private Button ButtonOnline;
		[SerializeField]
		private Button ButtonOffline;

		[SerializeField]
		private Text TextButtonOnline;
		[SerializeField]
		private Text TextButtonOffline;
        
		[SerializeField]
        private GameObject Spinner;

        [SerializeField]
        private PwePopupSplashScreen popup;

        //

        private NetManager netManager;
        private LevelChunkManager levelChunkManager;
        
		private bool _onlineModeAvailable;
		private int _iCurLevelChunk;

        //
        void Awake () {
            Debug.Log ("SplashScreenController Awake");
            if (popup != null) {
                popup.hide ();
            }
            resetScreen ();
        }

        //
        public void init ()
		{
			if (XRSettings.enabled)
			{
				SceneManager.LoadScene (2);
			}
			else
            {
                netManager = NetManager.Instance;
                levelChunkManager = LevelChunkManager.Instance;

                _onlineModeAvailable = true;

				FileInfo.text = Globals.version;

				//resetScreen ();
				ButtonOnline.gameObject.SetActive (true);
				ButtonOnline.interactable = _onlineModeAvailable;
				ButtonOffline.gameObject.SetActive (true);

				TextButtonOnline.text = _onlineModeAvailable ? "Load multiple level chunks\nfrom dummy server" : "Currently unavailable!";

				#if UNITY_WEBGL
				TextButtonOffline.text = "Loading and saving levels\nnot available in Web version";
				#else
				TextButtonOffline.text = "Load and save levels\non your hard drive";
				#endif
		
				_iCurLevelChunk = 0;
			}
		}

		private void resetScreen()
		{
			ButtonOnline.gameObject.SetActive(false);
			ButtonOffline.gameObject.SetActive(false);

			Message.text = "";
			Update.text = "";

			Spinner.SetActive(false);
		}

		//
		// CONNECT
		//

        public void workOnline()
        {
            AppController.Instance.editorIsInOfflineMode = false;

            resetScreen ();

			Message.gameObject.SetActive (true);
			Message.text = "Connecting...";

            Spinner.SetActive(true);

            AttemptConnection();
        }

		//
		public void workOffline()
		{
            AppController.Instance.editorIsInOfflineMode = true;

			resetScreen ();

            //LevelEditor.Instance.initOfflineMode ();

            Message.text = "Loading Editor...";

            AppController.Instance.loadMainScene ();
		}

		//
		private void AttemptConnection()
        {
			NetManager.Instance.loadLevelList (ConnectionSuccess, ConnectionFail);

            StartCoroutine(TimerUtils.WaitAndPerform(5.0f, ConnectionTimeout));
        }

		//
		private void ConnectionSuccess(string data)
		{
            Debug.Log ("SplashScreenController ConnectionSuccess");
            StopCoroutine (TimerUtils.WaitAndPerform(5.0f, ConnectionTimeout));

            levelChunkManager.init (data);

			Message.text = "Loading Level...";
			Update.gameObject.SetActive (true);

			loadLevelChunks ();
		}

		//
		private void ConnectionFail(string error)
		{
            Debug.Log ("SplashScreenController ConnectionFail");
            StopCoroutine (TimerUtils.WaitAndPerform(5.0f, ConnectionTimeout));

			resetScreen ();

			popup.showPopup (Globals.PopupMode.Notification, "ERROR", error, timeOutPopupContinue);
		}

		private void ConnectionTimeout()
		{
			StopCoroutine(TimerUtils.WaitAndPerform(5.0f, ConnectionTimeout));

			resetScreen ();

            popup.showPopup (Globals.PopupMode.Notification, "Warning", "Could not connect to Server!\n\nEditor will run in Offline Mode!", timeOutPopupContinue);
		}

		//
		// LEVEL CHUNKS
		//

		private void loadLevelChunks()
		{
			// done loading?
			if (_iCurLevelChunk >= levelChunkManager.numLevels)
			{
                resetScreen ();

                //Message.text = "Creating Level...";
                //Update.text = "";
				//StartCoroutine("createLevels");
                Message.text = "Loading Editor...";
                AppController.Instance.loadMainScene ();
            }
			else 
			{
				Update.text = (_iCurLevelChunk + 1).ToString () + " of " + levelChunkManager.numLevels.ToString ();
				NetManager.Instance.loadLevelChunk (levelChunkManager.levelByIndex[_iCurLevelChunk].filename, LoadSuccess, loadFail);
				StartCoroutine(TimerUtils.WaitAndPerform(5.0f, LoadTimeout));
			}
		}

		//
		private void LoadSuccess(string data)
		{
			StopCoroutine(TimerUtils.WaitAndPerform(5.0f, LoadTimeout));

            levelChunkManager.setLevelJson (levelChunkManager.levelByIndex [_iCurLevelChunk].id, data);

			_iCurLevelChunk++;

			loadLevelChunks ();
		}

		//
		private void loadFail(string error)
		{
			StopCoroutine(TimerUtils.WaitAndPerform(5.0f, LoadTimeout));

			resetScreen ();

            popup.showPopup (Globals.PopupMode.Notification, "ERROR", error, timeOutPopupContinue);
		}

		//
		private void LoadTimeout()
		{
			StopCoroutine(TimerUtils.WaitAndPerform(5.0f, LoadTimeout));

			resetScreen ();

            popup.showPopup (Globals.PopupMode.Notification, "Warning", "Could not load all level chunks!\n\nEditor will run in Offline Mode!", timeOutPopupContinue);
		}

		//
		// LOAD LEVEL DATA
		//
		private IEnumerator createLevels()
		{
            int i, len = levelChunkManager.numLevels;
			for (i = 0; i < len; ++i) {

				Update.text = (i + 1).ToString () + " of " + len.ToString ();

				int levelId = levelChunkManager.getLevelIdByIndex (i);
				//LevelEditor.Instance.createLevelChunkWithIndex (levelId, i);

				yield return new WaitForEndOfFrame();
			}

			resetScreen ();

            Message.text = "Loading Editor...";
            AppController.Instance.loadMainScene ();
        }

		//
		// POPUP BUTTON CLICK
		//

		//
		private void timeOutPopupContinue(int buttonId)
		{
            popup.hide();

			workOffline ();
		}
    }
}