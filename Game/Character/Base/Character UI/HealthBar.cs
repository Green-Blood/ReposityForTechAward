using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.Factories.Interfaces;
using Sirenix.OdinInspector;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.GamePlay.Character.Base.Character_UI
{
    public class HealthBar : MonoBehaviour
    {
        [BoxGroup("Health Bar Settings"), LabelText("Health Bar Image"), Required] [SerializeField]
        private Image healthBarImage;

        [BoxGroup("Health Bar Settings"), LabelText("Update Duration (s)"), MinValue(0.1f)] [SerializeField]
        private float healthBarUpdateTime = 0.5f;

        [BoxGroup("Health Bar Settings"), LabelText("Ease")] [SerializeField]
        private Ease ease = Ease.OutSine;

        [SerializeField] private CanvasGroup canvasGroup;

        protected Vital Health;
        [SerializeField] private float fadeDuration = 0.35f;
        [SerializeField] private Ease fadeEase = Ease.InOutCubic;

        [Inject]
        public void Construct(IStatCollection characterStatCollection, IShared shared)
        {
            Health = characterStatCollection.TryGetStat<Vital>(StatType.Health);
            Health.OnCurrentValueChange += OnHealthChangedAction;
            shared.UnitDeath.IsDead.Subscribe(OnCharacterDeath).AddTo(this);
        }

        protected void OnCharacterDeath(bool isDead)
        {
            if(isDead)
            {
                canvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEase);
            }
            else
            {
                canvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEase);
            }
        }

        protected void OnHealthChangedAction(object sender, EventArgs eventArgs)
        {
            var health = (Vital)sender;
            float fillAmount = health.StatCurrentValue / health.StatValue;

            UpdateHealthBar(fillAmount).Forget();
        }

        private async UniTask UpdateHealthBar(float fillAmount)
        {
            healthBarImage.DOKill();
            await healthBarImage
                .DOFillAmount(fillAmount, healthBarUpdateTime)
                .SetEase(ease)
                .AsyncWaitForCompletion();
        }

        private void OnDestroy()
        {
            if(Health != null)
                Health.OnCurrentValueChange -= OnHealthChangedAction;
        }
    }
}