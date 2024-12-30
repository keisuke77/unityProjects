using UnityEngine;using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Cell
{
    public bool dead;
    public int nearCellCount;
    public Renderer Renderer;
    public Vector2 pos;

   public Cell(bool dead,Vector2 pos){
    this.pos=pos;
    this.dead=dead;
   }
   public void update(){
  
if (dead)
{
 if (nearCellCount==3)
 {
    dead=false;
 }   
}else
{
    if (nearCellCount<=1||nearCellCount>=4)
    {
        dead=true;
    }
}

  draw();

 }
   public void draw(){

    Renderer.enabled=!dead;
    
   }
}

public class lifeGame : MonoBehaviour {
    public Text text;
    public List<Cell> cells;
    public GameObject obj;
    public float interval=0.4f;
    public int cellCount=5;
    public int age;
    float route2;
    private void Start() {
        cells = new List<Cell>();
         route2=Mathf.Sqrt(2);
        // 5x5のセルを作成し、cellsに追加します
        for (int i = 0; i < cellCount; i++)
        {
             for (int j = 0; j < cellCount; j++)
            {
               Cell cell=new Cell(Random.Range(0, 2)==1?true:false,new Vector2(i,j));
             var Instanse= Instantiate(obj,cell.pos,Quaternion.identity);
                  cell.Renderer=Instanse.GetComponent<Renderer>();
      
                 cells.Add(cell);
                 
                     }
            
        }     
        // 開始から interval 秒後に、指定した関数を繰り返し呼び出す
        InvokeRepeating("Updates", interval, interval);
 
        }


        void Updates() {
            age++;
            text.text=age+"世代";

         foreach (Cell cell1 in cells)
{ 
    int nearCellCount = 0;  

    foreach (Cell cell2 in cells)
    {
        if (cell1 == cell2) continue; // 同じセルはスキップ

        float distance = Vector2.Distance(cell1.pos, cell2.pos);
        if (distance <= route2)
        {
            if (!cell2.dead)
            {
                nearCellCount++;
            }
        }
    }

    cell1.nearCellCount = nearCellCount;
  
}
 foreach (Cell cell1 in cells)
{ 
  cell1.update();
}

            }  

        }
       
