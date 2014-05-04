using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadSceneObject : MonoBehaviour
{

    //这里是需要加载激活的游戏对象
    public List<GameObject> objList = new List<GameObject>();
    public int objCount;
    public bool isLoadObjComplete = false;

    //当前加载的进度
    int load_index = 0;
    void Start()
    {
        StopWatch.Split("Load Obj start");
        transform.BFSVisit<List<GameObject>>(AddObjToList, objList);
        objCount = objList.Count;

        //开启一个异步任务，加载模型。
        StartCoroutine(loadObject());
    }

    void AddObjToList(Transform trans, List<GameObject> list)
    {
        list.Add(trans.gameObject);
    }

    IEnumerator loadObject()
    {
        //遍历所有游戏对象
        foreach (GameObject obj in objList)
        {
            //激活游戏对象
            obj.active = true;
            //记录当前加载的对象
            load_index++;

            //这里可以理解为通知主线程刷新UI
            yield return 0;
        }
        objList.Clear();

        isLoadObjComplete = true;
        StopWatch.Split("Load Obj complete");
        //全部便利完毕返回
        yield return 0;
    }

    void OnGUI()
    {
        //显示加载的进度
        GUILayout.Box("当前加载的对象ID是： " + load_index + "/" + objCount);
    }
}
