using SpotiSharp.Models;

namespace SpotiSharp.ViewModels.Filters;

public delegate void FilterRemove(int index);

public class BaseFilter : BaseViewModel
{
    public static event FilterRemove OnFilterRemove;

    internal void InvokeRemoveEvent(int index)
    {
        OnFilterRemove?.Invoke(index);
        PlaylistCreatorPageModel.Filters.RemoveAt(index);
    }
}