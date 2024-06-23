using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    [TextArea(3, 5)]
    private string[] _texts;

    public void StartDialogue()
    {
        DialogueManager.Instance.StartDialogue(_texts);
    }
    
}
