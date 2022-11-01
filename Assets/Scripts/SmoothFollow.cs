using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {
    
	// public Transform target;
	public string targetTag;
    public Vector3 offset = new Vector3(10, 10, 10);
    public float speed = 1f;

	private CachedMember<Transform> target;

	void Start() {
		target = new CachedMember<Transform>(() => GameObject.FindGameObjectWithTag(targetTag).transform);
	}

	void LateUpdate () {
		// Early out if we don't have a target
		if (target.Equals(null)) return;

        transform.position = Vector3.Lerp(transform.position, target.Value.position + offset, Time.deltaTime * speed);
	}
}