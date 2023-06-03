using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Utilities
{
    using Elements;
    using UnityEditor.UIElements;

    public static class DSElementUtility
    {

        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };
            return button;
        }
        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };
            return foldout;
        }

        public static Port CreatePort(this DSNode node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = portName;
            return port;
        }

        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }

        public static FloatField CreateFloatField(float value = 0f, string label = null, EventCallback<ChangeEvent<float>> onValueChanged = null)
        {
            FloatField floatField = new FloatField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                floatField.RegisterValueChangedCallback(onValueChanged);
            }

            return floatField;
        }

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, label, onValueChanged);
            textArea.multiline = true;

            return textArea;
        }

        public static ObjectField CreateObjectField<T>(UnityEngine.Object value, string label = null, EventCallback<ChangeEvent<UnityEngine.Object>> onValueChanged = null)
        {
            ObjectField objectField = new ObjectField(label) { 
                value = value
            };
            objectField.objectType = typeof(T);
            if (onValueChanged != null)
            {
                objectField.RegisterValueChangedCallback(onValueChanged);
            }
            return objectField;
        }

        public static DropdownField CreateDropDownField(string label = null, List<string> choices = null, int index = 0, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            DropdownField dropdownField = new DropdownField(label) { 
                choices = choices,
                index = index == -1 ? 0 : index,
                value = choices[index == -1 ? 0 : index]
            };

            if (onValueChanged != null)
            {
                dropdownField.RegisterValueChangedCallback(onValueChanged);
            }

            return dropdownField;
        }

        public static Toggle CreateToggle(string label = null, bool value = false, EventCallback<ChangeEvent<bool>> onValueChanged = null)
        {
            Toggle toggle = new Toggle(label) { 
                value = value
            };

            if (onValueChanged != null)
            {
                toggle.RegisterValueChangedCallback(onValueChanged);
            }

            return toggle;
        }
    }
}


