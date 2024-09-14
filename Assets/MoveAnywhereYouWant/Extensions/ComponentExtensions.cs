using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtensions
{
	public static void GetComponents<T>(this Component a, bool includeInactive, List<T> results)
		=> a.gameObject.GetComponents<T>(includeInactive, results);
}