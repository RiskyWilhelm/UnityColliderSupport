using UnityEngine;

public sealed partial class TestTriggerListener : MonoBehaviour, ITriggerExitDisabledListener
{
	// Update
	private void OnTriggerEnter(Collider other)
	{
		Debug.LogFormat(this, "{0} has taken OnTriggerEnter of {1}", this.gameObject.name, other.gameObject.name);
	}

	private void OnTriggerStay(Collider other)
	{
		Debug.LogFormat(this, "{0} has taken OnTriggerStay of {1}", this.gameObject.name, other.gameObject.name);
	}

	private void OnTriggerExit(Collider other)
	{
		Debug.LogFormat(this, "{0} has taken OnTriggerExit of {1}", this.gameObject.name, other.gameObject.name);
	}

	public void OnTriggerExitDisabled(Collider other)
	{
		Debug.LogFormat(this, "{0} has taken OnTriggerExitDisabled of {1}", this.gameObject.name, other.gameObject.name);
	}
}


#if UNITY_EDITOR

public sealed partial class TestTriggerListener
{ }


#endif
