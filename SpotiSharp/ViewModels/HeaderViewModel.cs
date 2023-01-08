using System.Windows.Input;

namespace SpotiSharp.ViewModels;

public class HeaderViewModel : BaseViewModel
{
    public HeaderViewModel()
    {
        ItemClicked = new Command(SwitchToPage);
    }

    private async void SwitchToPage(object parameter)
    {
        await Shell.Current.GoToAsync($"//{(string)parameter}");
    }

    public ICommand ItemClicked { private set; get; }

}