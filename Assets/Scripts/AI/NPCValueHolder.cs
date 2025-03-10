using UnityEngine;

public class NPCValueHolder : MonoBehaviour
{
    #region Variables

    [Header("NPC Values")]
    [Tooltip("The NPC values associated with this NPC.")]
    [SerializeField] private NPCValues values; // The NPC values (e.g., random index, color, etc.)

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or sets the NPC values.
    /// </summary>
    public NPCValues Values
    {
        get { return values; }
        set { values = value; }
    }

    #endregion Properties
}