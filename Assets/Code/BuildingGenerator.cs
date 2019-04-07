using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour {


	public GameObject pinkBuildingPrefab; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void createBuildings(Vector3 position, Vector3 buildingAdjust, Quaternion rotation) {
		Instantiate(pinkBuildingPrefab, position + buildingAdjust, rotation);
	}
}
