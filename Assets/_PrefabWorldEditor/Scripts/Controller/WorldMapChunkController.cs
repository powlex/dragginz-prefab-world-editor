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
        public void init(Vector3 pos, Vector3 size, Vector3 v3Scale, string chunkName)
		{
            string s = pos.x.ToString() + ", " + pos.y.ToString() + ", " + pos.z.ToString();
            s += "\n" + size.x.ToString() + " * " + size.y.ToString() + " * " + size.z.ToString();
            s += "\n" + chunkName;

            _chunk.transform.localScale = v3Scale;

            float xOffset = v3Scale.x * .5f + 0.01f;
            float yOffset = v3Scale.y * .5f + 0.01f;
            float zOffset = v3Scale.z * .5f + 0.01f;

            int i, len = _txtMeshes.Count;
            for (i = 0; i < len; ++i) {
                _txtMeshes[i].text = s;
                if (i == 0) {
                    _txtMeshes[i].transform.localPosition = new Vector3 (0, 0, -zOffset);
                }
                else if (i == 2) {
                    _txtMeshes[i].transform.localPosition = new Vector3 (0, 0, zOffset);
                }
                else if (i == 1) {
                    _txtMeshes[i].transform.localPosition = new Vector3 (xOffset, 0, 0);
                }
                else if (i == 3) {
                    _txtMeshes[i].transform.localPosition = new Vector3 (-xOffset, 0, 0);
                }
                else if (i == 4) {
                    _txtMeshes[i].transform.localPosition = new Vector3 (0, yOffset, 0);
                }
                else if (i == 5) {
                    _txtMeshes[i].transform.localPosition = new Vector3 (0, -yOffset, 0);
                }
            }
        }
	}
}