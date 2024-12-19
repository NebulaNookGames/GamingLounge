using UnityEngine;

/// <summary>
/// The base class for all data handlers. Data handler methods recieve or send information to the current savedata, using that data and setting values at runtime.
/// </summary>
public abstract class DataHandler : MonoBehaviour
{
    /// <summary>
    /// A abstract method to get data from a savedata class. 
    /// </summary>
    /// <param name="saveData"></param> The data object given to set values.
    public abstract void ReceiveData(SaveData saveData);

    /// <summary>
    /// A abstract method to send data to the savedata class. 
    /// </summary>
    /// <param name="saveData"></param> The data object which to send information to. 
    public abstract void SendData(SaveData saveData);
  
}