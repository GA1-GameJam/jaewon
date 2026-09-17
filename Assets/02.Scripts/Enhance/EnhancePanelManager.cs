using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnhancePanelManager : MonoBehaviour
{
    [Header("메니징 패널")]
    [SerializeField] private GameObject _enhancePanel;
    public GameObject EnhancePanel => _enhancePanel;
    [Header("인헨스 버튼 프리팹 목록")]
    [SerializeField] private List<EnhanceButton> _enhanceButtons;

    [Header("한 번에 보여줄 버튼 개수")]
    [SerializeField] private int _displayButtonCount = 3;

    // 랜덤 선택 후보
    private readonly List<EnhanceButton> _buttonsCandidate = new();

    // 실제로 생성된 버튼
    private readonly List<EnhanceButton> _spawnedButtons = new();


    private void Awake()
    {
        InitSetting();
    }


    private void InitSetting()
    {
        foreach (EnhanceButton button in _enhanceButtons)
        {
            if (button == null)
                continue;

            // 패널에 실제 버튼 생성
            EnhanceButton initButton =
                Instantiate(button, _enhancePanel.transform, false);

            // 최초에는 전부 비활성화
            initButton.gameObject.SetActive(false);

            // 실제 생성된 버튼을 관리 리스트에 추가
            _spawnedButtons.Add(initButton);
        }
    }


    public void OnEnhancePanel()
    {
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        MakeEnhanceButtons();
    }


    public void OffEnhancePanel()
    {
        DeleteEnhanceButtons();

        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void MakeEnhanceButtons()
    {
        // 기존에 표시된 버튼 전부 비활성화
        DeleteEnhanceButtons();

        // 후보 리스트 초기화
        _buttonsCandidate.Clear();

        // 실제 생성된 버튼들을 후보에 추가
        _buttonsCandidate.AddRange(_spawnedButtons);


        // 랜덤 셔플
        for (int i = 0; i < _buttonsCandidate.Count; i++)
        {
            int randomIndex = Random.Range(i, _buttonsCandidate.Count);

            EnhanceButton temp = _buttonsCandidate[i];

            _buttonsCandidate[i] =
                _buttonsCandidate[randomIndex];

            _buttonsCandidate[randomIndex] = temp;
        }


        // 실제로 보여줄 개수
        int count = Mathf.Min(
            _displayButtonCount,
            _buttonsCandidate.Count
        );


        // 랜덤으로 선택된 버튼만 활성화
        for (int i = 0; i < count; i++)
        {
            EnhanceButton button = _buttonsCandidate[i];

            // 패널의 i번째 위치로 이동
            button.transform.SetSiblingIndex(i);

            // 활성화
            button.gameObject.SetActive(true);
        }
    }


    private void DeleteEnhanceButtons()
    {
        foreach (EnhanceButton button in _spawnedButtons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(false);
            }
        }
    }


    public void SwitchButtonsToAwakening(EnhanceButton button)
    {
        for (int i = 0; i < _spawnedButtons.Count; i++)
        {
            if (_spawnedButtons[i] == button)
            {
                // 각성 버튼으로 교체
                EnhanceButton awakeningButton =
                    button.AwakeingButtons;

                if (awakeningButton == null)
                    return;

                // 기존 버튼 제거
                Destroy(_spawnedButtons[i].gameObject);

                // 각성 버튼 생성
                EnhanceButton newButton =
                    Instantiate(
                        awakeningButton,
                        _enhancePanel.transform,
                        false
                    );

                newButton.gameObject.SetActive(false);

                // 리스트 교체
                _spawnedButtons[i] = newButton;

                break;
            }
        }
    }
}
