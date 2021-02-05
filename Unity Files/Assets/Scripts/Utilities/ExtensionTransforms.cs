using UnityEngine;

/*
* Used as a utility class to destroy children game objects for instantiated prefabs
* e.g player and room listings
*/
public static class Transforms
{
    public static void DestroyChildren(this Transform t, bool destroyImmediately = false)
    {
        foreach (Transform child in t)
        {
            if (destroyImmediately)
                MonoBehaviour.DestroyImmediate(child.gameObject);
            else
                MonoBehaviour.Destroy(child.gameObject);
        }
    }

}