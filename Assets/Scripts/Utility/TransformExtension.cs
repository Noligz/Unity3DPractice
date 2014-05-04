using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TransformExtension
{
    /// <summary>
    /// ���������������
    /// </summary>
    /// <typeparam name="TP">����ʱ���õĺ����Ĳ���������</typeparam>
    /// <typeparam name="TR">����ʱ���õĺ����ķ���ֵ������</typeparam>
    /// <param name="visitFunc">����ʱ���õĺ���
    /// <para>TR Function(Transform t, T para)</para>
    /// </param>
    /// <param name="para">����ʱ���õĺ����ĵڶ�������</param>
    /// <param name="failReturnValue">����ʱ����ʧ�ܵķ���ֵ</param>
    /// <returns>����ʱ���õĺ����ķ���ֵ</returns>
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
    /// ���������������
    /// </summary>
    /// <typeparam name="TP">����ʱ���õĺ����Ĳ���������</typeparam>
    /// <typeparam name="TR">����ʱ���õĺ����ķ���ֵ������</typeparam>
    /// <param name="visitFunc">����ʱ���õĺ���
    /// <para>TR Function(Transform t, T para)</para>
    /// </param>
    /// <param name="para">����ʱ���õĺ����ĵڶ�������</param>
    /// <param name="failReturnValue">����ʱ����ʧ�ܵķ���ֵ</param>
    /// <returns>����ʱ���õĺ����ķ���ֵ</returns>
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
    /// ���������������
    /// </summary>
    /// <typeparam name="T">����ʱ���õĺ����Ĳ���������</typeparam>
    /// <param name="action">����ʱ���õĺ���
    /// <para>void Function(Transform t, T para)</para>
    /// </param>
    /// <param name="para">����ʱ���õĺ����ĵڶ�������</param>
    public static void BFSVisit<T>(this Transform root, System.Action<Transform, T> action, T para)
    {
        BFSVisit<T, bool>(root,
            (t, p) => { action(t, p); return false; },
            para);
    }

    /// <summary>
    /// ���������������
    /// </summary>
    /// <typeparam name="T">����ʱ���õĺ����Ĳ���������</typeparam>
    /// <param name="action">����ʱ���õĺ���
    /// <para>void Function(Transform t, T para)</para>
    /// </param>
    /// <param name="para">����ʱ���õĺ����ĵڶ�������</param>
    public static void DFSVisit<T>(this Transform root, System.Action<Transform, T> action, T para)
    {
        DFSVisit<T, bool>(root,
            (t, p) => { action(t, p); return false; },
            para);
    }

    /// <summary>
    /// �������ֲ��Ҳ�������������������
    /// </summary>
    /// <param name="childName">Ҫ���ҵ����������</param>
    /// <returns>Ҫ���ҵ������Transform</returns>
    public static Transform FindDescendant_BFS(this Transform root, string childName)
    {
        return BFSVisit<string, Transform>(root,
            (t, str) => { if (t.name.Equals(str)) return t; return null; },
            childName
            );
    }

    /// <summary>
    /// �������ֲ��Ҳ�������������������
    /// </summary>
    /// <param name="childName">Ҫ���ҵ����������</param>
    /// <returns>Ҫ���ҵ������Transform</returns>
    public static Transform FindDescendant_DFS(this Transform root, string childName)
    {
        return DFSVisit<string, Transform>(root,
            (t, str) => { if (t.name.Equals(str)) return t; return null; },
            childName
            );
    }

    /// <summary>
    /// �жϵ�ǰTransform�Ƿ�ΪĳһTransform����������������
    /// </summary>
    public static bool IsDescendantOf_BFS(this Transform obj, Transform root)
    {
        return BFSVisit<Transform, bool>(root,
            (x, y) => { if (x.Equals(y)) return true; return false; },
            obj
            );
    }

    /// <summary>
    /// �жϵ�ǰTransform�Ƿ�ΪĳһTransform����������������
    /// </summary>
    public static bool IsDescendantOf_DFS(this Transform obj, Transform root)
    {
        return DFSVisit<Transform, bool>(root,
            (x, y) => { if (x.Equals(y)) return true; return false; },
            obj
            );
    }

    /// <summary>
    /// ɾ�����к��������ɾ��
    /// </summary>
    public static void DestroyAllChildren(this Transform parent)
    {
        foreach (Transform child in parent)
            Object.Destroy(child.gameObject);
    }
}
