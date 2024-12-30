using System;
using System.Collections.ObjectModel;

[Serializable]
public class ValueChangeEvent<T> where T : class
{
    public T _Value;

    // ジェネリック型の値が変更された時に呼ばれるイベント
    public event Action<T> OnValueChanged;

    // ジェネリック型のプロパティの定義
    public T Value
    {
        get { return _Value; }
        set
        {
            if (!_Value?.Equals(value) ?? value != null)
            {
                // リスト型だったらObservableCollectionにキャストしてイベントをフック
                if (_Value is ObservableCollection<T> oldList)
                {
                    oldList.CollectionChanged -= OnCollectionChanged;
                }

                _Value = value;

                // 新しいリストにもイベントをフック
                if (_Value is ObservableCollection<T> newList)
                {
                    newList.CollectionChanged += OnCollectionChanged;
                }

                // 値が変更された時にイベントを発火
                OnValueChanged?.Invoke(_Value);
            }
        }
    }

    private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        // コレクションが変更された場合の処理
        OnValueChanged?.Invoke(_Value);
    }
}
