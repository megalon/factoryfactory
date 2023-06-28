using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestModeration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _ = TestText("bass");
        _ = TestText("ass");
        _ = TestText("what the fuck");
        _ = TestText("this fucking thing");
        _ = TestText("this fucking thing is fucked and fuck you"); // Test tenses
    }

    private async Task TestText(string test)
    {
        try
        {
            await OpenAICompleter.Instance.DoesTextPassModeration(test);
            Debug.Log($"MODERATION TEST: {test} passes moderation");
        } catch (ModerationAPIFlaggedException) {
            Debug.LogWarning($"MODERATION TEST: {test} failed API moderation");
        } catch (ModerationBannedWordException)
        {
            Debug.LogWarning($"MODERATION TEST: {test} failed banned word moderation");
        }
    }
}
