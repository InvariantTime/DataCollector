using DataCollector.Domain;
using DataCollector.Terminal.App.Commands;
using DataCollector.Terminal.App.DTOs;
using System.Collections.ObjectModel;

namespace DataCollector.Terminal.App.ViewModels;

public class AdminPageViewModel : ViewModel
{
    public ObservableCollection<UserDTO> Users { get; }

    public AdminPageViewModel()
    {
        Users = new ObservableCollection<UserDTO>()
        {
            new UserDTO(Guid.NewGuid(), "Taras", UserRoles.User),
            new UserDTO(Guid.NewGuid(), "Vasya", UserRoles.Publisher),
            new UserDTO(Guid.NewGuid(), "Petya", UserRoles.Publisher),
            new UserDTO(Guid.NewGuid(), "Grisha", UserRoles.Admin)
        };
    }
}
