using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public ItemData itemData;  // ScriptableObjectを参照

    public Text nameText;  // 名前を表示するText
    public Text descriptionText;  // 説明を表示するText
    public Image iconImage;  // アイコンを表示するImage
    public Text priceText;  // 値段を表示するText
    public Text stocknumText;  // 在庫数を表示するText

// アイテムデータを設定し、UIを更新する
    public void ToSetItem(ItemDisplay ItemDisplay)
    {
        ItemDisplay.SetItem(this.itemData);
    }
    // アイテムデータを設定し、UIを更新する
    public void SetItem(ItemData newItemData)
    {
        if (newItemData == null)
        {
            Debug.LogWarning("ItemData is null. Cannot set item data.", this);
            if (iconImage!=null)
            {
                iconImage.color =new Color(0,0,0,0);
            }
            return;
        }   

        itemData = newItemData;

        if (stocknumText != null)
            stocknumText.text = $"{itemData.stocknum}コ";
        else
            Debug.LogWarning("StockNumText is not assigned in the inspector.", this);

        if (nameText != null)
            nameText.text = itemData.itemName;
        else
            Debug.LogWarning("NameText is not assigned in the inspector.", this);

        if (descriptionText != null)
            descriptionText.text = itemData.description;
        else
            Debug.LogWarning("DescriptionText is not assigned in the inspector.", this);

        if (iconImage != null){
            iconImage.color =new Color(255,255,255,1);
            iconImage.sprite = itemData.icon;
        }
            
        else
            Debug.LogWarning("IconImage is not assigned in the inspector.", this);

        if (priceText != null)
            priceText.text = $"Price: {itemData.price}";
        else
            Debug.LogWarning("PriceText is not assigned in the inspector.", this);
    }

    // Start関数は初期化時に呼び出されるデフォルトの動作
    void Awake()
    {
   
            SetItem(itemData);
        
    }
}
