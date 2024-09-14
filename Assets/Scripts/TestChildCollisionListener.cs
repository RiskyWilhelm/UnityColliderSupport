using UnityEngine;

public sealed partial class TestChildCollisionListener : MonoBehaviour, ICollisionListener
{
	// Update
	public void OnCollisionEnter(Collision collision)
	{
		var initialContact = collision.GetContact(0);
		Debug.LogFormat(this, "{0} has taken OnEnterCollision of {1}-{2}", this.gameObject.name, initialContact.thisCollider.gameObject.name, initialContact.otherCollider.gameObject.name);
	}

	public void OnCollisionStay(Collision collision)
	{
		var initialContact = collision.GetContact(0);
		Debug.LogFormat(this, "{0} has taken OnCollisionStay of {1}-{2}", this.gameObject.name, initialContact.thisCollider.gameObject.name, initialContact.otherCollider.gameObject.name);
	}

	public void OnCollisionExit(Collision collision)
	{
		var initialContact = collision.GetContact(0);
		Debug.LogFormat(this, "{0} has taken OnCollisionExit of {1}-{2}", this.gameObject.name, initialContact.thisCollider.gameObject.name, initialContact.otherCollider.gameObject.name);
	}

	public void OnCollisionExitDisabled(Collider thisCollider, Collider otherCollider)
	{
		Debug.LogFormat(this, "{0} has taken OnCollisionExitDisabled of {1}-{2}", this.gameObject.name, thisCollider.gameObject.name, otherCollider.gameObject.name);
	}
}


#if UNITY_EDITOR

public sealed partial class TestChildCollisionListener
{ }


#endif
