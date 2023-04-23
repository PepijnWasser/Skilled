using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSetting : MonoBehaviour
{
    public abstract void SetVisualToSaved();

    public abstract void RestoreToDefault();

}
