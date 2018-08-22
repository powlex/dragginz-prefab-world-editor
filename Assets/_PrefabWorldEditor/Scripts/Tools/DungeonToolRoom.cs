﻿//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class DungeonToolRoom : DungeonTool
    {
		private Part partFloor;
		private Part partWall;
		private Part partCorner;
		private Part partWallNF;
		private Part partCornerNF;

		public DungeonToolRoom(GameObject container) : base(container)
		{
			partFloor    = AssetManager.Instance.parts [Globals.PartList.Dungeon_Floor];
			partWall     = AssetManager.Instance.parts [Globals.PartList.Dungeon_Wall_L];
			partCorner   = AssetManager.Instance.parts [Globals.PartList.Dungeon_Corner];
			partWallNF   = AssetManager.Instance.parts [Globals.PartList.Dungeon_Wall_L_NF];
			partCornerNF = AssetManager.Instance.parts [Globals.PartList.Dungeon_Corner_NF];
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

			int x, z, y;
			for (x = xStart; x < xEnd; ++x) {
				for (z = zStart; z < zEnd; ++z) {
					for (y = 0; y <= _height; ++y) {
						
						Vector3 pos = new Vector3 (x * distance, y * distance, z * distance);

						isWall = false;

						if (y == _height)
						{
							if (!_ceiling) {
								continue;
							} else {
								partId = partFloor.id;
							}
						}
						else {
							if (x > xStart && z > zStart && x < (xEnd - 1) && z < (zEnd - 1)) {
								if (y > 0) {
									continue; // no floors on upper floors
								}
								partId = partFloor.id;
							} else {
								if ((x == xStart && z == zStart) || (x == (xEnd - 1) && z == (zEnd - 1)) || (x == xStart && z == (zEnd - 1)) || (x == (xEnd - 1) && z == zStart)) {
									if (y > 0) {
										partId = partCornerNF.id;
									} else {
										partId = partCorner.id;
									}
								} else {
									if (y > 0) {
										partId = partWallNF.id;
									} else {
										partId = partWall.id;
									}
								}
								isWall = true;
							}
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
							}

                            LevelController.Instance.setComponents (go, false, false);

							LevelController.LevelElement element = LevelController.Instance.createLevelElement(go, partId); //new LevelController.LevelElement ();

							_dungeonElements.Add (element);
						}
					}
				}
			}
		}
	}
}