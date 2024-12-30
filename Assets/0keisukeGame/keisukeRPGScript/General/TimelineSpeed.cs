using UnityEngine.Playables;
using UnityEngine;

public class TimelineController : MonoBehaviour
{
  public PlayableDirector playableDirector;
public float speed;

  public void Update()
  {
    playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(speed);
  }


}