using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CyberPunkCoding
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField]private CommandUiContainer commandUiContainer;

        private bool playing = false;
        protected void Awake()
        {
            button.onClick.AddListener(
                () =>
                {
                    if (playing)
                        return;
                    bool success = Pawn.Instance.FollowCommand(commandUiContainer.GetCommands(), OnCommandsFollowed);
                    if (success) playing = true;
                }
            );
        }

        private void OnCommandsFollowed()
        {
            bool win = true;
            Objective[] objectives = FindObjectsOfType<Objective>();
            foreach (Objective objective in objectives)
            {
                if (!objective.Valid)
                {
                    win = false;
                    break;
                }
            }

            if (win)
            {
                print("<color=green>WON</color>");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                foreach (Objective objective in objectives) objective.Reset();
                playing = false;
            }
        }
    }
    
}