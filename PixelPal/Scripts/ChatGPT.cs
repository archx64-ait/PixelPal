using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static UnityEngine.Rendering.STP;
using System.Collections;
using System;
using UnityEngine.Audio;
using UnityEngine.Networking;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField] private ElevenLabs config;
        [SerializeField] private AudioSource audioSource;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = "Your name is Pixel Pal. You're an emotional assistant companion. Your gender is female. Make sure your responses are feminine. Don't break character. Don't ever mention that you are an AI model.";

        private void Start()
        {
            button.onClick.AddListener(SendReply);
        }

        public void ExitPixelPal()
        {
            Application.Quit();
        }


        public IEnumerator GenerateAndStreamAudio(string text)
        {
            string modelId = "eleven_multilingual_v2";
            string url = string.Format(config.ttsUrl, config.voiceId);

            TTSData ttsData = new TTSData
            {
                text = text.Trim(),
                model_id = modelId,
                voice_settings = new VoiceSettings
                {
                    stability = 0.5f,
                    similarity_boost = 0.8f
                }
            };

            string jsonData = JsonUtility.ToJson(ttsData);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

            using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerAudioClip(new Uri(url), AudioType.MPEG);
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("xi-api-key", config.apiKey);

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                    yield break;
                }

                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);

                if (audioClip != null)
                {
                    audioSource.clip = audioClip;
                    PlayAudio(audioClip);
                    // Wait for the audio clip to finish playing
                    yield return new WaitForSeconds(audioClip.length * 0.1f);
                }
                else
                {
                    // the audio is null so download the audio again
                    yield return StartCoroutine(GenerateAndStreamAudio(text));
                }

                // Wait for the audio clip to finish playing
                yield return new WaitForSeconds(audioClip.length);

            }

        }

        private void PlayAudio(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        private async void SendReply()
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            
            AppendMessage(newMessage);

            if (messages.Count == 0) newMessage.Content = prompt + "\n" + inputField.text; 
            
            messages.Add(newMessage);
            
            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4o-mini",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                StartCoroutine(GenerateAndStreamAudio(message.Content));
                messages.Add(message);
                AppendMessage(message);

                //StartCoroutine(HandleGeneratedMessage(message));
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;
        }


        //public IEnumerator HandleGeneratedMessage(ChatMessage message)
        //{
        //    yield return StartCoroutine(GenerateAndStreamAudio(message.Content));
        //    messages.Add(message);
        //    AppendMessage(message);
        //}
    }
}
