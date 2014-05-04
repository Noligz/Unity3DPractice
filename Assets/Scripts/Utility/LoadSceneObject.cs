using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadSceneObject : MonoBehaviour
{

    //��������Ҫ���ؼ������Ϸ����
    public List<GameObject> objList = new List<GameObject>();
    public int objCount;
    public bool isLoadObjComplete = false;

    //��ǰ���صĽ���
    int load_index = 0;
    void Start()
    {
        StopWatch.Split("Load Obj start");
        transform.BFSVisit<List<GameObject>>(AddObjToList, objList);
        objCount = objList.Count;

        //����һ���첽���񣬼���ģ�͡�
        StartCoroutine(loadObject());
    }

    void AddObjToList(Transform trans, List<GameObject> list)
    {
        list.Add(trans.gameObject);
    }

    IEnumerator loadObject()
    {
        //����������Ϸ����
        foreach (GameObject obj in objList)
        {
            //������Ϸ����
            obj.active = true;
            //��¼��ǰ���صĶ���
            load_index++;

            //����������Ϊ֪ͨ���߳�ˢ��UI
            yield return 0;
        }
        objList.Clear();

        isLoadObjComplete = true;
        StopWatch.Split("Load Obj complete");
        //ȫ��������Ϸ���
        yield return 0;
    }

    void OnGUI()
    {
        //��ʾ���صĽ���
        GUILayout.Box("��ǰ���صĶ���ID�ǣ� " + load_index + "/" + objCount);
    }
}
