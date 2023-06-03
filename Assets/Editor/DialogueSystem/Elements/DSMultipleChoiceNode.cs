using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements {
    using Data.Save;
    using UnityEditor.UIElements;
    using Utilities;
    using Windows;
    public class DSMultipleChoiceNode : DSNode
    {

        public override void Initialize(string nodeName, DSGraphView dSGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dSGraphView, position);
            DialogueType = Enumerations.DSDialogueType.MultipleChoice;

            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = "New Choice",
            };

            Choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();

            Button addChoiceButton = DSElementUtility.CreateButton("Add Choice", () =>
            {
                DSChoiceSaveData choiceData = new DSChoiceSaveData()
                {
                    Text = "New Choice"
                };

                Choices.Add(choiceData);
                Port choicePort = CreateChoicePort(choiceData);
                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addChoiceButton);

            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }

        private static VisualElement CreateSanityFoldout(DSChoiceSaveData choice)
        {
            VisualElement visualElement = new VisualElement();
            FloatField floatField = DSElementUtility.CreateFloatField(choice.SanityThreshold, null, callback =>
            {
                choice.SanityThreshold = callback.newValue;
            });

            Toggle toggleField = DSElementUtility.CreateToggle("Sanity",choice.HasSanityThreshold, callback =>
            {
                choice.HasSanityThreshold = callback.newValue;
                floatField.style.display = callback.newValue ? DisplayStyle.Flex : DisplayStyle.None;
            });

            floatField.style.display = choice.HasSanityThreshold ? DisplayStyle.Flex : DisplayStyle.None;
            visualElement.style.flexDirection = FlexDirection.Row;
            toggleField.ElementAt(0).style.minWidth = 10;

            visualElement.Add(toggleField);
            visualElement.Add(floatField);
            return visualElement;
        }
        #region Elements Creation
        private Port CreateChoicePort(object userData)
        {
            Port choicePort = this.CreatePort();

            choicePort.userData = userData;
            DSChoiceSaveData choiceData = (DSChoiceSaveData)userData;

            Button deleteChoiceButton = DSElementUtility.CreateButton("X", () => 
            { 
                if (Choices.Count == 1)
                {
                    return;
                }
                if (choicePort.connected)
                {
                    graphView.DeleteElements(choicePort.connections);
                }
                Choices.Remove(choiceData);
                graphView.RemoveElement(choicePort);
            });

            deleteChoiceButton.AddToClassList("ds-node__button");

            TextField choiceTextField = DSElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });

            choiceTextField.AddClasses(
                "ds-node__textfield",
                "ds-node__choice-textfield",
                "ds-node__textfield__hidden"
            );

            VisualElement sanityFoldout = CreateSanityFoldout(choiceData);

            choicePort.Add(choiceTextField);
            choicePort.Add(sanityFoldout);
            choicePort.Add(deleteChoiceButton);
            return choicePort;
        }
        #endregion
    }
}

