using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using TwitchLib.Unity;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Twitchmata
{
    public class MegCommandManager : FeatureManager
    {
        protected string _overflowIndicator = "...";
        protected bool _debug = false;
        protected bool strictModeration = false;

        // Twitch max message length is 500
        private int maxMessageLength = 500;
        public void RegisterChatCommand(string command, Permissions permissions, ChatCommandCallback callback)
        {
            RegisteredCommands[command] = new RegisteredChatCommand()
            {
                Permissions = permissions,
                Callback = callback,
            };
        }

        protected void SendReplyMessage(Twitchmata.Models.User user, string text)
        {
            string textToSend = $"{user.DisplayName} {text}";

            if (textToSend.Length > maxMessageLength)
            {
                textToSend = textToSend.Substring(0, maxMessageLength - _overflowIndicator.Length) + _overflowIndicator;
            }

            if (_debug)
            {
                Debug.Log(textToSend);
            }
            else
            {
                SendChatMessage(textToSend);
            }
        }

        private Dictionary<string, RegisteredChatCommand> RegisteredCommands = new Dictionary<string, RegisteredChatCommand>();

        override internal void InitializeClient(Client client)
        {
            Logger.LogInfo("Setting up Chat Command Manager");
            client.OnChatCommandReceived -= Client_OnChatCommandReceived;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;
        }

        private void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs args)
        {
            var command = args.Command;
            if (RegisteredCommands.ContainsKey(command.CommandText) == false)
            {
                return;
            }

            var user = UserManager.UserForChatMessage(command.ChatMessage);

            var registeredCommand = RegisteredCommands[command.CommandText];

            if (user.IsPermitted(registeredCommand.Permissions) == false)
            {
                SendChatMessage("You don't have permission to use this command");
                return;
            }

            Debug.Log("TWITCH: User:" + user.DisplayName + " called command:" + command.CommandText + " with parameters:" + command.ArgumentsAsString);

            ModerateInputThenTryCommand(command, registeredCommand, user);
        }

        private async void ModerateInputThenTryCommand(ChatCommand command, RegisteredChatCommand registeredCommand, Models.User user)
        {
            if (await TryModerateCommand(command.CommandText, command.ArgumentsAsString, user))
            {
                List<string> argsAsList = new List<string>(command.ArgumentsAsString.Split(' '));
                registeredCommand.Callback.Invoke(argsAsList, user);
            }
        }

        public async Task<bool> TryModerateCommand(string commandName, string commandArgs, Models.User user)
        {
            try
            {
                if (await ModerateUserInputThrowsErrors(commandArgs, user))
                {
                    return true;
                }
            }
            catch (ModerationAPIFlaggedException)
            {
                Debug.LogWarning("TWITCH: Command failed API moderation! User:" + user.DisplayName + " called command:" + commandName + " with parameters:" + commandArgs);

                SendReplyMessage(user, "Request rejected! I can't make that!");
            }
            catch (ModerationBannedWordException)
            {
                Debug.LogWarning("TWITCH: Command contained banned word! User:" + user.DisplayName + " called command:" + commandName + " with parameters:" + commandArgs);

                SendReplyMessage(user, "Request rejected! I can't make that!");
            }
            catch
            {
                Debug.LogWarning("TWITCH: Command failed moderation for unspecified error! User:" + user.DisplayName + " called command:" + commandName + " with parameters:" + commandArgs);

                SendReplyMessage(user, "Request failed!");
            }
            return false;
        }

        private async Task<bool> ModerateUserInputThrowsErrors(string input, Models.User user)
        {
            input = Regex.Replace(input, @"[^a-zA-Z0-9\s-,'.%$]", string.Empty);

            if (await OpenAICompleter.Instance.DoesTextPassModeration(input))
            {
                // Extra moderation step for user input, if not a mod
                if (strictModeration && !user.IsModerator && !user.IsBroadcaster)
                {
                    input = StringUtils.Sanitize(input);
                    input = StringUtils.StrictSanitizeText(input, true);

                    StringUtils.StrictModerationCheckThrowsError(input);
                }
                return true;
            }
            return false;
        }
    }
}

