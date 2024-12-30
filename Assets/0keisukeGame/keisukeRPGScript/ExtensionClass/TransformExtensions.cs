using UnityEngine;
 public enum DiretionEnum
    {
        No,forward,backward,up,down,right,left
    }
public static class TransformExtensions
{

    public static Vector3 CameraDirection(this Transform tra,Camera cam,Vector2 vec){

     var cameravelc = Quaternion.AngleAxis(cam.transform.eulerAngles.y, Vector3.up);
       return cameravelc * new Vector3(vec.x, 0, vec.y).normalized;
     
    }
       public static Vector3 GetDirection(this Transform tra, DiretionEnum dir)
    {
       switch (dir)
       {
        case DiretionEnum.forward:
        return tra.forward;
            break;
            break; case DiretionEnum.backward:
        return -tra.forward;
            break;
            break; case DiretionEnum.up:
        return tra.up;
            break;
            break; case DiretionEnum.down:
        return -tra.up;
            break;

         case DiretionEnum.right:
        return tra.right; 
            break;
        case DiretionEnum.left:
        return -tra.right;
          
            break;
        default:
            break;
       }
       return Vector3.zero;
    }
    public static void SetPos(this Transform transform, float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }
    public static Vector3 RotateAround(this Vector3 point, Vector3 center, float angle)
    {
        // 度をラジアンに変換
        float radian = angle * Mathf.Deg2Rad;

        // 回転前の位置から中心へのベクトルを計算
        Vector3 offset = point - center;

        // 回転行列を使用してベクトルを回転
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);
        float rotatedX = cos * offset.x - sin * offset.z;
        float rotatedZ = sin * offset.x + cos * offset.z;

        // 回転後の位置を計算
        Vector3 rotatedPosition = new Vector3(rotatedX, offset.y, rotatedZ) + center;

        // Transformの位置を更新
        return rotatedPosition;
    }
     public static bool IsPlayerInView(this Camera cam, Transform player)
    {
        // プレイヤーの位置をスクリーン座標に変換
        Vector3 screenPoint = cam.WorldToViewportPoint(player.position);

        // スクリーン座標が視界内にあるかどうかをチェック
        bool isInView = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        return isInView;
    }
    public static void AddPos(this Transform transform, float x, float y, float z)
    {
        transform.position = new Vector3(transform.position.x + x,
            transform.position.y + y, transform.position.z + z);
    }    public static void AddPos(this Transform transform, Vector3 vec)
    {
        transform.position = transform.position + vec;
    }

    public static void SetPosX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public static void SetPosY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public static void SetPosZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
  public static void AddLocalPosX(this Transform transform, float x)
    {
        transform.localPosition = new Vector3(transform.localPosition.x + x, transform.localPosition.y, transform.localPosition.z);
    }

    public static void AddLocalPosY(this Transform transform, float y)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + y, transform.localPosition.z);
    }

    public static void AddLocalPosZ(this Transform transform, float z)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + z);
    }
    public static void AddPosX(this Transform transform, float x)
    {
        transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);
    }

    public static void AddPosY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + y, transform.position.z);
    }

    public static void AddPosZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + z);
    }
}