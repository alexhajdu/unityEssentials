using UnityEngine;
using System.Collections;

public class ShowForwardVector : MonoBehaviour
{
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.cyan;
		Vector3 direction = transform.TransformDirection ( Vector3.forward ) * 50;
		Gizmos.DrawRay ( transform.position, direction );
	}

}
