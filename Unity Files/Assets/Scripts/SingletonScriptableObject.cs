using UnityEngine;

/*
* Adapted from this tutorial series
* Singleton class used so this singleton can be accessed in multiple scenes
* and make the scenes easier to test
*/
public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //first find all references of singleton
                T[] results = Resources.FindObjectsOfTypeAll<T>();

                //if no results, then singleton isn't created
                if (results.Length == 0)
                {
                    Debug.LogError("SingletonScriptableObject: results length is 0 of " + typeof(T).ToString());
                    return null;
                }

                //if more than one result, you've created too many singletons which is an error!
                if (results.Length > 1)
                {
                    Debug.LogError("SingletonScriptableObject: results length is greater than 1 of  " + typeof(T).ToString());
                    return null;
                }

                _instance = results[0];
                //so your singleton doesn't get gobbled up by garbage collection
                _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
            }
            return _instance;
        }
    }
}