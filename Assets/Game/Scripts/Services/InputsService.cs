using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "InputsService", menuName = "Services/InputsService")]
public class InputsService : GameService
{
    private GameInputs _gameInputs;
    
    public override IEnumerator Run() { _gameInputs = new GameInputs(); _gameInputs.Enable(); yield break; }
    
    public GameInputs GetGameInputs() => _gameInputs;

    public override void Stop() { _gameInputs.Disable(); _gameInputs = null; }
}