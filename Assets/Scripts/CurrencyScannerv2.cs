using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]

public class CurrencyScannerv2 : MonoBehaviour
{
    ARTrackedImageManager m_TrackedImageManager;

    public CurrencyScriptableObject[] AllCountryCurrency;

    private void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
    }

    private void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;
    }

    void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateInfo(trackedImage);
        }
    }

    void UpdateInfo(ARTrackedImage trackedImage)
    {
        TextMeshProUGUI CurrencyInfoText = trackedImage.transform.GetChild(0).GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        if (trackedImage.trackingState != TrackingState.None)
        {
            string CurrencyName = trackedImage.referenceImage.name;
            string CurrencyCountry = "";
            string CurrencyType = "";
            bool isCurrencyDetected = false;
            foreach (var currentCountryCurrency in AllCountryCurrency)
            {
                foreach (var currencyNote in currentCountryCurrency.currencyData)
                {
                    if (currencyNote.m_CurrencyValue == CurrencyName)
                    {
                        CurrencyCountry = currencyNote.m_Binding.m_CountryName;
                        CurrencyType = currencyNote.m_Binding.m_CurrencyType;
                        isCurrencyDetected = true;
                    }
                }
            }
            CurrencyInfoText.text = isCurrencyDetected ? FormatCurrencyStrings(CurrencyName, CurrencyCountry, CurrencyType) : "Not in Library";
        }
    }
    
    string FormatCurrencyStrings(string currencyName, string currencyCountry, string currencyType)
    {
        currencyName = currencyName.StartsWith("F") ? currencyName.Remove(0, 4) : currencyName.Remove(0, 3);
        return string.Format("{0} {1} {2}", currencyCountry, currencyName, currencyType);
    }
}