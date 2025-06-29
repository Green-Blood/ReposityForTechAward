using System.Collections.Generic;
using Game.Core.Commands.Interfaces;
using Nova;
using UI.Configs;
using UI.Views.ViewCommands;
using UnityEngine;

namespace UI.Views
{
    public class UICommandBuilder
    {
        private readonly List<ICommand> _commands = new();

        public UICommandBuilder AddFade(UIBlock uiBlock, FadeAnimationComponent component, bool shouldWait = false)
        {
            var command = new FadeCommand(uiBlock, component.FadeStartValue, component.FadeEndValue, component.Duration, component.Ease, component.Delay, shouldWait);
            _commands.Add(command);
            return this;
        }

        public UICommandBuilder AddShow(UIBlock uiBlock)
        {
            var command = new ShowCommand(uiBlock);
            _commands.Add(command);
            return this;
        }

        public UICommandBuilder AddSlide(UIBlock uiBlock, Vector3 startPos, Vector3 endPos, float duration)
        {
            var command = new SlideCommand(uiBlock, startPos, endPos, duration);
            _commands.Add(command);
            return this;
        }

        public UICommandBuilder AddScale(UIBlock uiBlock, ScaleAnimationComponent component, bool shouldWait)
        {
            var command = new ScaleCommand(uiBlock, component.ScaleStartValue, component.ScaleEndValue, component.Duration, component.Ease, component.Delay, shouldWait);
            _commands.Add(command);
            return this;
        }

        public UICommandBuilder AddHide(UIBlock uiBlock)
        {
            var command = new HideCommand(uiBlock);
            _commands.Add(command);
            return this;
        }

        public ICommand Build()
        {
            var sequentialCommand = new SequentialCommand(_commands);
            return sequentialCommand.CanExecute() ? sequentialCommand : null;
        }
    }
}