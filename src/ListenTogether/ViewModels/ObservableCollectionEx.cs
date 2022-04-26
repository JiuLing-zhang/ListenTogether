using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ListenTogether.ViewModels;

public class ObservableCollectionEx<T> : ObservableCollection<T> where T : INotifyPropertyChanged
{
    public ObservableCollectionEx(IEnumerable<T> initialData) : base(initialData)
    {
        Init();
    }

    public ObservableCollectionEx()
    {
        Init();
    }

    private void Init()
    {
        foreach (T item in Items)
        {
            item.PropertyChanged += ItemOnPropertyChanged;
        }
        CollectionChanged += ObservableCollectionEx_CollectionChanged;
    }
    private void ObservableCollectionEx_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (T item in e.NewItems)
            {
                if (item != null)
                    item.PropertyChanged += ItemOnPropertyChanged;
            }
        }

        if (e.OldItems != null)
        {
            foreach (T item in e.OldItems)
            {
                if (item != null)
                    item.PropertyChanged -= ItemOnPropertyChanged;
            }
        }
    }

    private void ItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        => ItemChanged?.Invoke(sender, e);

    public event PropertyChangedEventHandler ItemChanged;
}
