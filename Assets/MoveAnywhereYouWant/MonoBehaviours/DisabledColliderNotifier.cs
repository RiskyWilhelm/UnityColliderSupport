using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary> Allows other collider's and attached rigidbody's <see cref="ICollisionExitListener"/> and <see cref="ITriggerExitDisabledListener"/>(s) to receive <see cref="ICollisionExitListener.OnCollisionExit(Collision)"/> and <see cref="ITriggerExitDisabledListener"/> events </summary>
[DisallowMultipleComponent]
public sealed partial class DisabledColliderNotifier : MonoBehaviour, ICollisionListener
{
	private readonly Dictionary<Collider, DisabledColliderListenerTrigger> interactingTriggersDict = new(); // Dict<this, other>

	private readonly Dictionary<Collider, Collider> interactingCollisionsDict = new(); // Dict<other, this>


	// Update
	private void LateUpdate()
	{
		CheckCollisionColliderStates();
		CheckTriggerColliderStates();
    }

	private void CheckTriggerColliderStates()
	{
		var cachedDict = DictionaryPool<Collider, DisabledColliderListenerTrigger>.Get();

		foreach (var iteratedCollisionPair in interactingTriggersDict)
			cachedDict[iteratedCollisionPair.Key] = iteratedCollisionPair.Value;

		foreach (var iteratedTriggerPair in cachedDict)
		{
			var thisCollider = iteratedTriggerPair.Key;
			var otherTrigger = iteratedTriggerPair.Value;

			if (!thisCollider || !otherTrigger)
			{
				Debug.LogError("You must not destroy colliders invidually. Unable to send any events", this);
				interactingTriggersDict.Remove(thisCollider);
				continue;
			}

			if (!thisCollider.enabled)
			{
				interactingTriggersDict.Remove(thisCollider);
				NotifyDisabledTrigger(thisCollider, otherTrigger);
			}

			if (!otherTrigger.enabled)
				interactingCollisionsDict.Remove(thisCollider);
		}

		DictionaryPool<Collider, DisabledColliderListenerTrigger>.Release(cachedDict);
	}

	private void CheckCollisionColliderStates()
	{
		var cachedDict = DictionaryPool<Collider, Collider>.Get();

		foreach (var iteratedCollisionPair in interactingCollisionsDict)
			cachedDict[iteratedCollisionPair.Key] = iteratedCollisionPair.Value;

		foreach (var iteratedCollisionPair in cachedDict)
		{
			var thisCollider = iteratedCollisionPair.Value;
			var otherCollider = iteratedCollisionPair.Key;

			if (!thisCollider || !otherCollider)
			{
				Debug.LogError("You must not destroy colliders invidually. Unable to send any events", this);
				interactingCollisionsDict.Remove(otherCollider);
				continue;
			}

			if (!thisCollider.enabled)
			{
				interactingCollisionsDict.Remove(otherCollider);
				NotifyDisabledCollision(otherCollider, thisCollider);
				NotifyDisabledCollision(thisCollider, otherCollider);
			}

			if (!otherCollider.enabled)
				interactingCollisionsDict.Remove(otherCollider);
		}

		DictionaryPool<Collider, Collider>.Release(cachedDict);
	}

	internal void OnEnterTrigger(Collider thisCollider, DisabledColliderListenerTrigger otherTrigger)
		=> OnStayTrigger(thisCollider, otherTrigger);

	internal void OnStayTrigger(Collider thisCollider, DisabledColliderListenerTrigger otherTrigger)
	{
		interactingTriggersDict[thisCollider] = otherTrigger;
	}

	internal void OnExitTrigger(Collider thisCollider, DisabledColliderListenerTrigger otherTrigger)
	{
		interactingTriggersDict.Remove(thisCollider);
	}

	public void OnCollisionEnter(Collision collision)
		=> OnCollisionStay(collision);

	public void OnCollisionStay(Collision collision)
	{
		var initialContact = collision.GetContact(0);
		interactingCollisionsDict[initialContact.otherCollider] = initialContact.thisCollider;
	}

	public void OnCollisionExit(Collision collision)
	{
		interactingCollisionsDict.Remove(collision.collider);
	}

	public void OnCollisionExitDisabled(Collider thisCollider, Collider otherCollider)
	{
		interactingCollisionsDict.Remove(otherCollider);
	}


	// Dispose
	private void OnDisable()
	{
		NotifyAllTriggerColliders();
		NotifyAllCollisionColliders();
		interactingTriggersDict.Clear();
		interactingCollisionsDict.Clear();
	}

	private void NotifyAllTriggerColliders()
	{
		var cachedDict = DictionaryPool<Collider, DisabledColliderListenerTrigger>.Get();

		foreach (var iteratedCollisionPair in interactingTriggersDict)
			cachedDict[iteratedCollisionPair.Key] = iteratedCollisionPair.Value;

		foreach (var iteratedTriggerPair in cachedDict)
		{
			var thisCollider = iteratedTriggerPair.Key;
			var otherTrigger = iteratedTriggerPair.Value;

			NotifyDisabledTrigger(thisCollider, otherTrigger);
		}

		DictionaryPool<Collider, DisabledColliderListenerTrigger>.Release(cachedDict);
	}

	private void NotifyAllCollisionColliders()
	{
		var cachedDict = DictionaryPool<Collider, Collider>.Get();

		foreach (var iteratedCollisionPair in interactingCollisionsDict)
			cachedDict[iteratedCollisionPair.Key] = iteratedCollisionPair.Value;

        foreach (var iteratedCollisionPair in cachedDict)
        {
			var thisCollider = iteratedCollisionPair.Value;
			var otherCollider = iteratedCollisionPair.Key;

			NotifyDisabledCollision(thisCollider, otherCollider);
			NotifyDisabledCollision(otherCollider, thisCollider);
		}

		DictionaryPool<Collider, Collider>.Release(cachedDict);
	}

	private void NotifyDisabledTrigger(Collider thisCollider, DisabledColliderListenerTrigger otherTrigger)
	{
		var cachedList = ListPool<ITriggerExitDisabledListener>.Get();
		otherTrigger.GetComponents<ITriggerExitDisabledListener>(cachedList);

		foreach (var iteratedReceiver in cachedList)
		{
			try
			{
				iteratedReceiver.OnTriggerExitDisabled(thisCollider);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		// By default, unity sends message to the attached body's game object too. Next code part will attempt to notify collided collider's body
		if (!otherTrigger.GetComponent<Collider>().TryGetBodyGameObject(out GameObject otherAttachedBodyGO, declineColliderGameObject: true))
			return;

		otherAttachedBodyGO.GetComponents<ITriggerExitDisabledListener>(cachedList);
		foreach (var iteratedReceiver in cachedList)
		{
			try
			{
				iteratedReceiver.OnTriggerExitDisabled(thisCollider);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		ListPool<ITriggerExitDisabledListener>.Release(cachedList);
	}

	private void NotifyDisabledCollision(Collider thisCollider, Collider otherCollider)
	{
		var cachedList = ListPool<ICollisionExitDisabledListener>.Get();
		otherCollider.GetComponents<ICollisionExitDisabledListener>(cachedList);

        foreach (var iteratedReceiver in cachedList)
		{
			try
			{
				iteratedReceiver.OnCollisionExitDisabled(otherCollider, thisCollider);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		// By default, unity sends message to the attached body's game object too. Next code part will attempt to notify collided collider's body
		if (!otherCollider.TryGetBodyGameObject(out GameObject otherAttachedBodyGO, declineColliderGameObject: true))
			return;

		otherAttachedBodyGO.GetComponents<ICollisionExitDisabledListener>(cachedList);
		foreach (var iteratedReceiver in cachedList)
		{
			try
			{
				iteratedReceiver.OnCollisionExitDisabled(otherCollider, thisCollider);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}
		
		ListPool<ICollisionExitDisabledListener>.Release(cachedList);
	}
}


#if UNITY_EDITOR

public sealed partial class DisabledColliderNotifier
{ }


#endif
