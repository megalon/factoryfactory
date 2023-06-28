using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI_API;
using OpenAI_API.Completions;
using System.Threading.Tasks;
using System;
using OpenAI_API.Moderation;
using System.Text.RegularExpressions;
using OpenAI_API.Chat;

public class OpenAICompleter : MonoBehaviour
{
    private OpenAIAPI api;
    public static OpenAICompleter Instance;

    [SerializeField]
    private int _completionAttemptsMax = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        if (api == null) api = new OpenAIAPI(Auth.OpenAIAPIKey);
    }

    public async Task<string> CreateScriptCompletionAsync(ScriptSection scriptSection)
    {
        return await CreateScriptCompletionAsync(scriptSection.promptSO.text, scriptSection);
    }

    public async Task<string> CreateScriptCompletionAsync(string customPrompt, ScriptSection scriptSection)
    {
        return await CreateCompletionAsync(customPrompt, scriptSection.promptSO);
    }

    public async Task<string> CreateCompletionAsync(string promptText, PromptSO p)
    {
        int numCompletionAttempts = 0;

        do
        {
            // Adjust prompt if we fail the first attempt
            if (numCompletionAttempts == 1)
            {
                promptText = "Make sure your response is family friendly, and uses PG-13 words. " + promptText;
            }

            try
            {
                string text = await GetResultFromRequest(promptText, p);

                // Replace the bad words here 
                if (numCompletionAttempts > 0)
                {
                    text = StringUtils.ReplaceAllBannedPhrases(text);
                }

                if (await DoesTextPassModeration(text))
                {
                    Debug.Log("Result from OpenAI API:" + text);

                    return p.textToPrepend + text;
                }
                else
                {
                    throw new Exception("Text did not pass moderation check!");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Caught exception from OpenAI API in CreateCompletionAsync");

                if (e != null)
                {
                    Debug.LogError(e);
                }

                numCompletionAttempts++;

                if (numCompletionAttempts < _completionAttemptsMax)
                {
                    await Task.Delay(1000);
                }
            }
        } while (numCompletionAttempts < _completionAttemptsMax);

        throw new Exception($"Failed to get result from OpenAI API after {_completionAttemptsMax} attempts.");
    }

    private async Task<string> GetResultFromRequest(string promptText, PromptSO p)
    {
        if (p.modelName.Equals(OpenAI_API.Models.Model.ChatGPTTurbo)
            || p.modelName.Equals(OpenAI_API.Models.Model.ChatGPTTurbo0301))
        {
            ChatRequest request = new ChatRequest();
            List<ChatMessage> messages = new List<ChatMessage>();
            messages.Add(new ChatMessage(ChatMessageRole.User, promptText));
            request.Messages = messages;
            request.Model = p.modelName;
            request.Temperature = p.temperature;

            // Use fewer tokens if running in the Unity editor
#if UNITY_EDITOR
            request.MaxTokens = p.maxLength > 100 ? p.maxLength / 100 : p.maxLength;
#else
            request.MaxTokens = p.maxLength;
#endif

            ChatResult chatResult = await api.Chat.CreateChatCompletionAsync(request);
            return chatResult.Choices[0].Message.Content.Trim();
        } else
        {
            CompletionRequest request = new CompletionRequest();
            request.Prompt = promptText;
            request.Model = p.modelName;
            request.Temperature = p.temperature;
            request.MaxTokens = p.maxLength;

            CompletionResult completionResult = await api.Completions.CreateCompletionAsync(request);
            return completionResult.Completions[0].Text.Trim();
        }
    }

    public async Task<bool> DoesTextPassModeration(string text)
    {
        // Check text against the banned words list
        StringUtils.BannedPhraseCheckThrowsError(text);

        ModerationRequest moderationRequest = new ModerationRequest(text, OpenAI_API.Models.Model.TextModerationLatest);

        ModerationResult moderationResult = await api.Moderation.CallModerationAsync(moderationRequest);

        if (moderationResult.Results[0].Flagged)
        {
            throw new ModerationAPIFlaggedException(moderationResult.Results[0].ToString());
        }

        // Didn't fail any of the checks, so just return true
        return true;
    }
}
