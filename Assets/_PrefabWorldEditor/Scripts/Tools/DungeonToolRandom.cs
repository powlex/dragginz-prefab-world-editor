//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class DungeonToolRandom : DungeonTool
    {
		private Part[] parts;

		public DungeonToolRandom(GameObject container) : base(container)
		{
			parts = new Part[7];
			parts[0] = AssetManager.Instance.parts [Globals.PartList.Dungeon_Floor];
			parts[1] = AssetManager.Instance.parts [Globals.PartList.Dungeon_Wall_L];
			parts[2] = AssetManager.Instance.parts [Globals.PartList.Dungeon_Wall_LR];
			parts[3] = AssetManager.Instance.parts [Globals.PartList.Dungeon_Corner];
			parts[4] = AssetManager.Instance.parts [Globals.PartList.Dungeon_DeadEnd];
			parts[5] = AssetManager.Instance.parts [Globals.PartList.Dungeon_Turn];
			parts[6] = AssetManager.Instance.parts [Globals.PartList.Dungeon_Floor];
		}

		// ------------------------------------------------------------------------
		public override void createObjects()
		{
			GameObject go;
			Globals.PartList partId;

			float distance = _cubeSize;

			int xStart = _width / 2 * -1;
			int xEnd = xStart + _width;
			int zStart = _depth / 2 * -1;
			int zEnd = zStart + _depth;

			int x, z;
			for (x = xStart; x < xEnd; ++x) {
				for (z = zStart; z < zEnd; ++z) {

					Vector3 pos = new Vector3 (x * distance, 0, z * distance);

					partId = parts [Random.Range (0, parts.Length)].id;
					go = PrefabLevelEditor.Instance.createPartAt (partId, 0, 0, 0);

					if (go != null) {
						go.name = "temp_part_" + _container.transform.childCount.ToString ();
						go.transform.SetParent (_container.transform);
						go.transform.localPosition = pos;
						go.transform.rotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 4) * 90, 0));

                        LevelController.Instance.setComponents (go, false, false);

						LevelController.LevelElement element = LevelController.Instance.createLevelElement(go, partId); //new LevelController.LevelElement ();

						_dungeonElements.Add (element);
					}
				}
			}
		}
	}
}