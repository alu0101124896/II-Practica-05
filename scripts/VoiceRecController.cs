using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceRecController : MonoBehaviour {
  public delegate void OnClickDelegate();
  public static event OnClickDelegate OnClickStartKwRecEvent;
  public static event OnClickDelegate OnClickStopKwRecEvent;
  public static event OnClickDelegate OnClickStartDictRecEvent;
  public static event OnClickDelegate OnClickStopDictRecEvent;

  void Start() {}

  void Update() {}

  void OnGUI() {
    if (GUILayout.Button("Start Keyword Recognition")) {
      OnClickStartKwRecEvent();
    }
    if (GUILayout.Button("Stop Keyword Recognition")) {
      OnClickStopKwRecEvent();
    }
    if (GUILayout.Button("Start Dictation Recognition")) {
      OnClickStartDictRecEvent();
    }
    if (GUILayout.Button("Stop Dictation Recognition")) {
      OnClickStopDictRecEvent();
    }
  }
}
