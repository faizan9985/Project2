// Copyright 2022-2025 Niantic.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Niantic.Lightship.AR.ObjectDetection;
using Niantic.Lightship.AR.Subsystems.ObjectDetection;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDetectionP2 : MonoBehaviour
{
    
    [SerializeField]
    private float _probabilityThreshold = 0.5f;

    //[SerializeField] private SliderToggle _filterToggle;

    [SerializeField]
    private ARObjectDetectionManager _objectDetectionManager;

    private Color[] _colors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.cyan,
        Color.white,
        Color.black
    };

    //[SerializeField] [Tooltip("Slider GameObject to set probability threshold")] private Slider _probabilityThresholdSlider;

    //[SerializeField]
    //[Tooltip("Text to display current slider value")]
    private Text _probabilityThresholdText;

    //[SerializeField]
    private Dropdown _categoryDropdown;

    [SerializeField]
    private DrawRect _drawRect;

    [SerializeField]
    private Canvas _canvas;
    [Header("-----------------User Input-----------------")]
    [SerializeField] private ObjectManagement _objectManagementScript;

    [SerializeField]
    [Tooltip("Categories to find - working")]
    private List<string> _categoryNames;

    [SerializeField]
    [Tooltip("All Categories to find - storage")]
    private List<string> _categoryNamesAll;

    [SerializeField] DrawUI _drawUI;

    //[SerializeField] TMP_Text _debugTextbox;

    private bool _filterOn = false;

    [SerializeField] bool ProcessOnOff = true;

    // The name of the actively selected semantic category
    private string _categoryName = string.Empty;



    private void Awake()
    {
        _canvas = FindFirstObjectByType<Canvas>();
    }

    private void OnMetadataInitialized(ARObjectDetectionModelEventArgs args)
    {
        _objectDetectionManager.ObjectDetectionsUpdated += ObjectDetectionsUpdated;

        // Display person by default.
        _categoryName = _categoryNames[0];
        if (_categoryDropdown is not null && _categoryDropdown.options.Count == 0)
        {
            _categoryDropdown.AddOptions(_categoryNames.ToList());

            var dropdownList = _categoryDropdown.options.Select(option => option.text).ToList();
            _categoryDropdown.value = dropdownList.IndexOf(_categoryName);
        }

    }




 private void ObjectDetectionsUpdated(ARObjectDetectionsUpdatedEventArgs args)
    {
        string resultString = "";
        float _confidence = 0;
        string _name = "";
        //string _objectName = "";
        var result = args.Results; 
        if (result == null)
        {
            return;
        }

        if (!ProcessOnOff)
        {
            return;
        }
        _drawRect.ClearRects();
        _drawUI.CloseInfo();

        for (int i = 0; i < result.Count; i++)
        {
            if (!_filterOn)
            {
                var detection = result[i];
                var categorizations = detection.GetConfidentCategorizations(_probabilityThreshold);
                if (categorizations.Count <= 0)
                {
                    break;
                }

                categorizations.Sort((a, b) => b.Confidence.CompareTo(a.Confidence));
                var categoryToDisplay = categorizations[0];
                _confidence = categoryToDisplay.Confidence;
                _name = categoryToDisplay.CategoryName;
            }
            else
            {
                //Get name and confidence of the detected object in a given category.
                _confidence = result[i].GetConfidence(_categoryName);

                //filter out the objects with confidence less than the threshold 
                if (_confidence < _probabilityThreshold)
                {
                    break;
                }
                _name = _categoryName; //original line

            }


            int h = Mathf.FloorToInt(_canvas.GetComponent<RectTransform>().rect.height);
            int w = Mathf.FloorToInt(_canvas.GetComponent<RectTransform>().rect.width);

            //Get the rect around the detected object
            var _rect = result[i].CalculateRect(w, h, Screen.orientation);

            //resultString = $"{_name}: {_confidence}\n"; //original line
            resultString = $"{_name}";

            //check to see if the category name of the detected object is in the short list 
            for (int ii = 0; ii < _categoryNames.Count; ii++)
            {
                if (_name == _categoryNames[ii])
                {
                    SetProcess(); //turn off the detection engine
                    //Draw the Rect.
                    _drawRect.CreateRect(_rect, _colors[i % _colors.Length], resultString);
                    
                    _drawUI.OpenInfo(_name); //passing the category name
                    break;
                }
            }
           

        }
    }
    
    private void OnThresholdChanged(float newThreshold)
    {
        _probabilityThreshold = newThreshold;
        _probabilityThresholdText.text = "Confidence : " + newThreshold.ToString();
    }
    private void categoryDropdown_OnValueChanged(int val)
    {
        // Update the display category from the dropdown value.
        _categoryName = _categoryDropdown.options[val].text;
    }
    public void Start()
    {

        _objectDetectionManager.enabled = true;
        _objectDetectionManager.MetadataInitialized += OnMetadataInitialized;
        
    }
    private void ToggleFilter(bool on)
    {
        _filterOn = on;
        _categoryDropdown.interactable = on;
    }
    private void OnDestroy()
    {
        _objectDetectionManager.MetadataInitialized -= OnMetadataInitialized;
        _objectDetectionManager.ObjectDetectionsUpdated -= ObjectDetectionsUpdated;

    }
    //new code to pause the process
    public void SetProcess()
    {
        ProcessOnOff = !ProcessOnOff;
    }
    //new code to set object array
    public void SetList(List<string> _tempList)
    {
        _categoryNames = new List<string>();
        _categoryNames = _tempList;
    }
}