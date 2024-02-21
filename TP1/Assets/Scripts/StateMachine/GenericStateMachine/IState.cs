
public interface IState
{
    public void OnStart();
    public void OnEnter();
    public void OnExit();
    public void OnUpdate();
    public void OnFixedUpdate();
    public bool CanEnter(IState currentState);
    public bool CanExit();
}
