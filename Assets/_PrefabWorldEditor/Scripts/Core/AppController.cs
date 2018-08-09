//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using AssetsShared;

namespace PrefabWorldEditor
{
	//
	public enum AppState {
		Null,
        InitSplash,
        Splash,
        InitEditor,
		Editor
	};

	//
	public enum PopupMode {
		Notification,
		Confirmation,
		Input
	};

	//
    public class AppController : MonoSingleton<AppController>
    {
		private bool _editorIsInOfflineMode;

        private AppState _appState;

		private float time;
		private float timeDelta;

		public AppState appState {
			get { return _appState; }
		}

		public bool editorIsInOfflineMode {
			get { return _editorIsInOfflineMode; }
			set { _editorIsInOfflineMode = value; }
		}

        // ------------------------------------------------------------------------
        void Awake () {

            Debug.Log ("AppController Awake");
			Application.targetFrameRate = Globals.TargetClientFramerate;

			_editorIsInOfflineMode = true;

			_appState = AppState.Null;
        }

        // ------------------------------------------------------------------------
        void Start () {

            Debug.Log ("AppController Start");
            _appState = AppState.InitSplash;
        }

        // ------------------------------------------------------------------------
        void Update () {

			time = Time.realtimeSinceStartup;
			timeDelta = Time.deltaTime;

            if (_appState == AppState.Editor) {
                PrefabLevelEditor.Instance.customUpdate (time, timeDelta);
            }
            else if (_appState == AppState.InitEditor) {
                _appState = AppState.Editor;
                PrefabLevelEditor.Instance.init ();
            }
            else if (_appState == AppState.InitSplash) {
                _appState = AppState.Splash;
                SplashScreenController.Instance.init ();
            }
        }

        // ------------------------------------------------------------------------
        public void loadMainScene()
        {
            if (SceneManager.GetActiveScene ().name != BuildSettings.UnityClientScene)
            {
                _appState = AppState.Null;
                SceneManager.sceneLoaded += OnMainSceneLoaded;
                SceneManager.LoadScene (BuildSettings.UnityClientScene, LoadSceneMode.Single);
            }
        }

        // ------------------------------------------------------------------------
        void OnMainSceneLoaded (Scene scene, LoadSceneMode mode)
        {
            Debug.Log ("AppController OnMainSceneLoaded " + scene.name);
            SceneManager.sceneLoaded -= OnMainSceneLoaded;

            _appState = AppState.InitEditor;
        }
    }
}