//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System.Collections.Generic;

using UnityEngine;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class WorldMapController : MonoSingleton<WorldMapController>
    {
		public struct LevelChunk
		{
			public GameObject go;
        };

        [SerializeField]
        private GameObject _container;

        [SerializeField]
        private GameObject _chunkPrefab;

        private Dictionary<GameObject, LevelChunk> _levelChunks;

        #region PublicMethods

        // ------------------------------------------------------------------------
        void Awake () {
            _levelChunks = new Dictionary<GameObject, LevelChunk> ();
        }

        // ------------------------------------------------------------------------
        public void init()
		{
            reset ();

            int i, len = LevelManager.Instance.numLevels;
            for (i = 0; i < len; ++i) {

                createChunk (LevelManager.Instance.levelByIndex[i]);
            }
        }

        // ------------------------------------------------------------------------
        public void reset () {

            clear ();
        }

        // ------------------------------------------------------------------------
        private void clear()
		{
			foreach (Transform child in _container.transform) {
				GameObject.Destroy (child.gameObject);
			}

            _levelChunks.Clear ();
        }

        // ------------------------------------------------------------------------
        private void createChunk (LevelStruct ls) {

            GameObject go = Instantiate(_chunkPrefab);
            go.name = "chunk:" + ls.x.ToString () + "." + ls.y.ToString () + "." + ls.z.ToString ();
            go.transform.SetParent (_container.transform);
            go.transform.position = new Vector3 (ls.x, ls.y, ls.z);

            WorldMapChunkController wmcc = go.GetComponent<WorldMapChunkController>();
            if (wmcc != null) {
                wmcc.init (new Vector3 (ls.x, ls.y, ls.z), ls.name);
            }

            LevelChunk lc = new LevelChunk();
            lc.go = go;

            _levelChunks.Add (go, lc);
        }

        #endregion
    }
}