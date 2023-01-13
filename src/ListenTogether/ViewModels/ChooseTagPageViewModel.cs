using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(AllTypesJson), nameof(AllTypesJson))]
public partial class ChooseTagPageViewModel : ViewModelBase
{
    public string AllTypesJson { get; set; }

    [ObservableProperty]
    private ObservableCollection<MusicTypeTagViewModel> _allTypes;

    public ChooseTagPageViewModel()
    {
    }

    public Task InitializeAsync()
    {
        AllTypes = new ObservableCollection<MusicTypeTagViewModel>();

        var allTypes = AllTypesJson.ToObject<List<MusicTypeTag>>();

        foreach (var types in allTypes)
        {
            var typesItem = new MusicTypeTagViewModel();
            typesItem.TypeName = types.TypeName;

            var typeTags = new ObservableCollection<MusicTagViewModel>();
            foreach (var typeTag in types.Tags)
            {
                typeTags.Add(new MusicTagViewModel()
                {
                    Id = typeTag.Id,
                    Name = typeTag.Name,
                });
            }
            typesItem.Tags = typeTags;
            AllTypes.Add(typesItem);
        }
        return Task.CompletedTask;
    }

    [RelayCommand]
    public async void TagSelectedAsync(string id)
    {
        await Shell.Current.GoToAsync($"..?TagId={id}", true);
    }
}