using System;
using DG.Tweening;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.GamePlay.Character.Base.Character_UI
{
    public class ReviveTimer : MonoBehaviour
    {
        [BoxGroup("Revive Timer Settings"), LabelText("Revive Timer Image"), Required] [SerializeField]
        private Image reviveTimerImage;

        [BoxGroup("Revive Timer Settings"), LabelText("Revive Timer Text"), Required] [SerializeField]
        private TextMeshProUGUI reviveTimerText;

        [BoxGroup("Revive Timer Settings"), LabelText("Ease")] [SerializeField]
        private Ease ease = Ease.OutSine;

        [BoxGroup("Revive Timer Settings"), LabelText("Text Ease")] [SerializeField]
        private Ease textEase = Ease.OutSine;

        private IRevivableInTime _reviveableInTime;

        private void Awake()
        {
            reviveTimerImage.gameObject.SetActive(false);
        }

        [Inject]
        public void Construct(IRevivableInTime revivableInTime)
        {
            _reviveableInTime = revivableInTime;
            _reviveableInTime.OnReviveStart.Subscribe(StartReviveTimer).AddTo(this);
            _reviveableInTime.OnReviveFinished.Subscribe(FinishReviveTimer).AddTo(this);
        }

        private void StartReviveTimer(float timer)
        {
            reviveTimerImage.fillAmount = 1;
            reviveTimerImage.DOKill();
            reviveTimerImage.gameObject.SetActive(true);
            reviveTimerImage.DOFillAmount(0, timer).SetEase(ease);
            reviveTimerText.DOCounter((int)timer, 0, timer).SetEase(textEase);
        }

        private void FinishReviveTimer(Unit _)
        {
            reviveTimerImage.DOKill();
            reviveTimerImage.gameObject.SetActive(false);
            reviveTimerImage.fillAmount = 1;
        }
    }
}