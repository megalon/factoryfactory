using System.Collections;
using System.Collections.Generic;
using Twitchmata.Models;
using UnityEngine;

public class UserSpecificRedeemManager : MonoBehaviour
{
    public static UserSpecificRedeemManager Instance;
    private Dictionary<string, List<UserSpecificRedeem>> _redeemsDict;

    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        _redeemsDict = new Dictionary<string, List<UserSpecificRedeem>>();
    }

    public void StartRedeemsForUser(User user, Modification.ModificationTypes modificationType)
    {
        EnableOrDisableRedeemsForUser(user, modificationType, true);
    }

    public void StopRedeemsForUser(User user, Modification.ModificationTypes modificationType)
    {
        EnableOrDisableRedeemsForUser(user, modificationType, false);
    }

    private void EnableOrDisableRedeemsForUser(User user, Modification.ModificationTypes modificationType, bool enable)
    {
        if (!_redeemsDict.ContainsKey(user.UserId))
        {
            Debug.LogWarning("No redeems found for this user:" + user.DisplayName + " ID:" + user.UserId);
            return;
        }

        // Keep track of ids to remove so we don't remove them in the loop
        List<string> idsToRemove = new List<string>();

        if (_redeemsDict.TryGetValue(user.UserId, out List<UserSpecificRedeem> redeems))
        {
            // Keep track of redeems to remove so we don't remove them in the loop
            List<UserSpecificRedeem> redeemsToRemove = new List<UserSpecificRedeem>();

            foreach (UserSpecificRedeem redeem in redeems)
            {
                if (redeem.ModificationType != modificationType) continue;

                if (enable)
                {
                    if (!redeem.IsActive)
                    {
                        redeem.Activate();
                    }
                }
                else
                {
                    if (redeem.IsActive)
                    {
                        redeem.Deactivate();
                        redeemsToRemove.Add(redeem);
                    }
                }
            }

            foreach (UserSpecificRedeem redeem in redeemsToRemove)
            {
                _redeemsDict[user.UserId].Remove(redeem);
            }

            if (_redeemsDict[user.UserId].Count <= 0)
            {
                idsToRemove.Add(user.UserId);
            }
        }

        foreach (string userId in idsToRemove)
        {
            _redeemsDict.Remove(userId);
        }
    }

    public void AddUserSpecificRedeem(UserSpecificRedeem redeem)
    {
        string userID = redeem.Redemption.User.UserId;
        if (_redeemsDict.ContainsKey(userID))
        {
            _redeemsDict[userID].Add(redeem);
        }
        else
        {
            List<UserSpecificRedeem> redeemList = new List<UserSpecificRedeem>();
            redeemList.Add(redeem);
            _redeemsDict.Add(userID, redeemList);
        }
    }

    public void RefundAndRemoveAllRedeemsForUser(User user)
    {
        if (_redeemsDict.TryGetValue(user.UserId, out List<UserSpecificRedeem> redeems))
        {
            foreach (UserSpecificRedeem redeem in redeems)
            {
                TwitchChannelPointManager.Instance.CancelRedemption(redeem.Redemption);
            }
        }

        _redeemsDict.Remove(user.UserId);
    }

    /// <summary>
    /// Refund all redeems for everyone and remove them from the list
    /// </summary>
    public void RefundAndRemoveAllRedeems()
    {
        foreach (string key in _redeemsDict.Keys)
        {
            foreach (UserSpecificRedeem redeem in _redeemsDict[key])
            {
                TwitchChannelPointManager.Instance.CancelRedemption(redeem.Redemption);
            }
        }

        _redeemsDict.Clear();
    }
}
