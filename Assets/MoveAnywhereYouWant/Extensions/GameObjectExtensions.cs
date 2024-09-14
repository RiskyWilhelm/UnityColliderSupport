using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
	public static void GetComponents<T>(this GameObject a, bool includeInactiveComponents, List<T> results)
	{
		a.GetComponents<T>(results);

		if (!includeInactiveComponents)
			results.RemoveAll((c) => (c is Behaviour b) && !b.enabled);
	}
}