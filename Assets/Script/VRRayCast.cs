using UnityEngine;
using System.Collections;

abstract public class VRRayCast : MonoBehaviour {

	abstract public bool Raycast (out RaycastHit hit);
}
