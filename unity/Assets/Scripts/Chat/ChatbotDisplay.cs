using TMPro;
using UnityEngine;

namespace Chat
{
    public class ChatbotDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        private loadJSON parser;

        private void Start()
        {
            parser = GetComponent<loadJSON>();
            parser.OnChange.AddListener(DisplayElizaResponse);
        }

        private void DisplayElizaResponse()
        {
            Debug.Log($"FOO {parser.ChatbotResponse}");
            label.text = parser.ChatbotResponse;
        }
    }
}