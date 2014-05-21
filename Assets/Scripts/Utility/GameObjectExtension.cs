using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public static class GameObjectExtension
{
    /// <summary>
    /// 实例化一个GameObject作为子GameObject
    /// </summary>
    /// <param name="prefab">if null, instantiate an empty GameObject</param>
    public static GameObject AddChild(this GameObject parent, GameObject go, string name = null)
    {
        GameObject obj;
        if (go != null)
            obj = GameObject.Instantiate(go) as GameObject;
        else
            obj = new GameObject();
        obj.transform.parent = parent.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        obj.layer = parent.layer;
        if (name != null)
            obj.name = name;
        return obj;
    }

    /// <summary>
    /// 实例化一个空GameObject作为子GameObject，并绑定指定脚本
    /// </summary>
    public static T AddChild<T>(this GameObject parent) where T : MonoBehaviour
    {
        GameObject go = parent.AddChild(null, typeof(T).ToString());
        return go.AddComponent<T>();
    }

    /// <summary>
    /// 加载一个prefab并实例化为子GameObject
    /// </summary>
    public static GameObject AddChildPrefab(this GameObject parent, string prefabPath)
    {
        GameObject prefab = Resources.Load(prefabPath) as GameObject;
        return parent.AddChild(prefab);
    }

    static void SafelyEnableDisableComponent<T>(this GameObject obj, bool isEnable) where T : Behaviour
    {
        T comp = obj.GetComponentSafely<T>();
        if (comp != null)
            comp.enabled = isEnable;
    }

    /// <summary>
    /// 不会报错地禁用Component
    /// </summary>
    public static void SafelyDisableComponent<T>(this GameObject obj) where T : Behaviour
    {
        obj.SafelyEnableDisableComponent<T>(false);
    }

    /// <summary>
    /// 不会报错地启用Component
    /// </summary>
    public static void SafelyEnableComponent<T>(this GameObject obj) where T : Behaviour
    {
        obj.SafelyEnableDisableComponent<T>(true);
    }

    /// <summary>
    /// 不会报错地获取Component
    /// </summary>
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
    /// <summary>
    /// 复制所有Component（除了Camera等）
    /// </summary>
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

    /// <summary>
    /// 删除所有后代，自身不删除
    /// </summary>
    public static void DestroyAllChildren(this GameObject parent)
    {
        parent.transform.DestroyAllChildren();
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


