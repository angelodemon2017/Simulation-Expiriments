using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [SerializeField] private TextMeshProUGUI _stepLabel;
    [SerializeField] private TextMeshProUGUI _entities;
    [SerializeField] private Button _buttonCommand1;

    private void Awake()
    {
        Instance = this;
        _buttonCommand1.onClick.AddListener(SendCommand);
    }

    private void SendCommand()
    {
        NetworkManager.Instance.SendCommand("Add");
    }

    public void UpdateStepLabel(string step)
    {
        int tempHours = int.Parse(step);
        int days = tempHours / 24;
        int mounths = days / 30;
        int years = days / 365;

        _stepLabel.text = $"Y:{years},M:{mounths},d:{days} Step(hours): {step}";
    }

    public void UpdateEnts(List<string> ents)
    {
        _entities.text = string.Empty;
        foreach (var ent in ents)
        {
            _entities.text += $"{ent}\r\n";
        }
    }
}