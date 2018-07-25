//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;

using UnityEngine;

using AssetsShared;

using SimpleJSON;

namespace PrefabWorldEditor
{
	[Serializable]
	public class LevelObject
	{
		[SerializeField]
		public int id { get; set; }

		[SerializeField]
		public DataTypeVector3 position  { get; set; }

		[SerializeField]
		public DataTypeQuaternion rotation  { get; set; }

        [SerializeField]
        public DataTypeVector3 scale { get; set; }

        [SerializeField]
        public int overwriteStatic { get; set; }

        [SerializeField]
        public int overwriteGravity { get; set; }

        [SerializeField]
        public float shaderSnow { get; set; }

        [SerializeField]
        public string customData { get; set; }

        //
        // Parse JSON data
        //
        public void parseJson(JSONNode data)
		{
			id = 0;
			if (data ["id"] != null) {
				id = Int32.Parse (data ["id"]);
			}

			position = new DataTypeVector3 ();
			position.x = 0;
			position.y = 0;
			position.z = 0;
			if (data ["p"] != null) {
				if (data ["p"] ["x"] != null) {
					position.x = (float)data ["p"] ["x"];
				}
				if (data ["p"] ["y"] != null) {
					position.y = (float)data ["p"] ["y"];
				}
				if (data ["p"] ["z"] != null) {
					position.z = (float)data ["p"] ["z"];
				}
			}

			rotation = new DataTypeQuaternion ();
			rotation.w = 0;
			rotation.x = 0;
			rotation.y = 0;
			rotation.z = 0;
			if (data ["r"] != null) {
				if (data ["r"] ["w"] != null) {
					rotation.w = (float)data ["r"] ["w"];
				}
				if (data ["r"] ["x"] != null) {
					rotation.x = (float)data ["r"] ["x"];
				}
				if (data ["r"] ["y"] != null) {
					rotation.y = (float)data ["r"] ["y"];
				}
				if (data ["r"] ["z"] != null) {
					rotation.z = (float)data ["r"] ["z"];
				}
			}

            scale = new DataTypeVector3();
            scale.x = 1;
            scale.y = 1;
            scale.z = 1;
            if (data["s"] != null) {
                if (data["s"]["x"] != null) {
                    scale.x = (float)data["s"]["x"];
                }
                if (data["s"]["y"] != null) {
                    scale.y = (float)data["s"]["y"];
                }
                if (data["s"]["z"] != null) {
                    scale.z = (float)data["s"]["z"];
                }
            }

            overwriteStatic = 0;
            if (data["os"] != null) {
                overwriteStatic = Int32.Parse (data["os"]);
            }

            overwriteGravity = 0;
            if (data["og"] != null) {
                overwriteGravity = Int32.Parse(data["og"]);
            }

            shaderSnow = 0;
            if (data["shs"] != null) {
                shaderSnow = (float)data["shs"];
            }

            customData = "";
            if (data["c"] != null) {
                customData = data["c"];
            }
        }

        //
        // Create JSON string
        //
        public string getJsonString()
		{
			string s = "{";

			s += "\"id\":" + id.ToString();
			s += ",\"p\":" + position.getJsonString();
			s += ",\"r\":" + rotation.getJsonString();
            s += ",\"s\":" + scale.getJsonString();
            if (overwriteStatic != 0) {
                s += ",\"os\":" + overwriteStatic.ToString ();
            }
            if (overwriteGravity != 0) {
                s += ",\"og\":" + overwriteGravity.ToString();
            }
            if (shaderSnow != 0) {
                s += ",\"shs\":" + shaderSnow.ToString();
            }
            if (customData != "") {
                s += ",\"c\":\"" + customData + "\"";
            }

            s += "}";

			return s;
		}
	}
}