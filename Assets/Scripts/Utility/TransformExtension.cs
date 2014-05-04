using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TransformExtension
{
    /// <summary>
    /// 广度优先搜索遍历
    /// </summary>
    /// <typeparam name="TP">遍历时调用的函数的参数的类型</typeparam>
    /// <typeparam name="TR">遍历时调用的函数的返回值的类型</typeparam>
    /// <param name="visitFunc">遍历时调用的函数
    /// <para>TR Function(Transform t, T para)</para>
    /// </param>
    /// <param name="para">遍历时调用的函数的第二个参数</param>
    /// <param name="failReturnValue">遍历时查找失败的返回值</param>
    /// <returns>遍历时调用的函数的返回值</returns>
    public static TR BFSVisit<TP, TR>(this Transform root,
        System.Func<Transform, TP, TR> visitFunc,
        TP para,
        TR failReturnValue = default(TR))
    {
        TR ret = visitFunc(root, para);
        if (ret != null && !ret.Equals(failReturnValue))
            return ret;
        Queue<Transform> parents = new Queue<Transform>();
        parents.Enqueue(root);
        while (parents.Count > 0)
        {
            Transform parent = parents.Dequeue();
            foreach (Transform child in parent)
            {
                ret = visitFunc(child, para);
                if (ret != null && !ret.Equals(failReturnValue))
                    return ret;
                parents.Enqueue(child);
            }
        }
        return failReturnValue;
    }

    /// <summary>
    /// 深度优先搜索遍历
    /// </summary>
    /// <typeparam name="TP">遍历时调用的函数的参数的类型</typeparam>
    /// <typeparam name="TR">遍历时调用的函数的返回值的类型</typeparam>
    /// <param name="visitFunc">遍历时调用的函数
    /// <para>TR Function(Transform t, T para)</para>
    /// </param>
    /// <param name="para">遍历时调用的函数的第二个参数</param>
    /// <param name="failReturnValue">遍历时查找失败的返回值</param>
    /// <returns>遍历时调用的函数的返回值</returns>
    public static TR DFSVisit<TP, TR>(this Transform root,
        System.Func<Transform, TP, TR> visitFunc,
        TP para,
        TR failReturnValue = default(TR))
    {
        Stack<Transform> parents = new Stack<Transform>();
        parents.Push(root);
        while (parents.Count > 0)
        {
            Transform parent = parents.Pop();
            TR ret = visitFunc(parent, para);
            if (ret != null && !ret.Equals(failReturnValue))
                return ret;
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                parents.Push(parent.GetChild(i));
            }
        }
        return failReturnValue;
    }

    /// <summary>
    /// 广度优先搜索遍历
    /// </summary>
    /// <typeparam name="T">遍历时调用的函数的参数的类型</typeparam>
    /// <param name="action">遍历时调用的函数
    /// <para>void Function(Transform t, T para)</para>
    /// </param>
    /// <param name="para">遍历时调用的函数的第二个参数</param>
    public static void BFSVisit<T>(this Transform root, System.Action<Transform, T> action, T para)
    {
        BFSVisit<T, bool>(root,
            (t, p) => { action(t, p); return false; },
            para);
    }

    /// <summary>
    /// 深度优先搜索遍历
    /// </summary>
    /// <typeparam name="T">遍历时调用的函数的参数的类型</typeparam>
    /// <param name="action">遍历时调用的函数
    /// <para>void Function(Transform t, T para)</para>
    /// </param>
    /// <param name="para">遍历时调用的函数的第二个参数</param>
    public static void DFSVisit<T>(this Transform root, System.Action<Transform, T> action, T para)
    {
        DFSVisit<T, bool>(root,
            (t, p) => { action(t, p); return false; },
            para);
    }

    /// <summary>
    /// 根据名字查找并返回子孙，广度优先搜索
    /// </summary>
    /// <param name="childName">要查找的子孙的名字</param>
    /// <returns>要查找的子孙的Transform</returns>
    public static Transform FindDescendant_BFS(this Transform root, string childName)
    {
        return BFSVisit<string, Transform>(root,
            (t, str) => { if (t.name.Equals(str)) return t; return null; },
            childName
            );
    }

    /// <summary>
    /// 根据名字查找并返回子孙，深度优先搜索
    /// </summary>
    /// <param name="childName">要查找的子孙的名字</param>
    /// <returns>要查找的子孙的Transform</returns>
    public static Transform FindDescendant_DFS(this Transform root, string childName)
    {
        return DFSVisit<string, Transform>(root,
            (t, str) => { if (t.name.Equals(str)) return t; return null; },
            childName
            );
    }

    /// <summary>
    /// 判断当前Transform是否为某一Transform的子孙，广度优先搜索
    /// </summary>
    public static bool IsDescendantOf_BFS(this Transform obj, Transform root)
    {
        return BFSVisit<Transform, bool>(root,
            (x, y) => { if (x.Equals(y)) return true; return false; },
            obj
            );
    }

    /// <summary>
    /// 判断当前Transform是否为某一Transform的子孙，深度优先搜索
    /// </summary>
    public static bool IsDescendantOf_DFS(this Transform obj, Transform root)
    {
        return DFSVisit<Transform, bool>(root,
            (x, y) => { if (x.Equals(y)) return true; return false; },
            obj
            );
    }

    /// <summary>
    /// 删除所有后代，自身不删除
    /// </summary>
    public static void DestroyAllChildren(this Transform parent)
    {
        foreach (Transform child in parent)
            Object.Destroy(child.gameObject);
    }
}
