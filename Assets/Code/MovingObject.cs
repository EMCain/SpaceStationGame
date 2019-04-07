using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// based on https://unity3d.com/learn/tutorials/projects/-roguelike-tutorial/moving-object-script?playlist=17150
public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;
	public float speed = 0.5f;
	public float zAdjust = 0.75f; 
	// adjusts wonky visuals where moving on Z axis seems faster
	public LayerMask blockingLayer;
	private BoxCollider boxCollider;
	private Rigidbody rb;
	private float inverseMoveTime; 

	private bool stopped = false;

	private IEnumerator smoothMovementEnumerator = null;

	// Use this for initialization
	protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider>();
		rb = GetComponent<Rigidbody>();
		inverseMoveTime = 1f/moveTime;
	}

	protected bool Move(int xDir, int zDir, out RaycastHit hit) {
		Vector3 start = transform.position;
		Vector3 end = start + new Vector3(xDir, 0, zDir * zAdjust) * speed;


		bool success = Physics.Raycast(start, end, out hit, 10, blockingLayer);

		if (hit.transform == null) {
			if (smoothMovementEnumerator != null) {
				StopCoroutine(smoothMovementEnumerator);
			}
			smoothMovementEnumerator = SmoothMovement(end);
			StartCoroutine(smoothMovementEnumerator);
		}
		return success;
	}

	// TODO debug weird jerky angular movement on direction change
	protected IEnumerator SmoothMovement(Vector3 end) {
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards(
				rb.position, 
				end, 
				inverseMoveTime * Time.deltaTime
			);
			rb.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected virtual void AttemptMove<T> (int xDir, int zDir)
		where T : Component
	{
		RaycastHit hit;
		bool canMove = Move(xDir, zDir, out hit);

		if (hit.transform == null) {
			// if nothing was hit we're done
			return;
		}

		// get the thing that was hit
		T hitComponent = hit.transform.GetComponent<T> ();

		if (hitComponent != null) {
			// can't move because of hitComponent
			OnCantMove(hitComponent);
		}

	}


	protected abstract void OnCantMove<T> (T component)
		where T : Component;


	// Update is called once per frame
	void Update () {
		
	}

}
