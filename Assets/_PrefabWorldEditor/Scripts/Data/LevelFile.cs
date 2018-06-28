//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using AssetsShared;

using SimpleJSON;

namespace PrefabWorldEditor
{
	[Serializable]
	public class LevelFile {

		[SerializeField]
		public int fileFormatVersion { get; set; }

		[SerializeField]
		public int levelId { get; set; }

		[SerializeField]
		public DataTypeVector3 levelPos { get; set; }

		[SerializeField]
		public string levelName { get; set; }

		[SerializeField]
		public DataTypeVector3 levelSize { get; set; }

		[SerializeField]
		public List<LevelObject> levelObjects { get; set; }

		[SerializeField]
		public DataTypeVector3 playerPosition  { get; set; }

		[SerializeField]
		public DataTypeVector3 playerEuler  { get; set; }

		//
		// Parse JSON data
		//
		public void parseJson(string json)
		{
			int i, len;
			LevelObject levelObject;

			//Debug.Log (json);
			JSONNode data = JSON.Parse(json);

			fileFormatVersion = -1;
			if (data ["v"] != null) {
				fileFormatVersion = Int32.Parse (data ["v"]);
			}

			levelId = -1;
			if (data ["id"] != null) {
				levelId = Int32.Parse (data ["id"]);
			}

			levelPos = new DataTypeVector3 ();
			if (data ["pos"] != null) {
				if (data ["pos"] ["x"] != null) {
					levelPos.x = (float)data ["pos"] ["x"];
				}
				if (data ["pos"] ["y"] != null) {
					levelPos.y = (float)data ["pos"] ["y"];
				}
				if (data ["pos"] ["z"] != null) {
					levelPos.z = (float)data ["pos"] ["z"];
				}
			}

			levelName = "";
			if (data ["n"] != null) {
				levelName = data ["n"];
			}

			levelSize = new DataTypeVector3 ();
			if (data ["size"] != null) {
				if (data ["size"] ["x"] != null) {
					levelSize.x = (float)data ["size"] ["x"];
				}
				if (data ["size"] ["y"] != null) {
					levelSize.y = (float)data ["size"] ["y"];
				}
				if (data ["size"] ["z"] != null) {
					levelSize.z = (float)data ["size"] ["z"];
				}
			}

			levelObjects = new List<LevelObject> ();
			if (data ["objs"] != null) {
				JSONArray elements = (JSONArray) data ["objs"];
				if (elements != null) {
					len = elements.Count;
					for (i = 0; i < len; ++i) {
						levelObject = new LevelObject ();
						levelObject.parseJson (elements [i]);
						levelObjects.Add (levelObject);
					}
				}
			}

			playerPosition = new DataTypeVector3 ();
			playerPosition.x = 0;
			playerPosition.y = 0;
			playerPosition.z = 0;
			if (data ["p"] != null) {
				if (data ["p"] ["x"] != null) {
					playerPosition.x = (float)data ["p"] ["x"];
				}
				if (data ["p"] ["y"] != null) {
					playerPosition.y = (float)data ["p"] ["y"];
				}
				if (data ["p"] ["z"] != null) {
					playerPosition.z = (float)data ["p"] ["z"];
				}
			}

			playerEuler = new DataTypeVector3 ();
			playerEuler.x = 0;
			playerEuler.y = 0;
			playerEuler.z = 0;
			if (data ["r"] != null) {
				if (data ["r"] ["x"] != null) {
					playerEuler.x = (float)data ["r"] ["x"];
				}
				if (data ["r"] ["y"] != null) {
					playerEuler.y = (float)data ["r"] ["y"];
				}
				if (data ["r"] ["z"] != null) {
					playerEuler.z = (float)data ["r"] ["z"];
				}
			}
		}

		//
		// Create JSON string
		//
		public string getJsonString()
		{
			int i, len;

			string s = "{";

			s += "\"v\":" + fileFormatVersion.ToString();
			s += ",\"id\":" + levelId.ToString();
			s += ",\"pos\":" + levelPos.getJsonString();
			s += ",\"n\":" + "\"" + levelName + "\"";
			s += ",\"size\":" + levelSize.getJsonString();

			s += ",\"objs\":[";
			len = levelObjects.Count;
			for (i = 0; i < len; ++i) {
				s += (i > 0 ? "," : "");
				s += levelObjects [i].getJsonString ();
			}
			s += "]";

			s += ",\"p\":" + playerPosition.getJsonString();
			s += ",\"r\":" + playerEuler.getJsonString();

			s += "}";

			return s;
		}
	}
}