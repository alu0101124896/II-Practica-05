using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class DictationScript : MonoBehaviour {
  [SerializeField]
  private Text m_Hypotheses;

  // [SerializeField]
  // private Text m_Recognitions;

  private DictationRecognizer m_DictationRecognizer;

  void Start() {
    VoiceRecController.OnClickStartDictRecEvent += dictationStart;
    VoiceRecController.OnClickStopDictRecEvent += dictationStop;
    // this.dictationStart();
  }

  void dictationStart() {
    m_DictationRecognizer = new DictationRecognizer();

    m_DictationRecognizer.DictationResult += (text, confidence) => {
      Debug.LogFormat("Dictation result: {0}", text);
      // m_Recognitions.text += text + "\n";
    };

    m_DictationRecognizer.DictationHypothesis += (text) => {
      Debug.LogFormat("Dictation hypothesis: {0}", text);
      m_Hypotheses.text = text;
    };

    m_DictationRecognizer.DictationComplete += (completionCause) => {
      if (completionCause != DictationCompletionCause.Complete)
        Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
    };

    m_DictationRecognizer.DictationError += (error, hresult) => {
      Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
    };

    // PhraseRecognitionSystem.Shutdown();
    m_DictationRecognizer.Start();
  }

  void dictationStop() {
    m_DictationRecognizer.Dispose();
    m_DictationRecognizer = null;
  }

  void OnDisable() {
    VoiceRecController.OnClickStartDictRecEvent -= dictationStart;
    VoiceRecController.OnClickStopDictRecEvent -= dictationStop;
    this.dictationStop();
  }
}
