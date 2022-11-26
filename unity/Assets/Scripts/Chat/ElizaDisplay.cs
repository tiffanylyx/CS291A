using TMPro;
using UnityEngine;

namespace Chat
{
    public class ElizaDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        private loadJSON parser;

        public void Start()
        {
            parser = GetComponent<loadJSON>();
            parser.OnChange.AddListener(DisplayElizaResponse);
        }

        private void DisplayElizaResponse()
        {
            var msg = label == null ? "null" : label.ToString();
            Debug.Log($"FOO {label}");
            label.text = parser.ElizaResponse;
        }
    }
}