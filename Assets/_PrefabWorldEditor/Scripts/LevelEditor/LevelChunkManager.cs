//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

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
		public int posX;
		public int posY;
		public int posZ;
        public int sizeX;
        public int sizeY;
        public int sizeZ;
        public int updated;
        public string jsonData;
		public LevelStruct(int id, string name, string filename, int posX, int posY, int posZ, int sizeX, int sizeY, int sizeZ, int updated, string jd = null) {
			this.id = id;
			this.name = name;
			this.filename = filename;
			this.posX = posX;
			this.posY = posY;
			this.posZ = posZ;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;
            this.updated = updated;
            this.jsonData = jd;
		}
	};

	//
	public class LevelChunkManager : Singleton<LevelChunkManager>
	{
		private int _numLevels = 0;
        private List<LevelChunk> _aChunks = new List<LevelChunk>();

        private LevelStruct[] _levelByIndex;

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
				LevelStruct ls = new LevelStruct ( int.Parse(level["id"]), level["name"], level["filename"],
                                                   int.Parse(level["pos-x"]), int.Parse(level["pos-y"]), int.Parse(level["pos-z"]),
                                                   int.Parse(level["size-x"]), int.Parse(level["size-y"]), int.Parse(level["size-z"]),
                                                   int.Parse(level["updated"]) );
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
        public void destroyAllChunks ()
        {
            int i, len = _aChunks.Count;
            for (i = 0; i < len; ++i) {

                foreach (Transform child in _aChunks[i].transform) {
                    GameObject.Destroy (child.gameObject);
                }
                GameObject.Destroy (_aChunks[i].gameObject);
            }

            _aChunks.Clear ();
        }

        //
        public void createAllChunks()
        {
            int i;
            for (i = 0; i < _numLevels; ++i) {
                if (_levelByIndex[i].id != LevelData.Instance.currentLevelId) {
                    
                    LevelChunk lc = createLevelChunks(i);
                    _aChunks.Add (lc);
                }
            }
        }

        //
        public LevelChunk createLevelChunks(int i)
		{
			LevelStruct level = _levelByIndex [i];

			GameObject gameObject = new GameObject("[LEVEL-CONTAINER-"+level.id.ToString()+"]");

			gameObject.transform.position = new Vector3(level.posX, level.posY, level.posZ);

			LevelChunk chunk = gameObject.AddComponent<LevelChunk> ();
			chunk.init (level.jsonData);

			return chunk;
		}

        #endregion
    }
}