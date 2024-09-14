using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary> Allows child <see cref="ICollisionEnterListener"/>, <see cref="ICollisionStayListener"/> and <see cref="ICollisionExitListener"/>(s) to receive events </summary>
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public sealed partial class ChildCollisionNotifier : MonoBehaviour
{
	private readonly Dictionary<Collider, Collider> interactingCollisionDict = new(); // Dict<other, this>


	// Update
	private void NotifyChildCollisionEnter(Collider selfChildCollider, Collision collision)
	{
		var cachedList = ListPool<ICollisionEnterListener>.Get();
		selfChildCollider.GetComponents<ICollisionEnterListener>(true, cachedList);

		foreach (var iteratedReceiver in cachedList)
		{
			try
			{
				iteratedReceiver.OnCollisionEnter(collision);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}
		
		ListPool<ICollisionEnterListener>.Release(cachedList);
	}

	private void NotifyChildCollisionStay(Collider selfChildCollider, Collision collision)
	{
		var cachedList = ListPool<ICollisionStayListener>.Get();
		selfChildCollider.GetComponents<ICollisionStayListener>(true, cachedList);

		foreach (var iteratedReceiver in cachedList)
		{
			try
			{
				iteratedReceiver.OnCollisionStay(collision);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}
		
		ListPool<ICollisionStayListener>.Release(cachedList);
	}

	private void NotifyChildCollisionExit(Collider selfChildCollider, Collision collision)
	{
		var cachedList = ListPool<ICollisionExitListener>.Get();
		selfChildCollider.GetComponents<ICollisionExitListener>(true, cachedList);

		foreach (var iteratedReceiver in cachedList)
		{
			try
			{
				iteratedReceiver.OnCollisionExit(collision);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		ListPool<ICollisionExitListener>.Release(cachedList);
	}

	private void OnCollisionEnter(Collision collision)
	{
		var initialContact = collision.GetContact(0);
		var thisCollider = initialContact.thisCollider;
		interactingCollisionDict[initialContact.otherCollider] = thisCollider;

		if (!IsColliderInRigidbodyHiearchy(thisCollider))
			NotifyChildCollisionEnter(thisCollider, collision);
	}

	private void OnCollisionStay(Collision collision)
	{
		var initialContact = collision.GetContact(0);
		var thisCollider = initialContact.thisCollider;

		if (!IsColliderInRigidbodyHiearchy(thisCollider))
			NotifyChildCollisionStay(thisCollider, collision);
	}

	private void OnCollisionExit(Collision collision)
	{
		var otherCollider = collision.collider;
		var thisCollider = interactingCollisionDict[otherCollider];
		interactingCollisionDict.Remove(otherCollider);

		if (!IsColliderInRigidbodyHiearchy(thisCollider))
			NotifyChildCollisionExit(thisCollider, collision);
	}

	private bool IsColliderInRigidbodyHiearchy(Collider collider)
	{
		return (collider.gameObject == this.gameObject);
	}
}


#if UNITY_EDITOR

public sealed partial class ChildCollisionNotifier
{ }


#endif
