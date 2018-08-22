//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;

namespace PrefabWorldEditor
{
	public class DungeonToolMaze : DungeonTool
    {
		private Part partFloor;
		private Part partWall;
		private Part partCorner;
		private Part partTurn;

		public DungeonToolMaze(GameObject container) : base(container)
		{
			partWall   = AssetManager.Instance.parts [Globals.PartList.Dungeon_Wall_L];
			partCorner = AssetManager.Instance.parts [Globals.PartList.Dungeon_Corner];
			partTurn   = AssetManager.Instance.parts [Globals.PartList.Dungeon_Turn];
		}

		// ------------------------------------------------------------------------
		public override void createObjects()
		{
			GameObject go;
			Globals.PartList partId;

			float distance = _cubeSize;
			bool isWall = false;

			int xStart = _width / 2 * -1;
			int xEnd = xStart + _width;
			int zStart = _depth / 2 * -1;
			int zEnd = zStart + _depth;

			int x, z;
			for (x = xStart; x < xEnd; ++x) {
				for (z = zStart; z < zEnd; ++z) {

					Vector3 pos = new Vector3 (x * distance, 0, z * distance);

					isWall = false;
					if (x > xStart && z > zStart && x < (xEnd - 1) && z < (zEnd - 1)) {
						partId = partTurn.id;
					} else {
						if ((x == xStart && z == zStart) || (x == (xEnd - 1) && z == (zEnd - 1)) || (x == xStart && z == (zEnd - 1)) || (x == (xEnd - 1) && z == zStart)) {
							partId = partCorner.id;
						} else {
							partId = partWall.id;
						}
						isWall = true;
					}

					go = PrefabLevelEditor.Instance.createPartAt (partId, 0, 0, 0);

					if (go != null) {
						go.name = "temp_part_" + _container.transform.childCount.ToString ();
						go.transform.SetParent (_container.transform);
						go.transform.localPosition = pos;

						if (isWall) {
							if (x == xStart && z == (zEnd - 1)) {
								go.transform.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
							} else if (x == (xEnd - 1) && z == zStart) {
								go.transform.rotation = Quaternion.Euler (new Vector3 (0, 270, 0));
							} else if (x == xStart) {
								go.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
							} else if (x == (xEnd - 1)) {
								go.transform.rotation = Quaternion.Euler (new Vector3 (0, 180, 0));
							} else if (z == zStart) {
								go.transform.rotation = Quaternion.Euler (new Vector3 (0, 270, 0));
							} else if (z == (zEnd - 1)) {
								go.transform.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
							}
						} else {
							go.transform.rotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 4) * 90, 0));
						}

                        LevelController.Instance.setComponents (go, false, false);

                        LevelController.LevelElement element = LevelController.Instance.createLevelElement(go, partId); // new LevelController.LevelElement ();

						_dungeonElements.Add (element);
					}
				}
			}
		}
	}
}