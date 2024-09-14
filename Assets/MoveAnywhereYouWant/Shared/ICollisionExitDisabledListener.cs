using UnityEngine;

public interface ICollisionExitDisabledListener
{
	/// <summary> Called when this collider/rigidbody has stopped touching another rigidbody/collider </summary>
	/// <remarks> Gets called at OnDisable() </remarks>
	public void OnCollisionExitDisabled(Collider thisCollider, Collider otherCollider);
}