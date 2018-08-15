//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;
using UnityEngine.SceneManagement;

using System;

namespace PrefabWorldEditor
{
    public class AppEntry : MonoBehaviour
    {
        // ------------------------------------------------------------------------
        void Awake () {

            if (Globals.debug)
                Debug.Log ("AppEntry Awake");

            bool appIsInstantiated = true;

            // Instantiate app controller singleton
            if (GameObject.Find (Globals.appContainerName) == null) {
                GameObject goAppController = new GameObject (Globals.appContainerName);
                DontDestroyOnLoad (goAppController);
                goAppController.AddComponent<AppController> ();

                appIsInstantiated = false;
            }

            if (GameObject.Find (Globals.netContainerName) == null) {
                GameObject goNetManager = new GameObject (Globals.netContainerName);
                DontDestroyOnLoad (goNetManager);
                goNetManager.AddComponent<NetManager> ();
            }

            if (!appIsInstantiated)
            {
                if (SceneManager.GetActiveScene ().name != BuildSettings.SplashScreenScene) {
                    SceneManager.sceneLoaded += OnSplashSceneLoaded;
                    SceneManager.LoadScene (BuildSettings.SplashScreenScene, LoadSceneMode.Single);
                }
            }
        }

        // ------------------------------------------------------------------------
        void OnSplashSceneLoaded (Scene scene, LoadSceneMode mode)
        {
            if (Globals.debug)
                Debug.Log ("AppEntry OnSplashSceneLoaded " + scene.name);

            SceneManager.sceneLoaded -= OnSplashSceneLoaded;
        }
    }
}