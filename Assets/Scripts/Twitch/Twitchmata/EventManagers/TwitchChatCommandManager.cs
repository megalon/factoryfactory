using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twitchmata;
using TwitchLib.Unity;
using TwitchLib.Client.Events;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class TwitchChatCommandManager : MegCommandManager
{
    public static TwitchChatCommandManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public override void InitializeFeatureManager()
    {
        // Mod commands
        RegisterChatCommand("skip", Permissions.Mods, SkipCommand);

        RegisterChatCommand("open", Permissions.Mods, OpenQueueCommand);

        RegisterChatCommand("close", Permissions.Mods, CloseQueueCommand);

        RegisterChatCommand("clear", Permissions.Mods, ClearQueueCommand);

        RegisterChatCommand("strict", Permissions.Mods, StrictCommand);

        RegisterChatCommand("queuesize", Permissions.Mods, QueueSizeCommand);

        RegisterChatCommand("peruser", Permissions.Mods, RequestsPerUserCommand);

        RegisterChatCommand("timeout", Permissions.Mods, SetTimeoutCommand);

        RegisterChatCommand("steps", Permissions.Mods, StepsCommand);

        RegisterChatCommand("voice", Permissions.Mods, VoiceChangeCommand);

        RegisterChatCommand("restart", Permissions.Mods, RestartCommand);

        // Everyone commands
        RegisterChatCommand("queue", Permissions.Everyone, QueueCommand);
        RegisterChatCommand("q", Permissions.Everyone, QueueCommand);

        RegisterChatCommand("request", Permissions.Everyone, ProductRequestCommand);
        RegisterChatCommand("req", Permissions.Everyone, ProductRequestCommand);
        RegisterChatCommand("r", Permissions.Everyone, ProductRequestCommand);

        RegisterChatCommand("info", Permissions.Everyone, InfoCommand);
        RegisterChatCommand("i", Permissions.Everyone, InfoCommand);

        RegisterChatCommand("help", Permissions.Everyone, HelpCommand);
        RegisterChatCommand("h", Permissions.Everyone, HelpCommand);

        RegisterChatCommand("remove", Permissions.Everyone, RemoveItemCommand);
        RegisterChatCommand("x", Permissions.Everyone, RemoveItemCommand);
    }

    public void TestMessages()
    {
        _debug = true;
        _ = TestDebug();
    }

    private async Task TestDebug()
    {
        Twitchmata.Models.User user = new Twitchmata.Models.User("12345", "TEST", "TESTNAME");
        List<string> emptyArgs = new List<string>() { };

        OpenQueueCommand(emptyArgs, user);
        await Task.Delay(100);
        ProductRequestCommand(new List<string> { "test1" }, user);
        await Task.Delay(100);
        ProductRequestCommand(new List<string> { "test2" }, user);
        await Task.Delay(100);
        ProductRequestCommand(new List<string> { "test3" }, user);
        await Task.Delay(100);
        QueueCommand(emptyArgs, user);
        await Task.Delay(100);
        RemoveItemCommand(new List<string> { "3" }, user);
        await Task.Delay(100);
        QueueCommand(emptyArgs, user);
        await Task.Delay(100);
        RemoveItemCommand(new List<string> { "1" }, user);
        await Task.Delay(100);
        QueueCommand(emptyArgs, user);
        await Task.Delay(100);
        RemoveItemCommand(new List<string> { "1" }, user);
        await Task.Delay(100);
        QueueCommand(emptyArgs, user);
        await Task.Delay(100);
        RemoveItemCommand(new List<string> { "1" }, user);
        await Task.Delay(100);
        QueueCommand(emptyArgs, user);
        await Task.Delay(100);

        ProductRequestCommand(new List<string> { "test1" }, user);
        await Task.Delay(100);
        ProductRequestCommand(new List<string> { "test2" }, user);
        await Task.Delay(100);
        ProductRequestCommand(new List<string> { "test3" }, user);
        await Task.Delay(100);
        ClearQueueCommand(emptyArgs, user);
        await Task.Delay(100);

        OpenQueueCommand(emptyArgs, user);
        await Task.Delay(100);
        ProductRequestCommand(new List<string> { "Testing bass" }, user);
        await Task.Delay(100);
        StrictCommand(emptyArgs, user);
        await Task.Delay(100);
        ProductRequestCommand(new List<string> { "Testing bass after strict" }, user);
        await Task.Delay(100);
        ProductRequestCommand(new List<string> { "What the fuckingfuck is this" }, user);
        await Task.Delay(100);
        StrictCommand(new List<string> { "off" }, user);
        await Task.Delay(100);
        ProductRequestCommand(new List<string> { "After strict: What the fuckingfuck is this?" }, user);
        await Task.Delay(100);

        QueueSizeCommand(new List<string> { "3" }, user);
        await Task.Delay(100);
        QueueSizeCommand(emptyArgs, user);
        await Task.Delay(100);
        QueueSizeCommand(new List<string> { "alsdkfj" }, user);
        await Task.Delay(100);
        QueueSizeCommand(new List<string> { "999" }, user);
        await Task.Delay(100);
        QueueSizeCommand(new List<string> { "-5" }, user);
        await Task.Delay(100);

        RequestsPerUserCommand(new List<string> { "3" }, user);
        await Task.Delay(100);
        RequestsPerUserCommand(emptyArgs, user);
        await Task.Delay(100);
        RequestsPerUserCommand(new List<string> { "alsdkfj" }, user);
        await Task.Delay(100);
        RequestsPerUserCommand(new List<string> { "999" }, user);
        await Task.Delay(100);
        RequestsPerUserCommand(new List<string> { "-5" }, user);
        await Task.Delay(100);

        InfoCommand(emptyArgs, user);
        await Task.Delay(100);


        // Disable debug mode just in case...
        _debug = false;
    }

    private void QueueSizeCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        if (arguments.Count <= 0)
        {
            SendReplyMessage(user, $"Please provide a number. !queuesize n");
            return;
        }

        if (Int32.TryParse(arguments[0], out int size))
        {
            if (size <= 0)
            {
                CloseQueueCommand(arguments, user);
                return;
            } else
            {
                int setSize = RequestsManager.Instance.SetRequestQueueSize(size);
                SendReplyMessage(user, $"Set request queue size to {setSize}");
            }
        }
        else
        {
            SendReplyMessage(user, $"I did not understand \"{arguments[0]}\"!");
        }
    }

    private void RequestsPerUserCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        if (arguments.Count <= 0)
        {
            SendReplyMessage(user, $"Please provide a number. !peruser n");
            return;
        }

        if (Int32.TryParse(arguments[0], out int size))
        {
            int setSize = RequestsManager.Instance.SetMaxRequestsPerUser(size);
            SendReplyMessage(user, $"Set max requests per user to {setSize}");
        } else
        {
            SendReplyMessage(user, $"I did not understand {arguments[0]}!");
        }
    }

    private void ProductRequestCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        if (arguments.Count <= 0)
        {
            return;
        }

        arguments[0] = arguments[0].Trim();

        ProductRequest(string.Join(" ", arguments), user, null);
    }

    public bool ProductRequest(string request, Twitchmata.Models.User user, string voiceID)
    {
        if (!RequestsManager.Instance.IsQueueOpen)
        {
            Debug.LogWarning("Got ProductRequestCommand, but queue is closed!");
            SendReplyMessage(user, "Sorry, queue is closed!");
            return false;
        }

        if (!user.IsModerator && !user.IsBroadcaster && RequestsManager.Instance.UserHasMaxRequestsInQueue(user))
        {
            Debug.LogWarning("Got ProductRequestCommand, but user has reached max requests!");
            SendReplyMessage(user, $"You already have {RequestsManager.Instance.MaxRequestsPerUser} request{(RequestsManager.Instance.MaxRequestsPerUser > 1 ? "s" : "")} in the queue!");
            return false;
        }

        if (!user.IsModerator && !user.IsBroadcaster && RequestsManager.Instance.IsUserInTimeout(user))
        {
            SendReplyMessage(user, $"Your request was just played! Please wait {RequestsManager.Instance.SecondsRemainingOnTimeout(user)} sec");
            return false;
        }

        if (request.Equals(""))
        {
            SendReplyMessage(user, $"Your request cannot be blank!");
            return false;
        }

        if (request.Length > RequestsManager.Instance.RequestMaxCharacters)
        {
            SendReplyMessage(user, $"Requests cannot be longer than {RequestsManager.Instance.RequestMaxCharacters} characters! Your req is {request.Length}");
            return false;
        }

        try
        {
            RequestsManager.Instance.ProductRequestList.Add(new TwitchRequest(user, request, voiceID));

            SendReplyMessage(user, $"Added request to queue! Position: {RequestsManager.Instance.ProductRequestList.Count}");

            // If next script was generated by the bot, abandon it and use the request
            if (!MainScriptHandler.Instance.IsNextScriptUserRequest())
            {
                if (StateMachine.Instance.currentState.ID == StateMachine.StateIDs.INTRO || 
                    StateMachine.Instance.currentState.ID == StateMachine.StateIDs.MAIN)
                {
                    MainScriptHandler.Instance.IgnoreNextScriptAndGenerateNewOne();
                }
            }

            return true;
        } catch (InvalidOperationException)
        {
            Debug.LogWarning("Could not add request to queue because the queue is full!");

            SendReplyMessage(user, "The request queue is full. Please try again later!");
        } catch (Exception e)
        {
            Debug.LogError(e);
        }

        return false;
    }

    private void QueueCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        int offset = 0;
        int totalInQueue = RequestsManager.Instance.ProductRequestList.Count;

        if (totalInQueue <= 0)
        {
            if (RequestsManager.Instance.IsQueueOpen)
            {
                SendReplyMessage(user, "Queue is empty!");
            } else
            {
                SendReplyMessage(user, "Queue is empty and also closed!");
            }
            return;
        }

        if (arguments.Count > 0)
        {
            TryParseIndexFromArg(arguments[0], true, out offset);

            if (offset >= totalInQueue)
            {
                SendReplyMessage(user, $"There are only ({totalInQueue}) requests in the queue!");
                return;
            }

            if (offset < 0)
            {
                offset = 0;
            }
        }

        string textToSend = $"QUEUE ({totalInQueue}) -> ";

        for (int i = offset; (i < totalInQueue && i < offset + 5); ++i)
        {
            TwitchRequest request = RequestsManager.Instance.ProductRequestList[i];

            textToSend += $"{i + 1}. {request.RequestText} ";
        }

        SendReplyMessage(user, textToSend);
    }

    private void OpenQueueCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        if (RequestsManager.Instance.IsQueueOpen)
        {
            SendReplyMessage(user, $"Queue is already open!");
        } else
        {
            RequestsManager.Instance.OpenQueue();
            SendReplyMessage(user, "Queue is now open!");
        }
    }

    private void CloseQueueCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        if (RequestsManager.Instance.IsQueueOpen)
        {
            RequestsManager.Instance.CloseQueue();
            SendReplyMessage(user, "Queue is now closed!");
        }
        else
        {
            SendReplyMessage(user, "Queue is already closed!");
        }
    }

    private void RemoveItemCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        if (arguments.Count <= 0 || arguments[0].Trim().Equals(string.Empty))
        {
            // Try to remove the users own message
            int i = RequestsManager.Instance.GetRequestIndexForUserInQueue(user);
            if (i >= 0)
            {
                    RemoveAndSendChatMessage(user, i);
            }
            else
            {
                SendChatMessage($"{user.DisplayName} you do not have any requests in the queue!");
            }

            return;
        }

        if (TryParseIndexFromArg(arguments[0], true, out int index))
        {
            if (user.IsModerator || user.IsBroadcaster)
            {
                RemoveAndSendChatMessage(user, index);
            } else
            {
                int i = RequestsManager.Instance.GetRequestIndexForUserInQueue(user);

                if (i == index)
                {
                    RemoveAndSendChatMessage(user, i);
                } else
                {
                    SendChatMessage($"{user.DisplayName} You cannot remove someone else's request!");
                }

                return;
            }
        } else
        {
            SendReplyMessage(user, $"I did not understand \"{arguments[0]}\". Please include which number in the queue to remove.");
        }
    }

    private void ClearQueueCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        if (RequestsManager.Instance.ProductRequestList.Count <= 0)
        {
            SendReplyMessage(user, "Queue is already empty!");
            return;
        }

        RequestsManager.Instance.ProductRequestList.Clear();

        SendReplyMessage(user, "Cleared the queue!");
    }

    private void SkipCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        StateMachine.Instance.EndShowImmediately();

        SendReplyMessage(user, "Skipped current show! You may need to wait a bit for the next one.");
    }

    private void SetTimeoutCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        if (arguments.Count <= 0 || arguments[0].Equals(""))
        {
            SendReplyMessage(user, "Please provide the number of seconds! Ex: !timeout 30");
            return;
        }

        if (Int32.TryParse(arguments[0], out int timeout))
        {
            int actualTime = RequestsManager.Instance.SetTimeout(timeout);
            SendReplyMessage(user, $"Set timeout to {actualTime}!");
        }
        else
        {
            SendReplyMessage(user, $"I did not understand \"{arguments[0]}\"!");
        }
    }

    private void InfoCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        string info = 
            $"Queue is ({(RequestsManager.Instance.IsQueueOpen ? "OPEN" : "CLOSED")})" +
            $" | Queue size: ({RequestsManager.Instance.ProductRequestList.MaxSize})" +
            $" | Reqs per user: ({RequestsManager.Instance.MaxRequestsPerUser})" +
            $" | Timeout: ({RequestsManager.Instance.TimeoutSeconds} sec)" +
            $" | Show steps: ({StateMachine.Instance.scriptBuilder.maxSteps})" +
            $" | Max req length: ({RequestsManager.Instance.RequestMaxCharacters})";

        SendReplyMessage(user, info);
    }

    private void HelpCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        SendReplyMessage(user, "See the \"REQUESTS\" panel below the stream for more info!");
    }

    private void StrictCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        if (arguments.Count <= 0 || arguments[0].Equals("on"))
        {
            if (strictModeration)
            {
                SendReplyMessage(user, "Strict mode is already on!");
            }
            else
            {
                strictModeration = true;
                SendReplyMessage(user, "Strict mode enabled!");
            }
        } else if (arguments[0].Equals("off"))
        {
            if (!strictModeration)
            {
                SendReplyMessage(user, "Strict mode is already off!");
            }
            else
            {
                strictModeration = false;
                SendReplyMessage(user, "Strict mode disabled!");
            }
        } else
        {
            SendReplyMessage(user, $"I did not understand: {arguments[0]}");
        }
    }

    private void StepsCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        if (arguments.Count <= 0 || arguments[0].Equals(""))
        {
            SendReplyMessage(user, "Please provide the number of steps!");
            return;
        }

        if (Int32.TryParse(arguments[0], out int steps))
        {
            if (steps > 8)
            {
                steps = 8;
            } else if (steps < 1)
            {
                steps = 1;
            }

            StateMachine.Instance.scriptBuilder.maxSteps = steps;

            int minSteps = steps - 3;

            StateMachine.Instance.scriptBuilder.minSteps = minSteps > 1 ? minSteps : 1;

            SendReplyMessage(user, $"Set steps to: {steps}!");
        }
        else
        {
            SendReplyMessage(user, $"I did not understand \"{arguments[0]}\"!");
        }
    }

    private void VoiceChangeCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        if (arguments.Count <= 0 || arguments[0].Equals(""))
        {
            SendReplyMessage(user, "Please provide a voice to switch to!");
            return;
        }

        VoiceChangeCommandAsync(arguments[0], user);
    }

    private async void VoiceChangeCommandAsync(string voiceID, Twitchmata.Models.User user)
    {
        if (await VoiceChangeManager.Instance.SetDefaultVoice(voiceID))
        {
            SendReplyMessage(user, "Voice found! Changing when next dialog is generated. (May not be the next show, but one after.)");
        } else
        {
            SendReplyMessage(user, "Error! I could not find that voice!");
        }
    }

    private void RestartCommand(List<string> arguments, Twitchmata.Models.User user)
    {
        RefundAndRestart();
    }

    private async void RefundAndRestart()
    {
        SendMessage("Refunding request redeems...");
        UserSpecificRedeemManager.Instance.RefundAndRemoveAllRedeems();
        await Task.Delay(3000);
        SendMessage("Restarting!");
        Debug.Log("Restarting!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RemoveAndSendChatMessage(Twitchmata.Models.User user, int index)
    {
        if (RequestsManager.Instance.ProductRequestList.Count <= 0)
        {
            SendReplyMessage(user, $"Nothing to remove. Queue is empty!");
            return;
        }

        if (index < 0 || index >= RequestsManager.Instance.ProductRequestList.Count)
        {
            SendReplyMessage(user, $"Could not remove request! Index out of range!");
            return;
        }

        TwitchRequest twitchRequest = RequestsManager.Instance.ProductRequestList[index];
        UserSpecificRedeemManager.Instance.RefundAndRemoveAllRedeemsForUser(twitchRequest.User);

        RequestsManager.Instance.ProductRequestList.RemoveAt(index);

        if (RequestsManager.Instance.ProductRequestList.Count <= 0)
        {
            SendReplyMessage(user, $"Removed final request from queue! Queue is now empty!");
        } else
        {
            SendReplyMessage(user, $"Removed request {index + 1} from the queue! Remaining: {RequestsManager.Instance.ProductRequestList.Count}");
        }
    }

    private bool TryParseIndexFromArg(string arg, bool inputIndexStartsAtOne, out int index)
    {
        if (Int32.TryParse(arg, out int num))
        {
            if (num == 0)
            {
                index = num;
            }
            else if (num < 0)
            {
                index = num + RequestsManager.Instance.ProductRequestList.Count;
            }
            else
            {
                index = inputIndexStartsAtOne ? num - 1 : num;
            }
            return true;
        }
        else
        {
            Debug.LogWarning("Could not parse number from QueueCommand arg! Defaulting to zero");
        }

        index = 0;
        return false;
    }
}
