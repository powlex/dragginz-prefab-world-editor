//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using AssetsShared;

using SimpleJSON;

namespace PrefabWorldEditor
{
	public struct LevelStruct {
		public int id;
		public string name; 
		public string filename; 
		public int x;
		public int y;
		public int z;
		public string jsonData;
		public LevelStruct(int id, string name, string filename, int x, int y, int z, string jd = null) {
			this.id = id;
			this.name = name;
			this.filename = filename;
			this.x = x;
			this.y = y;
			this.z = z;
			this.jsonData = jd;
		}
	};

	//
	public class LevelManager : Singleton<LevelManager>
	{
		private LevelStruct[] _levelByIndex;

		private int _numLevels;

		#region Getters

		public int numLevels {
			get { return _numLevels; }
		}

		public LevelStruct[] levelByIndex {
			get { return _levelByIndex; }
		}

		#endregion

		#region PublicMethods

		public void init(string json)
		{
			_numLevels = 0;

			JSONNode data = JSON.Parse (json);
			if (data == null || data ["levels"] == null) {
				return;
			}

			JSONArray levels = (JSONArray) data ["levels"];
			_numLevels = levels.Count;
			_levelByIndex = new LevelStruct[_numLevels];

			int i;
			for (i = 0; i < _numLevels; ++i) {
				JSONNode level = levels [i];
				LevelStruct ls = new LevelStruct (int.Parse(level["id"]), level["name"], level["filename"], int.Parse(level["x"]), int.Parse(level["y"]), int.Parse(level["z"]));
				_levelByIndex [i] = ls;
			}
		}

		//
		public int getLevelIdByIndex(int index)
		{
			if (index < 0 || index >= _levelByIndex.Length) {
				return -1;
			}

			return _levelByIndex [index].id;
		}

		//
		/*public void loadLevelByIndex(int index)
		{
			LevelStruct ls = _levelByIndex [index];
		}*/

		//
		public void setLevelJson(int id, string json) 
		{
			int i;
			for (i = 0; i < _numLevels; ++i) {
				if (_levelByIndex [i].id == id) {
					LevelStruct level = _levelByIndex [i];
					level.jsonData = json;
					_levelByIndex [i] = level;
					break;
				}
			}
		}

		//
		public string getLevelJson(int id) 
		{
			string json = null;

			int i;
			for (i = 0; i < _numLevels; ++i) {
				if (_levelByIndex [i].id == id) {
					json = _levelByIndex [i].jsonData;
					break;
				}
			}

			return json;
		}

        //
        public LevelStruct getLevelStruct (int id)
        {
            LevelStruct ls = new LevelStruct();

            int i;
            for (i = 0; i < _numLevels; ++i) {
                if (_levelByIndex[i].id == id) {
                    ls = _levelByIndex[i];
                    break;
                }
            }

            return ls;
        }

        //
        public LevelChunk createOfflineLevelChunk()
		{
			LevelChunk chunk;

            GameObject gameObject = new GameObject();// AssetFactory.Instance.createLevelContainerClone ();
			//gameObject.name = "LevelChunk_Offline";

			chunk = gameObject.AddComponent<LevelChunk> ();
			//chunk.init (Vector3.zero);

			return chunk;
		}

		//
		public Dictionary<int, LevelChunk> createLevelChunks()
		{
			Dictionary<int, LevelChunk> chunks = new Dictionary<int, LevelChunk> ();

			/*int i;
			for (i = 0; i < _numLevels; ++i) {

				LevelStruct level = _levelByIndex [i];

				GameObject gameObject = AssetFactory.Instance.createLevelContainerClone ();
				gameObject.name = "LevelChunk_" + level.id.ToString ();

				Vector3 chunkPos = new Vector3 (level.x * Globals.LEVEL_WIDTH, -level.y * Globals.LEVEL_HEIGHT, level.z * Globals.LEVEL_DEPTH);
				gameObject.transform.position = chunkPos;

				LevelChunk chunk = gameObject.AddComponent<LevelChunk> ();
				chunk.init (chunkPos);
				chunk.setLevelData (level);

				chunks.Add (level.id, chunk);
			}*/

			return chunks;
		}

        #endregion
    }
}