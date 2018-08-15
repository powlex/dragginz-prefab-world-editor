//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

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

        private GameObject _goHit;
        private RaycastHit _hit;
        private Ray _ray;

        #region SystemMethods

        // ------------------------------------------------------------------------
        void Awake () {
            _levelChunks = new Dictionary<GameObject, LevelChunk> ();
        }

        // ------------------------------------------------------------------------
        private void Update () {
        
            if (PrefabLevelEditor.Instance.leftMouseButtonPressed) {
                if (!EventSystem.current.IsPointerOverGameObject ()) {

                    _goHit = null;
                    _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                    if (Physics.Raycast (_ray, out _hit, 100)) {
                        _goHit = _hit.collider.gameObject;

                        loadClickedLevel ();
                    }
                }
            }
        }

        #endregion

        //

        #region PublicMethods

        // ------------------------------------------------------------------------
        public void init()
		{
            reset ();

            int i, len = LevelChunkManager.Instance.numLevels;
            for (i = 0; i < len; ++i) {
                createChunk (LevelChunkManager.Instance.levelByIndex[i]);
            }
        }

        // ------------------------------------------------------------------------
        public void reset () {

            clear ();
        }

        #endregion

        //

        #region PrivateMethods

        // ------------------------------------------------------------------------
        private void clear()
		{
			foreach (Transform child in _container.transform) {
				GameObject.Destroy (child.gameObject);
			}

            _levelChunks.Clear ();
        }

        // ------------------------------------------------------------------------
        private void loadClickedLevel() {

            if (_goHit != null && _goHit.transform.parent != null) {

                GameObject target = _goHit.transform.parent.gameObject;
                int index = 0;
                foreach (KeyValuePair<GameObject, LevelChunk> chunk in _levelChunks) {

                    if (chunk.Key == target) {
                        //Debug.Log (chunk.Key.name + " + " + target.name + " : " + index);
                        PweMainMenu.Instance.selectDropDownChunksValue (index);
                        break;
                    }
                    index++;
                }
            }
        }

        // ------------------------------------------------------------------------
        private void createChunk (LevelStruct ls) {

            GameObject go = Instantiate(_chunkPrefab);
            go.name = "chunk:" + ls.posX.ToString () + "." + ls.posY.ToString () + "." + ls.posZ.ToString ();
            go.transform.SetParent (_container.transform);

            Vector3 v3Pos   = new Vector3 ((float)ls.posX / 10f, (float)ls.posY / 10f, (float)ls.posZ / 10f);
            Vector3 v3Scale = new Vector3 ((float)ls.sizeX / 10f, (float)ls.sizeY / 10f, (float)ls.sizeZ / 10f);

            go.transform.position = new Vector3(v3Pos.x + v3Scale.x * .5f, v3Pos.y + v3Scale.y * .5f, v3Pos.z + v3Scale.z * .5f);

            WorldMapChunkController wmcc = go.GetComponent<WorldMapChunkController>();
            if (wmcc != null) {
                wmcc.init (new Vector3(ls.posX, ls.posY, ls.posZ), new Vector3 (ls.sizeX, ls.sizeY, ls.sizeZ), v3Scale, ls.name);
            }

            LevelChunk lc = new LevelChunk();
            lc.go = go;

            _levelChunks.Add (go, lc);
        }

        #endregion
    }
}