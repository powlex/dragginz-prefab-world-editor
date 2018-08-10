//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System.Collections.Generic;

using UnityEngine;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class WorldMapChunkController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _chunk;

        [SerializeField]
        private List<TextMesh> _txtMeshes;

        private Dictionary<GameObject, LevelChunk> _levelChunks;

        // ------------------------------------------------------------------------
        public void init(Vector3 pos, string chunkName)
		{
            string s = pos.x.ToString() + ", " + pos.y.ToString() + ", " + pos.z.ToString() + "\n" + chunkName;

            int i, len = _txtMeshes.Count;
            for (i = 0; i < len; ++i) {

                _txtMeshes[i].text = s;
            }
        }
	}
}