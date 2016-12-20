using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CurveRayCast : VRRayCast {

	[SerializeField]
	private float divLength;
	[SerializeField]
	private float divAngle;
	[SerializeField]
	private float rate;
    [SerializeField]
    private GameObject rayObject;


	LineDrawControl lines;
	int counter;

	void Start()
	{
		lines = GetComponent<LineDrawControl> ();
	}

	void Update()
	{
        SteamVR_TrackedObject trackedObject = rayObject.GetComponent<SteamVR_TrackedObject>();
        if (trackedObject)
        {
            var device = SteamVR_Controller.Input((int)trackedObject.index);

            Vector2 position = device.GetAxis();
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                divAngle += position.x * rate;
                divLength += position.y;   
            }
        }
        else
        {
            float v = Input.mouseScrollDelta.y * rate;
            divAngle += v;
        }

		if (divAngle < 0) {
			divAngle = 0f;
		}
	}

	public override bool Raycast(out RaycastHit hit)
	{
		List<Vector3> results = new List<Vector3>();

		counter = 0;

		if (divLength > 0)
		{
            Vector3 pos = rayObject.transform.position;
            Vector3 dir = rayObject.transform.forward;


			Vector3 check = pos + dir;
			check.y = pos.y;
			check = check - pos;

			results.Add(pos);

			while (Vector3.Angle (check, dir) < 90 && counter < 360)
			{
				++counter;

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
