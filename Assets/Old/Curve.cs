using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class Curve : MonoBehaviour {
    [SerializeField]
    private float x;
    [SerializeField]
    private float y;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float divLength;
	[SerializeField]
	private float divAngle;

    [SerializeField]
    private GameObject can;
    [SerializeField]
    private GameObject cannot;

    [SerializeField]
    private float angle;


    [SerializeField]
    private float debug;

    LineDrawControl lines;

    // Use this for initialization
    void Start()
    {
        lines = GetComponent<LineDrawControl>();
    }

    // Update is called once per frame
    void Update()
    {
        y += Input.GetAxis("Horizontal");
        x += Input.GetAxis("Vertical");

        Camera.main.transform.rotation = Quaternion.Euler(x, y, 0);

        RaycastHit hit;

        target.SetActive(false);

        if (curveRaycast(out hit))
        {
            target.SetActive(true);

            target.transform.position = hit.point;

            float distance = Vector3.Distance(Camera.main.transform.position, hit.point) / 30;
            target.transform.localScale = new Vector3(distance, distance, distance);

			float tmpAngle = Vector3.Angle(hit.normal, Vector3.up);
            

			can.SetActive(tmpAngle < angle);
			cannot.SetActive(tmpAngle >= angle);

			if (tmpAngle < angle)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    Camera.main.transform.position = target.transform.position + offset;
                }
            }
        }
    }

    bool curveRaycast(out RaycastHit hit)
    {
        List<Vector3> results = new List<Vector3>();

        if (divLength > 0)
        {
			Vector3 pos = Camera.main.transform.position;
			Vector3 dir = Camera.main.transform.forward;


			Vector3 check = pos + dir;
			check.y = pos.y;
			check = check - pos;
            
            results.Add(pos);

			while (Vector3.Angle (check, dir) < 90)
            {
                Vector3 tmpPos = pos + dir * divLength;
                results.Add(tmpPos);

                if (Physics.Raycast(pos, dir, out hit, divLength))
                {
                    lines.points = results.ToArray();

                    return true;
                }

                pos = tmpPos;
				dir = Quaternion.AngleAxis(divAngle, Camera.main.transform.right) * dir;
				results.Add(tmpPos);
            }
        }

        lines.points = results.ToArray();

        hit = new RaycastHit();

        return false;
    }
}
