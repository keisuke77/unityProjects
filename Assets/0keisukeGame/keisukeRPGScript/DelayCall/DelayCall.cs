using UnityEngine;
using System.Threading.Tasks;
using System;
public static class DelayCall {

    
public static void delaycall(this float delay, System.Action action){
  
Task.Run(async () => {
  await Task.Delay((int)(delay*1000));
 action.Invoke();
});

}

}


