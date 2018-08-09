//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

namespace PrefabWorldEditor
{
    public static class BuildSettings
    {
        public static readonly string SplashScreenScene  = "pwe_scene0_splash";
        public static readonly string UnityClientScene   = "pwe_scene1_main";
        public static readonly string UnityClientSceneVR = "pwe_scene2_vr";

        public static readonly string ClientDefaultActiveScene = UnityClientScene;
        public static readonly string[] ClientScenes = { UnityClientScene, UnityClientSceneVR, SplashScreenScene };

        public static readonly string UnityWorkerScene = "UnityWorker";
        public static readonly string WorkerDefaultActiveScene = UnityWorkerScene;
        public static readonly string[] WorkerScenes = { UnityWorkerScene };

        public const string SceneDirectory = "Assets";
    }
}
