using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineRayCast : VRRayCast {

	LineDrawControl lines;

	void Start()
	{
		lines = GetComponent<LineDrawControl> ();
	}

	public override bool Raycast(out RaycastHit hit)
	{
		List<Vector3> results = new List<Vector3>();

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		results.Add(Camera.main.transform.position);

		bool result = Physics.Raycast (ray, out hit);

		results.Add (hit.point);

		lines.enabled = result;

		return result;
	}
}
