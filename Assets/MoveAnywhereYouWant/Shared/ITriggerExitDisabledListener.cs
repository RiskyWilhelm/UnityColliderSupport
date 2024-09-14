using UnityEngine;

public interface ITriggerExitDisabledListener
{
	/// <summary> Called when the other Collider has stopped touching the trigger </summary>
	/// <remarks> Gets called at OnDisable() </remarks>
	public void OnTriggerExitDisabled(Collider other);
}