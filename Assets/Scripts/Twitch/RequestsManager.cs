using Lexone.UnityTwitchChat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestsManager : MonoBehaviour
{
    public static RequestsManager Instance;

    [SerializeField]
    private int _maxProductRequestsAtStart = 10;
    [SerializeField]
    private int _maxRequestsPerUser = 1;
    [SerializeField]
    private int _timeoutSeconds = 30;
    [SerializeField]
    private int _maxCharacters = 75;

    private bool _isQueueOpen = false;
    private Dictionary<string, float> _timeoutDictionary;
    private List<string> _keysToRemove;
    private int _checkTimePeriodFrames = 60;
    private int _countToCheckTime = 0;

    public LimitedList<TwitchRequest> ProductRequestList { get; protected set; }
    public bool IsQueueOpen { get => _isQueueOpen; set { } }
    public int MaxRequestsPerUser { get => _maxRequestsPerUser; set { } }
    public int TimeoutSeconds { get => _timeoutSeconds; set { } }
    public int RequestMaxCharacters { get => _maxCharacters; set { } }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        ProductRequestList = new LimitedList<TwitchRequest>(_maxProductRequestsAtStart);
        _timeoutDictionary = new Dictionary<string, float>();
        _keysToRemove = new List<string>();
    }

    private void FixedUpdate()
    {
        if (_countToCheckTime <= 0)
        {
            _countToCheckTime = _checkTimePeriodFrames;
            CheckTimeouts();
        } else
        {
            --_countToCheckTime;
        }
    }

    private void CheckTimeouts()
    {
        float currentTime = Time.time;

        _keysToRemove.Clear();

        foreach (string key in _timeoutDictionary.Keys)
        {
            if (_timeoutDictionary[key] < currentTime)
            {
                _keysToRemove.Add(key);
            }
        }

        foreach (string key in _keysToRemove)
        {
            _timeoutDictionary.Remove(key);
        }
    }

    public bool IsUserInTimeout(Twitchmata.Models.User user)
    {
        return _timeoutDictionary.ContainsKey(user.UserId);
    }

    public int SecondsRemainingOnTimeout(Twitchmata.Models.User user)
    {
        if (IsUserInTimeout(user))
        {
            int time = (int)Mathf.Ceil(_timeoutDictionary[user.UserId] - Time.time);
            return time > 0 ? time : 0;
        }

        return 0;
    } 

    public void AddRequestToTimeout(TwitchRequest req)
    {
        if (req == null)
        {
            Debug.LogError("Tried to AddRequestToTimeout but req was null!");
            return;
        }

        if (req.User.IsModerator || req.User.IsBroadcaster)
        {
            return;
        }

        Debug.Log($"Putting {req.User.DisplayName} in timeout for {_timeoutSeconds}!");

        string key = req.User.UserId;
        float time = Time.time + _timeoutSeconds;
        if (_timeoutDictionary.ContainsKey(key))
        {
            _timeoutDictionary[key] = time;
        } else
        {
            _timeoutDictionary.Add(key, time);
        }
    }

    public bool UserHasMaxRequestsInQueue(Twitchmata.Models.User user)
    {
        int reqCount = 0;
        
        foreach (TwitchRequest req in ProductRequestList)
        {
            if (req.User.UserId == user.UserId)
            {
                ++reqCount;
            }
        }

        if (reqCount >= _maxRequestsPerUser)
        {
            return true;
        }

        return false;
    }

    public int GetRequestIndexForUserInQueue(Twitchmata.Models.User user)
    {
        for (int i = 0; i < ProductRequestList.Count; ++i)
        {
            TwitchRequest req = ProductRequestList[i];
            if (req.User.UserId == user.UserId)
            {
                return i;
            }
        }

        return -1;
    }

    public void OpenQueue()
    {
        _isQueueOpen = true;
        TwitchChannelPointManager.Instance.EnableQueueBasedRewards();
    }

    public void CloseQueue()
    {
        _isQueueOpen = false;
        TwitchChannelPointManager.Instance.DisableQueueBasedRewards();
    }

    public int SetRequestQueueSize(int size)
    {
        if (size > 50)
        {
            size = 50;
        } else if (size <= 0)
        {
            size = 1;
        }

        LimitedList<TwitchRequest> tempList = ProductRequestList;

        ProductRequestList = new LimitedList<TwitchRequest>(tempList, size);

        return size;
    }

    public int SetMaxRequestsPerUser(int size)
    {
        if (size > 0)
        {
            if (size > 10)
            {
                size = 10;
            }

            _maxRequestsPerUser = size;
        }
        else
        {
            _maxRequestsPerUser = 1;
        }

        return _maxRequestsPerUser;
    }

    public int SetTimeout(int timeout)
    {
        if (timeout > 0)
        {
            if (timeout > 60 * 10)
            {
                timeout = 60 * 10;
            }

            _timeoutSeconds = timeout;
        }
        else
        {
            _timeoutSeconds = 0;
        }

        return _timeoutSeconds;
    }
}