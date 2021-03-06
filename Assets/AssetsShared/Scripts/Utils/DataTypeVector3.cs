﻿//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using UnityEngine;

namespace AssetsShared
{
	[Serializable]
	public class DataTypeVector3 {

		public float x { get; set; }
		public float y { get; set; }
		public float z { get; set; }

		//
		public string getJsonString() {

			string s = "{";

			if (x != 0f) {
				s += "\"x\":" + x.ToString ();
			}

			if (y != 0f) {
				if (x != 0f) {
					s += ",";
				}
				s += "\"y\":" + y.ToString ();
			}

			if (z != 0f) {
				if (x != 0f || y != 0f) {
					s += ",";
				}
				s += "\"z\":" + z.ToString ();
			}

			s += "}";

			return s;
		}
	}
}