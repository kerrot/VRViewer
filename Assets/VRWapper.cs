using UnityEngine;
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


			Collider[] cs = Physics.OverlapBox (hit.point + offset, box.size);
			NavMeshPath path = new NavMeshPath ();
			agent.CalculatePath (target.transform.position, path);

			float tmpAngle = Vector3.Angle(hit.normal, Vector3.up);

			bool canReach = path.status == NavMeshPathStatus.PathComplete && cs.Length == 0 && tmpAngle < 90;
				


			can.SetActive(canReach);
			cannot.SetActive(!canReach);

			if (canReach && Input.GetMouseButtonUp(1))
            {
				Camera.main.transform.position = target.transform.position + offset;
            }
        } 
	}
}
