﻿using UnityEngine;
using System.Collections;

public class VRWapper : MonoBehaviour {
    [SerializeField]
    private float x;
    [SerializeField]
    private float y;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private GameObject can;
    [SerializeField]
    private GameObject cannot;

	[SerializeField]
	private GameObject test;

	[SerializeField]
	private GameObject navTest;

	[SerializeField]
	private Vector3 debug1;
	[SerializeField]
	private Vector3 debug2;

	NavMeshAgent agent;
	BoxCollider box;

	// Use this for initialization
	void Start () {
		agent = test.GetComponent<NavMeshAgent> ();
		box = test.GetComponent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
        y += Input.GetAxis("Horizontal");
        x += Input.GetAxis("Vertical");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Camera.main.transform.rotation = Quaternion.Euler(x, y, 0);

        RaycastHit hit;

        target.SetActive(false);

        if (Physics.Raycast(ray, out hit))
        {
            target.SetActive(true);

            target.transform.position = hit.point;

            float distance = Vector3.Distance(Camera.main.transform.position, hit.point) / 30;
            target.transform.localScale = new Vector3(distance, distance, distance);


			debug1 = hit.point;

			Collider[] cs = Physics.OverlapBox (hit.point + offset, box.size);
			NavMeshPath path = new NavMeshPath ();
			agent.CalculatePath (target.transform.position, path);

			NavMeshHit navHit;
			bool nearest = NavMesh.SamplePosition (hit.point, out navHit, 1.0f, NavMesh.AllAreas);
			navTest.SetActive (false);
			if (nearest) {
				navTest.transform.position = navHit.position;
				Ray navRay = new Ray (navHit.position, Camera.main.transform.position - navHit.position);

				debug2 = navHit.position;

				if (Physics.Raycast (navRay, out hit)) {
					float tmpD = Vector3.Distance (Camera.main.transform.position, navHit.position);
					nearest = hit.distance > tmpD;
				}
			}



			bool canReach = path.status == NavMeshPathStatus.PathComplete && cs.Length == 0 && nearest && debug1.x == debug2.x && debug1.z == debug2.z;



			can.SetActive(canReach);
			cannot.SetActive(!canReach);

			if (canReach && Input.GetMouseButtonUp(1))
            {
				Camera.main.transform.position = target.transform.position + offset;
            }
        } 
	}
}
