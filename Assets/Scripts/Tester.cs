using System.Collections;
using UnityEngine;

public sealed partial class Tester : MonoBehaviour
{
	[Header("Collision Test")]
	#region Collision Test

	[SerializeField]
	private Rigidbody collisionRigidbody;

	[SerializeField]
	private Transform childCollisionColliderContainer;


	#endregion

	[Header("Trigger Test")]
	#region Trigger Test

	[SerializeField]
	private GameObject triggerContainer;


	#endregion

	[Header("Other")]
	#region Other

	[SerializeField]
	private Transform floor;

	[SerializeField]
	private bool testCollision = true;

	[SerializeField]
	private bool testTrigger = true;

	private readonly WaitForSecondsRealtime wait = new(5f);


	#endregion


	// Initialize
	private IEnumerator Start()
	{
		if (testCollision)
			yield return TestCollision();

		yield return null;

		if (testTrigger)
			yield return TestTrigger();
	}

	private IEnumerator TestCollision()
	{
		Debug.LogWarning("Starting Collision Test");
		floor.gameObject.SetActive(true);
		triggerContainer.SetActive(false);
		var childCollisionCollider = childCollisionColliderContainer.GetComponent<Collider>();

		yield return wait;

		Debug.LogWarningFormat("Disabling {0}. Next: Enable Collider", childCollisionCollider.name);
		collisionRigidbody.isKinematic = true;
		childCollisionCollider.enabled = false;

		yield return wait;

		Debug.LogWarningFormat("Enabled {0}. Next: Destroy Collider", childCollisionCollider.name);
		collisionRigidbody.isKinematic = false;
		childCollisionCollider.enabled = true;

		yield return wait;

		Debug.LogWarningFormat("Destroyed {0}. Next: Create Collider", childCollisionCollider.name);
		collisionRigidbody.isKinematic = true;
		Destroy(childCollisionCollider);

		yield return wait;

		childCollisionCollider = childCollisionColliderContainer.gameObject.AddComponent<SphereCollider>();
		collisionRigidbody.isKinematic = false;
		Debug.LogWarningFormat("Created Collider {0}. Next: Destroy Collider GameObject", childCollisionCollider.name);

		yield return wait;

		Debug.LogWarningFormat("Destroyed Collider GameObject {0}. Finished", childCollisionCollider.gameObject.name);
		collisionRigidbody.isKinematic = true;
		Destroy(childCollisionCollider.gameObject);
	}

	private IEnumerator TestTrigger()
	{
		Debug.LogWarning("Starting Trigger Test");
		floor.gameObject.SetActive(false);
		triggerContainer.SetActive(true);
		if (!childCollisionColliderContainer)
		{
			childCollisionColliderContainer = new GameObject("ChildCollider", typeof(SphereCollider), typeof(DisabledColliderNotifier)).transform;
			childCollisionColliderContainer.parent = collisionRigidbody.transform;
			Debug.LogWarningFormat("Created Collider and its GameObject. Next: Disable Collider");
		}

		collisionRigidbody.isKinematic = true;
		var childCollisionCollider = childCollisionColliderContainer.GetComponent<SphereCollider>();

		yield return wait;

		Debug.LogWarningFormat("Disabling {0}. Next: Enable Collider", childCollisionCollider.name);
		childCollisionCollider.enabled = false;

		yield return wait;

		Debug.LogWarningFormat("Enabled {0}. Next: Destroy Collider", childCollisionCollider.name);
		childCollisionCollider.enabled = true;

		yield return wait;

		Debug.LogWarningFormat("Destroyed {0}. Next: Create Collider", childCollisionCollider.name);
		Destroy(childCollisionCollider);

		yield return wait;

		childCollisionCollider = childCollisionColliderContainer.gameObject.AddComponent<SphereCollider>();
		Debug.LogWarningFormat("Created Collider {0}. Next: Destroy GameObject", childCollisionCollider.name);

		yield return wait;

		Debug.LogWarningFormat("Destroyed Collider GameObject {0}. Finished", childCollisionCollider.gameObject.name);
		Destroy(childCollisionColliderContainer.gameObject);
	}
}


#if UNITY_EDITOR

public sealed partial class Tester
{ }


#endif
