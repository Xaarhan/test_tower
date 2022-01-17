
using System.Collections.Generic;
using UnityEngine;


public class ResMan {


	public ResMan () {
		prefabs = new Dictionary<string, GameObject>();
	
	}


	public static void registerPrefab(string name, GameObject prefab ) {
		prefabs.Add (name, prefab);
	}


	public static UnityEngine.Object getPrefub(string name) {
		if ( prefabs.ContainsKey(name) == false) {
             UnityEngine.Object prefub = Resources.Load(name);
             if ( prefub == null)   {
				  Debug.Log("model_not_found " + name);
                return null;
			 }
			 return prefub ;
		} else {
			 return prefabs[name];

		}
	}
		

	

	public static bool havePrefub(string name) {
		return prefabs.ContainsKey(name);
	}


	public static Dictionary<string, GameObject> prefabs;  



}


