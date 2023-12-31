﻿using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LethalSettings.UI.Components;

public class SliderComponent : MenuComponent
{
    public string Text { get; set; } = "Slider";
    public bool ShowValue { get; set; } = true;
    public bool WholeNumbers {  get; set; } = true;
    public bool Enabled { get; set;} = true;
    public float MinValue { get; set; } = 0f;
    public float MaxValue { get; set; } = 100f;

    internal float _currentValue = 50f;
    public float Value
    {
        get => _currentValue;
        set
        {
            _currentValue = value;
            if (componentObject != null)
            {
                componentObject.SetValue(value);
            }
        }
    }
    public Action<SliderComponent, float> OnValueChanged { internal get; set; } = (self, value) => { };
    public Action<SliderComponent> OnInitialize { get; set; } = (self) => { };

    private SliderComponentObject componentObject;

    public override GameObject Construct(GameObject root)
    {
        componentObject = GameObject.Instantiate(Assets.SliderPrefab, root.transform);
        return componentObject.Initialize(this);
    }
}

internal class SliderComponentObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI label;

    [SerializeField]
    private Slider slider;

    private SliderComponent component;

    internal GameObject Initialize(SliderComponent component)
    {
        this.component = component;

        slider.onValueChanged.AddListener(SetValue);

        component.OnInitialize?.Invoke(component);

        return gameObject;
    }

    private void FixedUpdate()
    {
        slider.wholeNumbers = component.WholeNumbers;
        slider.interactable = component.Enabled;
        slider.minValue = component.MinValue;
        slider.maxValue = component.MaxValue;
        label.text = $"{component.Text} {(component.ShowValue ? slider.value : "")}";
    }

    internal void SetValue(float value)
    {
        slider.value = value;
        component._currentValue = value;
        component.OnValueChanged?.Invoke(component, value);
    }
}
