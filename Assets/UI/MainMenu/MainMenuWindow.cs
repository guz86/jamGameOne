using System;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {

        private Action _closeAction;
        
        public void OnShowSettings()
        {
            
        }
        
        public void OnStartGame()
        {
            //SceneManager.LoadScene("Level1");
            _closeAction = () => { SceneManager.LoadScene("Level1"); };
            Close();
        }
        
        public void OnExit()
        {
            //Close();
            _closeAction = () =>
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
            
            
        }
        public override void OnCloseAnimationComplete()
        {
            base.OnCloseAnimationComplete();
            _closeAction?.Invoke();
             
        }
    }
}