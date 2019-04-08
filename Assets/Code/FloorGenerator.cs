using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour {

	public Transform originalFloor;
	public float floorDistanceZ; 
	private Rigidbody player;

	// mostly a proof of concept for being able to render different tiles in order
	public Color[] colorList;
	public int floorIndex = 0;

	public GameObject mostRecentFloor; 
	
	public BuildingGenerator buildingGenerator;

	void Start () {
		player = GetComponent<Rigidbody>();
		buildingGenerator = gameObject.GetComponent<BuildingGenerator>();
		fillInFloor();
	}

	public void addFloor(float x, float y, float z){
		mostRecentFloor = Instantiate(originalFloor, new Vector3(x, y, z), Quaternion.identity).gameObject;
	}

	public Color getNextColor(){
		Color c = colorList[floorIndex];
		floorIndex++;
		if (floorIndex >= colorList.Length) floorIndex = 0;
		return c;
	}

	public GameObject makeNewFloor(Transform spawnObject, Vector3 position, Quaternion rotation){
		buildingGenerator.createBuildings(position, rotation);
		return Instantiate(spawnObject, position, rotation).gameObject;
	}

	public GameObject floorInFrontOfMe(GameObject currentFloor) {
		Transform tileTransform = currentFloor.transform;
		Vector3 position = currentFloor.transform.position;
		position.z += floorDistanceZ;
		Quaternion rotation = originalFloor.rotation;
		GameObject newFloor = makeNewFloor(originalFloor, position, rotation);
		MeshRenderer sr = newFloor.GetComponent<MeshRenderer>();
		sr.material.color = getNextColor();
		return newFloor;
	}

	public GameObject closestFloorTile(Rigidbody player){
		Vector3 playerPosition = player.transform.position;

		GameObject[] floorTiles = GameObject.FindGameObjectsWithTag("floor_tile");
		GameObject closest = floorTiles[0];

		// can skip first item since it is assigned by default
		for (int i = 1; i < floorTiles.Length; i++) {
			GameObject o = floorTiles[i];
			if(
				Vector3.Distance(o.transform.position, player.transform.position) <
				Vector3.Distance(closest.transform.position, player.transform.position)
			) {
				closest = o; 
			}
		}
		return closest;
	}
	public void fillInFloor(){
		print("filling in floor...");
		//GameObject closest = closestFloorTile(player);
		GameObject newFloor = floorInFrontOfMe(mostRecentFloor);
		mostRecentFloor = newFloor;
	}

	void OnCollisionEnter(Collision collision){
		// get the thing we hit 
		GameObject other = collision.collider.gameObject;
		// if it is the most recently created floor: create the next one 
		if (other == mostRecentFloor) fillInFloor();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space"))
        {
            print("adding a new floor tile");
			fillInFloor();
        }
	}
}
