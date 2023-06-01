using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace DS.Elements 
{
    using Enumerations;
    using Utilities;
    using Windows;
    using Data.Save;

    public class DSNode : Node
    {
        public string ID { get; set; }
        public string DialogueName { get; set; }
        public List<DSChoiceSaveData> Choices { get; set; }
        public string Text { get; set; }
        public Character Character { get; set; }
        public InventoryItem Item { get; set; }

        [HideInInspector]
        public bool HasSceneTransition { get; set; }

        [HideInInspector]
        public int NextSceneIndex { get; set; }
        public DSDialogueType DialogueType { get; set; }
        public DSGroup Group { get; set; }

        private Color defaultBackgroundColor;
        protected DSGraphView graphView;

        public virtual void Initialize(string nodeName, DSGraphView dSGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            DialogueName = nodeName;
            Choices = new List<DSChoiceSaveData>();
            Text = "Dialogue text";
            Character = null;
            HasSceneTransition = false;
            NextSceneIndex = 0;
            graphView = dSGraphView;
            defaultBackgroundColor = new Color(29f/255f, 29f/255f, 30f/255f);

            SetPosition(new Rect(position, Vector2.zero));
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        #region Overrided Methods
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());
            base.BuildContextualMenu(evt);
        }
        #endregion

        public virtual void Draw()
        {
            // Title container
            TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, null, callback =>
            {
                TextField target = (TextField)callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(DialogueName))
                    {
                        ++graphView.NameErrorsAmount;
                    }
                } else
                {
                    if (string.IsNullOrEmpty(DialogueName))
                    {
                        --graphView.NameErrorsAmount;
                    }
                }

                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);
                    DialogueName = target.value;
                    graphView.AddUngroupedNode(this);
                    return;
                }

                DSGroup currentGroup = Group;
                graphView.RemoveGroupedNode(this, Group);
                DialogueName = target.value;
                graphView.AddGroupedNode(this, currentGroup);
                
            });

            dialogueNameTextField.AddClasses(
                "ds-node__textfield",
                "ds-node__filename-textfield",
                "ds-node__textfield__hidden"
            );

            titleContainer.Insert(0, dialogueNameTextField);

            // Input container
            Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Dialogue Connection";

            inputContainer.Add(inputPort);

            // Extensions Container
            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");

            TextField textTextField = DSElementUtility.CreateTextArea(Text, null, callback =>
            {
                Text = callback.newValue;
            });

            textTextField.AddClasses(
                "ds-node__textfield",
                "ds-node__quote-textfield"
            );

            textFoldout.Add(textTextField);

            ObjectField characterField = DSElementUtility.CreateObjectField<Character>(Character, "Character", callback =>
            {
                Character = (Character)callback.newValue;
            });


            ObjectField itemField = DSElementUtility.CreateObjectField<InventoryItem>(Item, "Item", callback =>
            {
                Item = (InventoryItem)callback.newValue;
            });

            DropdownField nextSceneDropDownField = DSElementUtility.CreateDropDownField("Scene", Constants.SceneNamesArray.ToList(), NextSceneIndex, callback =>
            {
                NextSceneIndex = callback.newValue;
            });

            Toggle hasSceneTransitionField = DSElementUtility.CreateToggle("Has Scene Transition", HasSceneTransition, callback =>
            {
                HasSceneTransition = callback.newValue;
                nextSceneDropDownField.visible = HasSceneTransition;
            });

            nextSceneDropDownField.visible = HasSceneTransition;

            customDataContainer.Add(characterField);
            customDataContainer.Add(itemField);
            customDataContainer.Add(hasSceneTransitionField);
            customDataContainer.Add(nextSceneDropDownField);

            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }

        #region Utility Methods

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                graphView.DeleteElements(port.connections);
            }
        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port)inputContainer.Children().First();
            return !inputPort.connected;
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }
        #endregion
    }
}

