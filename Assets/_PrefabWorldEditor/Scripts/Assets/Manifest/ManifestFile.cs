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
	public class ManifestFile {

		[SerializeField]
		public int bundleId { get; set; }

		[SerializeField]
		public string fileName { get; set; }

        [SerializeField]
        public int updated { get; set; }

        [SerializeField]
		public List<AssetTypeObject> assetTypes { get; set; }

		//
		// Parse JSON data
		//
		public void parseJson(string json)
		{
			int i, len;
			AssetTypeObject assetTypeObject;

			//Debug.Log (json);
			JSONNode data = JSON.Parse(json);

            bundleId = -1;
			if (data ["id"] != null) {
                bundleId = Int32.Parse (data ["id"]);
			}

            fileName = "";
			if (data ["file"] != null) {
                fileName = data ["file"];
			}

            updated = DateTime.Now.Second;
            if (data["updated"] != null) {
                updated = Int32.Parse (data["updated"]);
            }

            assetTypes = new List<AssetTypeObject> ();
			if (data ["asset-types"] != null) {
				JSONArray elements = (JSONArray) data ["asset-types"];
				if (elements != null) {
					len = elements.Count;
					for (i = 0; i < len; ++i) {
                        assetTypeObject = new AssetTypeObject ();
                        assetTypeObject.parseJson (elements [i]);
                        assetTypes.Add (assetTypeObject);
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

			s += "\"id\":" + bundleId.ToString();
			s += ",\"file\":" + "\"" + fileName + "\"";
            s += ",\"updated\":" + updated.ToString ();

            s += ",\"asset-types\":[";
			len = assetTypes.Count;
			for (i = 0; i < len; ++i) {
				s += (i > 0 ? "," : "");
				s += assetTypes[i].getJsonString ();
			}
			s += "]";

			s += "}";

			return s;
		}
	}
}