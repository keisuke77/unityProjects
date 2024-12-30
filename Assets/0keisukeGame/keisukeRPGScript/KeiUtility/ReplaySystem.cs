//using System.Collections.Generic;
//using UI_Inputs.Tools;
//using UnityEngine;
//[System.Serializable]
//public struct InputData<T>
//{
//    public T inputParam;
//    public float time;
//
//    // コンストラクタ
//    public InputData(T inputParam, float time)
//    {
//        this.inputParam = inputParam;
//        this.time = time;
//    }
//  
//
//}
//
//
//public abstract class ReplaySystem<T> : MonoBehaviour where T : ReplaySystem<T>
//{
//    public List<InputData<T>> inputDatas = new List<InputData<T>>();
//    private bool isRecording,isLoading = false;
//float StartTime;
// public InputData<T> GetDataFromTime(float time){
//    InputData<T> nowdata=new InputData<T>();
//    float temp=100000;
//    foreach (var item in inputDatas)
//    {
//        float duration=item.time-time;
//       if (duration<temp)
//       {
//        nowdata=item;
//       } 
//    }
//
//return nowdata;
//    }
//
//    // 録画を開始する
//    public void StartRecording()
//    {
//        if (!isRecording)
//        {
//            isRecording = true;
//            StartTime=Time.time;
//            inputDatas.Clear(); // 録画開始時にデータをクリア
//        }
//    }
//
//    // 録画を停止する
//    public void StopRecording()
//    {
//        if (isRecording)
//        {
//            isRecording = false;
//        }
//    }
//
//    // 入力を記録する
//    private void Record(T t)
//    {
//        inputDatas.Add(new InputData<T>(t, Time.time-StartTime));
//    }
//
//    // リプレイデータを再生する
//    public IEnumerator<T> Play(T t)
//    {
//        foreach(var item in inputDatas)
//        {
//            if (t is object obj)
//            {
//                 item.inputParam.CopyAllFields(ref obj);
//                 t=obj as T;
//            }
//           yield return null; 
//        }
//     yield return null;     
//    }
//
//    // リプレイデータを読み込む
//    public void Load()
//    {
//        // 読み込みの実装をここに追加
//    }
//public T thisobj;
//    // 毎フレーム呼ばれるメソッド
//    private void Update()
//    {
//     
//    }
//}
//