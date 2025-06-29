using Cysharp.Threading.Tasks;
using Nova;
using UI.Interfaces;
using UnityEngine;

namespace UI.Views
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        [SerializeField] private UIBlock uiBlock;

        protected UIBlock UiBlock => uiBlock;

        private void OnValidate()
        {
            if(!TryGetComponent(out uiBlock))
            {
                uiBlock = transform.parent.GetComponent<UIBlock>();
            }
        }

        protected readonly UICommandBuilder ShowCommandsBuilder = new();
        protected readonly UICommandBuilder HideCommandsBuilder = new();

        public virtual async UniTask Show()
        {
            BeforeShow();
            gameObject.SetActive(true);
            ShowCommandsBuilder.AddShow(UiBlock);
            await ExecuteShowCommand();
            AfterShow();
        }
        protected virtual void BeforeShow() { }
        protected virtual void AfterShow() { }
        private async UniTask ExecuteShowCommand()
        {
            var command = ShowCommandsBuilder.Build();

            if(command != null)
            {
                await command.Execute();
                
            }
        }

        public virtual async UniTask Hide()
        {
            BeforeHide();
            HideCommandsBuilder.AddHide(UiBlock);
            await ExecuteHideCommand();
            AfterHide();
        }

        protected virtual void BeforeHide() { }
        protected virtual void AfterHide() { }

        private async UniTask ExecuteHideCommand()
        {
            var command = HideCommandsBuilder.Build();
            if (command != null) await command.Execute();
        }
    }
}