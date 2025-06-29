using NovaSamples.UIControls;
using UnityEngine;

namespace UI.Views
{
    public class ButtonAnimation : MonoBehaviour
    {
        [SerializeField] private Button button;

        private bool _isShown;
        public Button Button => button;

        public void ToggleButton()
        {
            _isShown = !_isShown;
            button.gameObject.SetActive(_isShown);
            
            //Animate here whatever
        }
    }
}