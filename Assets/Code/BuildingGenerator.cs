using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BuildingGenerator : MonoBehaviour {

	public struct BuildingPosition {
		public Vector3 offset; 
		public GameObject buildingPrefab;

		public BuildingPosition(GameObject prefab, float x, float z) {
			offset = new Vector3(x, 1f, z);  // hardcode the offset from the floor level
			buildingPrefab = prefab;
		}
	}

	public GameObject pinkBuildingPrefab;  
	private BuildingPosition pink;
	public GameObject greenBuildingPrefab; 
	private BuildingPosition green; 
	public GameObject domePrefab;
	private BuildingPosition dome;

	public List<BuildingPosition> nextLayout; // update each time 

	// Use this for initialization
	void Awake () {
		pink = new BuildingPosition(pinkBuildingPrefab, 1f, 0f);
		green = new BuildingPosition(greenBuildingPrefab, 0f, 2f);
		dome = new BuildingPosition(domePrefab, -3f, 0f);

		nextLayout = new List<BuildingPosition>();
		nextLayout.Add(pink); 
		nextLayout.Add(green);
		nextLayout.Add(dome);
		Debug.Log(nextLayout);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void buildOne(Vector3 floorPosition, Quaternion rotation, BuildingPosition buildingPosition) {
		Instantiate(
			buildingPosition.buildingPrefab, 
			floorPosition + buildingPosition.offset, 
			rotation
		);
	}

	public void buildMany(Vector3 floorPosition, Quaternion rotation, List<BuildingPosition> buildings) {
		Debug.Log(buildings);
		for (int i = 0; i < buildings.Count; i++) {
			buildOne(floorPosition, rotation, buildings[i]);
		}
	}

	public void createBuildings(Vector3 floorPosition, Quaternion rotation) {
		Debug.Log(nextLayout);
		buildMany(floorPosition, rotation, nextLayout);
	}
}
