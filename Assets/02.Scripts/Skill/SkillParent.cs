using UnityEngine;

public abstract class SkillParent : MonoBehaviour
{
    protected int _level=1;
    [Header("각성을 까지 레벨")][SerializeField]protected int _levelForAwakening;

    public void LevelUp()
    {
        _level++;
        ExcuteLevelUp();
        if (_level > _levelForAwakening)
        {
            Awakening();
        }
    }

    protected abstract void ExcuteLevelUp();
    protected abstract void Awakening();
}
