using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace UnityStandardAssets.Characters.ThirdPerson
{
  [RequireComponent(typeof (ThirdPersonCharacter))]
  public class KeywordScript : MonoBehaviour {
    [SerializeField]
    private string[] m_Keywords;

    private KeywordRecognizer m_Recognizer;

    private ThirdPersonCharacter m_Character;
    private Transform m_Cam;
    private Vector3 m_CamForward, m_Move;
    private float m_xAxis, m_yAxis;
    private bool m_Jump;

    void Start() {
      VoiceRecController.OnClickStartKwRecEvent += keywordStart;
      VoiceRecController.OnClickStopKwRecEvent += keywordStop;

      m_Keywords = new string[] {
        "Corre",
        "Camina",
        "Para",
        "Izquierda",
        "Adelante",
        "Derecha",
        "Salta"
      };

      m_Character = GetComponent<ThirdPersonCharacter>();
      m_Cam = Camera.main.transform;
      m_xAxis = 0.0f;
      m_yAxis = 0.0f;
      m_Jump = false;

      this.keywordStart();
    }

    void keywordStart() {
      m_Recognizer = new KeywordRecognizer(m_Keywords);
      m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
      m_Recognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args) {
      StringBuilder builder = new StringBuilder();
      builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
      builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
      builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
      Debug.Log(builder.ToString());

      switch(args.text){
        case "Corre":
          m_xAxis = 1.0f;
          break;

        case "Camina":
          m_xAxis = 0.5f;
          break;

        case "Para":
          m_xAxis = 0.0f;
          m_yAxis = 0.0f;
          break;

        case "Izquierda":
          m_yAxis = -0.1f;
          break;

        case "Adelante":
          m_yAxis = 0.0f;
          break;

        case "Derecha":
          m_yAxis = 0.1f;
          break;

        case "Salta":
          m_Jump = true;
          break;

        default:
          break;
      }
    }

    void FixedUpdate() {
      m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
      m_Move = m_xAxis * m_CamForward + m_yAxis * m_Cam.right;
      m_Character.Move(m_Move, false, m_Jump);
      m_Jump = false;
    }

    void keywordStop() {
      m_Recognizer.Dispose();
      m_Recognizer = null;
      PhraseRecognitionSystem.Shutdown();
    }

    void OnDisable() {
      VoiceRecController.OnClickStartKwRecEvent -= keywordStart;
      VoiceRecController.OnClickStopKwRecEvent -= keywordStop;
      this.keywordStop();
    }
  }
}
