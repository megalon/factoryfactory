using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestStateMachineUI : MonoBehaviour
{
    private VisualElement root;

    private Button nextButton;
    private Label currentStateLabel;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        nextButton = root.Q<Button>("ButtonNext");
        currentStateLabel = root.Q<Label>("LabelCurrentState");

        nextButton.clicked += () => StateMachine.NextStateAction();

        //StateMachine.ChangeStateAction += UpdateCurrentStateText;
    }

    private void UpdateCurrentStateText(StateMachine.StateIDs newStateID)
    {
        currentStateLabel.text = newStateID.GetType().Name;
    }
}
