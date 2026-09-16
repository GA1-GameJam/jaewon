using System.Collections.Generic;
using UnityEngine;

public class EnhancePanelManager : MonoBehaviour
{
    [Header("Enhance Panel")]
    [SerializeField] private GameObject _EnhancePanel;

    [Header("All Enhance Buttons")]
    [SerializeField] private List<EnhanceButton> _enhanceButtons;
    [SerializeField] private int _displayButtonCount = 3;

    private readonly List<EnhanceButton> _buttonsCandidate = new();
    private readonly List<EnhanceButton> _activeButtons = new();
    private readonly Dictionary<EnhanceButton, EnhanceButton> _runtimeToTemplate = new();
    private bool _candidateInitialized;

    public void OnEnhancePanel()
    {
        if (_EnhancePanel == null)
        {
            Debug.LogError("EnhancePanelManager: Enhance Panel is not assigned.");
            return;
        }

        _EnhancePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        MakeEnhanceButtons();
        _EnhancePanel.GetComponent<UIAnimator>()?.Open();
    }

    public void OffEnhancePanel()
    {
        DeleteEnhanceButtons();
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (_EnhancePanel == null)
        {
            return;
        }

        UIAnimator animator = _EnhancePanel.GetComponent<UIAnimator>();
        if (animator != null)
        {
            animator.Close();
        }
        else
        {
            _EnhancePanel.SetActive(false);
        }
    }

    private void MakeEnhanceButtons()
    {
        DeleteEnhanceButtons();

        if (_enhanceButtons == null || _enhanceButtons.Count == 0)
        {
            Debug.LogWarning("EnhancePanelManager: No enhance buttons are assigned.");
            return;
        }

        if (!_candidateInitialized)
        {
            foreach (EnhanceButton button in _enhanceButtons)
            {
                if (button != null && !_buttonsCandidate.Contains(button))
                {
                    _buttonsCandidate.Add(button);
                }
            }

            _candidateInitialized = true;
        }

        List<EnhanceButton> availableButtons = new(_buttonsCandidate);
        int buttonCount = Mathf.Min(_displayButtonCount, availableButtons.Count);
        for (int i = 0; i < buttonCount; i++)
        {
            int randomIndex = Random.Range(0, availableButtons.Count);
            EnhanceButton button = availableButtons[randomIndex];
            availableButtons.RemoveAt(randomIndex);

            GameObject buttonObject = Instantiate(button.gameObject, _EnhancePanel.transform);
            EnhanceButton runtimeButton = buttonObject.GetComponent<EnhanceButton>();
            if (runtimeButton == null)
            {
                Destroy(buttonObject);
                continue;
            }

            runtimeButton.gameObject.SetActive(true);
            _runtimeToTemplate[runtimeButton] = button;
            _activeButtons.Add(runtimeButton);
        }
    }

    private void DeleteEnhanceButtons()
    {
        foreach (EnhanceButton button in _activeButtons)
        {
            if (button != null)
            {
                _runtimeToTemplate.Remove(button);
                Destroy(button.gameObject);
            }
        }

        _activeButtons.Clear();
    }

    public void SwitchButtonsToAwakening(EnhanceButton button)
    {
        if (button == null || !_activeButtons.Remove(button))
        {
            return;
        }

        _runtimeToTemplate.TryGetValue(button, out EnhanceButton template);
        template ??= button;
        _buttonsCandidate.Remove(template);
        button.gameObject.SetActive(false);

        if (button.AwakeingButtons == null)
        {
            Debug.LogWarning($"EnhancePanelManager: {button.name} has no awakening button.");
            return;
        }

        GameObject awakeningPrefab = button.AwakeingButtons;
        EnhanceButton awakeningTemplate = awakeningPrefab.GetComponent<EnhanceButton>();
        if (awakeningTemplate == null)
        {
            button.gameObject.SetActive(false);
            Debug.LogWarning($"EnhancePanelManager: {awakeningPrefab.name} has no EnhanceButton component.");
            return;
        }

        GameObject awakeningObject = Instantiate(awakeningPrefab, _EnhancePanel.transform);
        EnhanceButton awakeningButton = awakeningObject.GetComponent<EnhanceButton>();
        if (awakeningButton == null)
        {
            Destroy(awakeningObject);
            Debug.LogWarning($"EnhancePanelManager: {button.AwakeingButtons.name} has no EnhanceButton component.");
            return;
        }

        if (!_buttonsCandidate.Contains(awakeningTemplate))
        {
            _buttonsCandidate.Add(awakeningTemplate);
        }

        awakeningButton.gameObject.SetActive(true);
        _runtimeToTemplate[awakeningButton] = awakeningTemplate;
        _activeButtons.Add(awakeningButton);
    }
}
