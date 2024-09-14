using UnityEngine;

public static class ColliderExtensions
{
	/// <param name="declineColliderGameObject"> Checks if collider and rigidbody game object's are the same </param>
	public static bool TryGetBodyGameObject(this Collider a, out GameObject result, bool declineColliderGameObject = false)
	{
		result = null;

		var attachedBody = a.GetBody();
		if (!attachedBody)
			return false;

		var otherAttachedBodyGO = attachedBody.gameObject;
		if (declineColliderGameObject && (a.gameObject == otherAttachedBodyGO))
			return false;

		result = otherAttachedBodyGO;
		return true;
	}

	public static Component GetBody(this Collider a)
		=> (a.attachedRigidbody as Component) ?? (a.attachedArticulationBody as Component);
}