using System.IO;
using UnityEngine;

public class ElevenLabs : MonoBehaviour
{
    public string apiKey;
    public string voiceId;
    public string ttsUrl;

    private string configFilePath;

    private void Awake()
    {
        string homeDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        configFilePath = Path.Combine(homeDirectory, ".elevenlabs", "auth.json");
        LoadConfig();
    }

    private void LoadConfig()
    {
        if (File.Exists(configFilePath))
        {
            string jsonContent = File.ReadAllText(configFilePath);

            ElevenLabsConfig config = JsonUtility.FromJson<ElevenLabsConfig>(jsonContent);

            apiKey = config.api_key;
            voiceId = config.voice_id;
            ttsUrl = config.tts_url;


            Debug.Log("ElevenLabs API Key loaded successfully.");
        }
        else
        {
            Debug.LogError("ElevenLabs auth.json not found" + configFilePath);
        }
    }
}
