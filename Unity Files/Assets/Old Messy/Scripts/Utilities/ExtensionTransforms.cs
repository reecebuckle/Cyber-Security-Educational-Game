using UnityEngine;

/*
* Used as a utility class to destroy children game objects for instantiated prefabs
* e.g player and room listings
*/
namespace Utilities
{
    public static class Transforms
    {
        public static void DestroyChildren(this Transform t, bool destroyImmediately = false)
        {
            foreach (Transform child in t)
            {
                if (destroyImmediately)
                    Object.DestroyImmediate(child.gameObject); //Rider corrected GameObject to Object
                else
                    Object.Destroy(child.gameObject);
            }
        }
    }
}