//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace PrefabWorldEditor
{
	[Serializable]
	public class AssetTypeObject
    {
		[SerializeField]
		public int id { get; set; }

        [SerializeField]
        public List<AssetObject> assets { get; set; }

        //
        // Parse JSON data
        //
        public void parseJson(JSONNode data)
		{
            int i, len;
            AssetObject assetObject;

            id = 0;
			if (data ["id"] != null) {
				id = Int32.Parse (data ["id"]);
			}

            assets = new List<AssetObject> ();
            if (data["assets"] != null) {
                JSONArray elements = (JSONArray) data ["assets"];
                if (elements != null) {
                    len = elements.Count;
                    for (i = 0; i < len; ++i) {
                        assetObject = new AssetObject ();
                        assetObject.parseJson (elements[i]);
                        assets.Add (assetObject);
                    }
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

			s += "\"id\":" + id.ToString();

            s += ",\"assets\":[";
            len = assets.Count;
            for (i = 0; i < len; ++i) {
                s += (i > 0 ? "," : "");
                s += assets[i].getJsonString ();
            }
            s += "]";

            s += "}";

			return s;
		}
	}
}