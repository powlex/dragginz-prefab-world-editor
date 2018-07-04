//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class LevelData : Singleton<LevelData> {

		public string lastLevelName = Globals.defaultLevelName;
		public int currentLevelId = -1;

		public void loadLevelFromJson(string json)
		{
			//LevelEditor.Instance.curLevelChunk.reset ();

			LevelFile levelFile = null;
			//try {
				levelFile = createDataFromJson(json);
				if (levelFile != null) {
					createLevel (levelFile);
				}
			//}
			//catch (System.Exception e) {
			//	Debug.LogWarning (e.Message);
			//	AppController.Instance.showPopup (PopupMode.Notification, "Warning", Globals.warningInvalidFileFormat.Replace("%1",""));
			//}
		}

		//
		public void loadLevelDataFromFile(string fileName)
		{
			//LevelEditor.Instance.curLevelChunk.reset ();

			string json = File.ReadAllText(fileName);

			LevelFile levelFile = null;
			try {
				levelFile = createDataFromJson(json);
				if (levelFile != null) {
					createLevel (levelFile);
				}
			}
			catch (System.Exception e) {
				Debug.LogWarning (e.Message);
				PweMainMenu.Instance.popup.showPopup (Globals.PopupMode.Notification, "Warning", Globals.txtWarningInvalidFileFormat.Replace("%1",""));
			}
		}

		//
		private LevelFile createDataFromJson(string json) {

			LevelFile levelFile = new LevelFile ();

			levelFile.parseJson (json);

			return levelFile;
		}

		//
		private void createLevel(LevelFile levelFile) {

			if (levelFile.fileFormatVersion != Globals.levelSaveFormatVersion) {
				PweMainMenu.Instance.popup.showPopup (Globals.PopupMode.Notification, "Warning", Globals.txtWarningObsoleteFileFormat);
				return;
			}

			currentLevelId = levelFile.levelId;

			PweMainMenu.Instance.setLevelNameText(levelFile.levelName);
			lastLevelName = levelFile.levelName;

			PrefabLevelEditor.Instance.newLevelWithDimensions ((int)levelFile.levelSize.x, (int)levelFile.levelSize.y, (int)levelFile.levelSize.z);

			Vector3 savedPos = new Vector3 (levelFile.playerPosition.x, levelFile.playerPosition.y, levelFile.playerPosition.z);
			Vector3 savedRot = new Vector3 (levelFile.playerEuler.x, levelFile.playerEuler.y, levelFile.playerEuler.z);

			FlyCam.Instance.setNewInitialPosition (savedPos, savedRot);

			if (levelFile.levelObjects != null) {
				
				LevelObject levelObj;
				Globals.PartList partId;
				PrefabLevelEditor.Part part;

				Vector3 pos = Vector3.zero;
				Quaternion rotation = Quaternion.identity;

				PrefabLevelEditor prefabLevelEditor = PrefabLevelEditor.Instance;
				LevelController levelController = LevelController.Instance;

				int i, len = levelFile.levelObjects.Count;
				for (i = 0; i < len; ++i)
				{
					levelObj = levelFile.levelObjects [i];

					partId = (Globals.PartList)levelObj.id;
					part = prefabLevelEditor.parts [partId];

					pos.x = levelObj.position.x;
					pos.y = levelObj.position.y;
					pos.z = levelObj.position.z;

					rotation.w = levelObj.rotation.w;
					rotation.x = levelObj.rotation.x;
					rotation.y = levelObj.rotation.y;
					rotation.z = levelObj.rotation.z;

					LevelController.LevelElement element = new LevelController.LevelElement ();
					element.part = partId;
					element.go = prefabLevelEditor.createPartAt (partId, pos.x, pos.y, pos.z);
					element.go.transform.rotation = rotation;

					levelController.setMeshCollider (element.go, true);
					levelController.setRigidBody (element.go, part.usesGravity);

					levelController.levelElements.Add (element.go.name, element);
				}
			}
		}

		//
		public void saveLevelData(string filename, string levelName) {

			lastLevelName = levelName;

			LevelFile levelFile = createLevelData (levelName);
			if (levelFile == null) {
				return;
			}

			string json = levelFile.getJsonString();

			File.WriteAllText (filename, json);
		}

		//
		private LevelFile createLevelData(string levelName) {

			LevelFile levelFile = new LevelFile ();
			levelFile.fileFormatVersion = Globals.levelSaveFormatVersion;

			levelFile.levelId    = -1;
			levelFile.levelName  = levelName;

			levelFile.levelPos   = new DataTypeVector3 ();
			levelFile.levelPos.x = 0;
			levelFile.levelPos.y = 0;
			levelFile.levelPos.z = 0;

			levelFile.levelSize   = new DataTypeVector3 ();
			levelFile.levelSize.x = PrefabLevelEditor.Instance.levelSize.x;
			levelFile.levelSize.y = PrefabLevelEditor.Instance.levelSize.y;
			levelFile.levelSize.z = PrefabLevelEditor.Instance.levelSize.z;

			levelFile.playerPosition   = new DataTypeVector3 ();
			levelFile.playerPosition.x = FlyCam.Instance.player.position.x;
			levelFile.playerPosition.y = FlyCam.Instance.player.position.y;
			levelFile.playerPosition.z = FlyCam.Instance.player.position.z;

			levelFile.playerEuler   = new DataTypeVector3 ();
			levelFile.playerEuler.x = FlyCam.Instance.player.eulerAngles.x;
			levelFile.playerEuler.y = FlyCam.Instance.player.eulerAngles.y;
			levelFile.playerEuler.z = FlyCam.Instance.player.eulerAngles.z;

			List<LevelObject> levelObjects = new List<LevelObject> ();

			Dictionary<string, LevelController.LevelElement> elements = LevelController.Instance.levelElements;
			foreach (KeyValuePair<string, LevelController.LevelElement> element in elements)
			{
				LevelController.LevelElement e = element.Value;

				LevelObject levelObj = new LevelObject ();
				levelObj.id = (int)e.part;

				levelObj.position   = new DataTypeVector3 ();
				levelObj.position.x = e.go.transform.position.x;
				levelObj.position.y = e.go.transform.position.y;
				levelObj.position.z = e.go.transform.position.z;

				levelObj.rotation = new DataTypeQuaternion ();
				levelObj.rotation.w = e.go.transform.rotation.w;
				levelObj.rotation.x = e.go.transform.rotation.x;
				levelObj.rotation.y = e.go.transform.rotation.y;
				levelObj.rotation.z = e.go.transform.rotation.z;

				levelObjects.Add (levelObj);	
			}

			levelFile.levelObjects = levelObjects;

			return levelFile;
		}
	}
}