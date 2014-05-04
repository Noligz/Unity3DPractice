using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public static class GameObjectExtension
{
    static void SafelyEnableDisableComponent<T>(this GameObject obj, bool isEnable) where T : Behaviour
    {
        T comp = obj.GetComponentSafely<T>();
        if (comp != null)
            comp.enabled = isEnable;
    }

    public static void SafelyDisableComponent<T>(this GameObject obj) where T : Behaviour
    {
        obj.SafelyEnableDisableComponent<T>(false);
    }

    public static void SafelyEnableComponent<T>(this GameObject obj) where T : Behaviour
    {
        obj.SafelyEnableDisableComponent<T>(true);
    }

    public static T GetComponentSafely<T>(this GameObject obj) where T : Behaviour
    {
        if (obj != null)
            return obj.GetComponent<T>();
        else
            return null;
    }

    static void CopyComponent(this GameObject obj, Component from)
    {
        if (obj == null || from == null)
            return;
        Component new_component = obj.AddComponent(from.GetType());
        foreach (FieldInfo f in from.GetType().GetFields())
        {
            f.SetValue(new_component, f.GetValue(from));
        }
        (new_component as Behaviour).enabled = (from as Behaviour).enabled;
    }
    public static void CopyAllComponent(this GameObject obj, GameObject from)
    {
        if (obj == null || from == null)
            return;
        System.Type[] excludeType = { typeof(Transform), typeof(Camera), typeof(GUILayer), typeof(AudioListener), typeof(Behaviour) };
        foreach (Component comp in from.GetComponents<Component>())
        {
            bool isCopy = true;
            foreach (System.Type type in excludeType)
            {
                if (comp != null && comp.GetType() == type)
                {
                    isCopy = false;
                    break;
                }

            }
            if (isCopy)
                obj.CopyComponent(comp);
        }
    }

    public static bool GetBoundingBox(this GameObject obj, out Bounds boundingBox)
    {
        boundingBox = new Bounds();

        List<Collider> colliders = new List<Collider>();

        Queue<Transform> children = new Queue<Transform>();
        children.Enqueue(obj.transform);

        while (children.Count > 0)
        {
            Transform child = children.Dequeue();

            if (child.gameObject.collider != null) colliders.Add(child.gameObject.collider);

            foreach (Transform c in child)
            {
                children.Enqueue(c);
            }
        }

        if (colliders.Count > 0)
        {
            var min = Vector3.one * Mathf.Infinity;
            var max = Vector3.one * Mathf.NegativeInfinity;
            foreach (Collider collider in colliders)
            {
                var box = collider.bounds; // or use obj.collider.bounds
                min = Vector3.Min(min, box.min); // expand min to encapsulate bounds.min
                max = Vector3.Max(max, box.max); // expand max to encapsulate bounds.max
            }
            var center = (min + max) / 2.0f;

            Bounds bounds = new Bounds(center, new Vector3(0.01f, 0.01f, 0.01f));

            for (int i = 0; i < colliders.Count; i++)
                bounds.Encapsulate(colliders[i].bounds);

            boundingBox = bounds;
            return true;
        }

        return false;
    }

}


