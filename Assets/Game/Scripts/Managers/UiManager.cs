using System.Collections;

public class UiManager : SceneManager
{
    public override IEnumerator Initialize()
    {
        G.GetState<CurrentEntities>().OnChange += UpdateUi;
        yield break;
    }

    private void UpdateUi(EntityConfig config)
    {
        
    }

    public override void Deinitialize()
    {
        G.GetState<CurrentEntities>().OnChange -= UpdateUi;
    }
}