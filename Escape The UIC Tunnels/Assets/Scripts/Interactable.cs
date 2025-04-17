public interface IInteractable
{
    string GetPrompt();             // e.g. "Grab [E]" or "Enter [E]"
    void OnInteract();              // What happens on E press
}
