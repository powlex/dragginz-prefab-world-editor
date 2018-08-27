//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;

using UnityEngine;

using SimpleJSON;

namespace PrefabWorldEditor
{
	[Serializable]
	public class AssetObject
	{
		[SerializeField]
		public int id { get; set; }

        [SerializeField]
        public string asset { get; set; }

        [SerializeField]
        public string desc { get; set; }

        //
        // Parse JSON data
        //
        public void parseJson(JSONNode data)
		{
			id = 0;
			if (data ["id"] != null) {
				id = Int32.Parse (data ["id"]);
			}

            asset = "";
            if (data["a"] != null) {
                asset = data["a"];
            }

            desc = "";
            if (data["d"] != null) {
                desc = data["d"];
            }
        }

        //
        // Create JSON string
        //
        public string getJsonString()
		{
			string s = "{";

			s += "\"id\":" + id.ToString();
            s += ",\"a\":\"" + asset + "\"";
            s += ",\"d\":\"" + desc + "\"";
            s += "}";

			return s;
		}
	}
}