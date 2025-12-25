using DataCollector.Terminal.App.DTOs;
using System.Collections;

namespace DataCollector.Terminal.App.Controls;

public partial class UsersList : ContentView
{
    public static readonly BindableProperty UsersProperty =
        BindableProperty.Create(nameof(Users), typeof(IEnumerable), 
            typeof(UsersList), Enumerable.Empty<UserDTO>(), BindingMode.OneWay);

    public static readonly BindableProperty SelectedUsersProperty =
    BindableProperty.Create(nameof(SelectedUsers), typeof(IEnumerable<UserDTO>),
        typeof(UsersList), Enumerable.Empty<UserDTO>(), BindingMode.OneWayToSource);

    public IEnumerable<UserDTO> Users
    {
        get => (IEnumerable<UserDTO>)GetValue(UsersProperty);

        set => SetValue(UsersProperty, value);
    }

    public IEnumerable<UserDTO> SelectedUsers => (IEnumerable<UserDTO>)GetValue(SelectedUsersProperty);

	public UsersList()
	{
		InitializeComponent();
	}
}