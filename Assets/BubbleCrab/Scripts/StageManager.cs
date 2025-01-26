using System;
using BubbleCrab.n5y;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace BubbleCrab
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] float GoalHeight = 100.0f;
        [SerializeField] CrabBehaviour _crab;
        [SerializeField] UIManager _uiManager;

        float _timer = 0.0f;
        float _maxHeight = 0.0f;
        float _defaultHeight = 0.0f;
        Rigidbody _crabRigidbody;
        InputAction _pauseAction;
        bool _isPause = false;
        IStateManager _stateManager;
        IAudioManager _audioManager;

        void Awake()
        {
            var playerInput = _crab.GetComponent<PlayerInput>();
            _pauseAction = playerInput.actions["Pause"];
            _crabRigidbody = _crab.GetComponent<Rigidbody>();
            _stateManager = FindAnyObjectByType<StateManager>();
            _audioManager = FindAnyObjectByType<AudioManager>();
            // 最初はタイトル
            _stateManager.ToTitle();
        }

        void OnEnable()
        {
            _defaultHeight = _crab.transform.position.y;
            _pauseAction.actionMap.Enable();
            _pauseAction.performed += SwitchPause;
        }

        void OnDisable()
        {
            _pauseAction.performed -= SwitchPause;
            _pauseAction.actionMap.Disable();
        }

        void SwitchPause(InputAction.CallbackContext ctx)
        {
            if (_stateManager.GetCurrentState() == GameState.Title)
            {
                _stateManager.ToMain();
                _audioManager.PlayBGM();
                // 操作方法の表示
                UniTask.Void(async () =>
                {
                    _uiManager.SwitchGuideShowing(true);
                    await UniTask.Delay(TimeSpan.FromSeconds(5.0), cancellationToken: destroyCancellationToken);
                    _uiManager.SwitchGuideShowing(false);
                });
                return;
            }

            if (_stateManager.GetCurrentState() == GameState.Main)
            {
                _isPause = !_isPause;
                Time.timeScale = _isPause ? 0.0f : 1.0f;
                _uiManager.SetPause(_isPause);
                // カメラに写っているバブルの描画を消す
                foreach (var x in FindObjectsOfType<BubbleBehaviour>())
                {
                    foreach (var y in x.GetComponentsInChildren<Renderer>())
                    {
                        y.enabled = !_isPause;
                    }
                }
                // BGMの再生・停止
                if (_isPause)
                {
                    _audioManager.PauseBGM();
                }
                else
                {
                    _audioManager.PlayBGM();
                }
                return;
            }

            if (_stateManager.GetCurrentState() == GameState.Result)
            {
                Time.timeScale = 1.0f; // 停止させたカニを戻す
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // シーンを戻す
                return;
            }
        }

        void Update()
        {
            _timer += Time.deltaTime;
            var height = _crab.transform.position.y - _defaultHeight;
            _maxHeight = Mathf.Max(_maxHeight, height);
            _uiManager.SetHeight(height, _crabRigidbody.linearVelocity.y < 0 && height > 1.0F);
            _uiManager.SetMaxHeight(_maxHeight);
            _uiManager.SetTime(_timer);
            _uiManager.SetHeightRatio(height / GoalHeight);
            // 終了判定
            if (height >= GoalHeight && _stateManager.GetCurrentState() != GameState.Result)
            {
                Finish();
            }
            // 高さと音量を連動
            _audioManager.SetBGMVolume(CalcBgmVolume(height));
        }

        float CalcBgmVolume(float currentHeight)
        {
            var maxVolumeHeight = 0.9F * GoalHeight;
            var minVolumeHeight = 0.25F * GoalHeight;
            if (currentHeight < minVolumeHeight)
            {
                return 0.0F;
            }
            if (currentHeight > maxVolumeHeight)
            {
                return 1.0F;
            }
            return (currentHeight - minVolumeHeight) / (maxVolumeHeight - minVolumeHeight);
        }

        void Finish()
        {
            Time.timeScale = 0.0f; // カニを停止
            _stateManager.ToResult();
            _uiManager.ShowResult(_timer);
        }
    }
}
