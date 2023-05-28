using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using UnityEditor.Experimental.GraphView;
    using Utilities;
    using Windows;
    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(string nodeName, DSGraphView dSGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dSGraphView, position);
            DialogueType = DSDialogueType.SingleChoice;

            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = ""
            };
            Choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();
            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);
                choicePort.userData = choice;
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();

        }
    }
}

