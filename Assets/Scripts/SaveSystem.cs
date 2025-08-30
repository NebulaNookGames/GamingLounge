using System.Collections.Generic;
using System.Linq;

public class SaveSystem<T> where T : new()
{
    protected List<T> _savedData;

    public delegate void LoadingCompleteHandler();
    public event LoadingCompleteHandler LoadingComplete;

    private bool _isReady;

    public SaveSystem(string filename)
    {
    }

    virtual public void Save()
    {

    }

    virtual public void Load()
    {
        LoadingCompleted();
    }

    protected void LoadingCompleted()
    {
        if (LoadingComplete != null)
            LoadingComplete();

        _isReady = true;
    }

    public T GetData(int index = 0)
    {
        if(_savedData == null)
            _savedData = new List<T>();

        if (index >= _savedData.Count)
            _savedData.AddRange(Enumerable.Repeat(new T(), index + 1 - _savedData.Count));

        return _savedData[index];
    }

    public bool IsReady
    {
        get { return _isReady; }
    }

    public virtual bool IsSaving()
    {
        return false;
    }
}