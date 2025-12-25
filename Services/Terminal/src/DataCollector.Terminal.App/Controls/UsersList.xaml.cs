using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.Controls;

public partial class UsersList : ContentView
{
    public static readonly BindableProperty UsersSourceProperty =
        BindableProperty.Create(nameof(Users), typeof(IEnumerable<UserDTO>), 
            typeof(UsersList), Enumerable.Empty<UserDTO>(), BindingMode.OneWay);

    public static readonly BindableProperty SelectedUsersProperty =
    BindableProperty.Create(nameof(SelectedUsers), typeof(IEnumerable<UserDTO>),
        typeof(UsersList), Enumerable.Empty<UserDTO>(), BindingMode.OneWayToSource);

    public IEnumerable<UserDTO> Users
    {
        get => (IEnumerable<UserDTO>)GetValue(UsersSourceProperty);

        set => SetValue(UsersSourceProperty, value);
    }

    public IEnumerable<UserDTO> SelectedUsers => (IEnumerable<UserDTO>)GetValue(SelectedUsersProperty);

	public UsersList()
	{
		InitializeComponent();
	}
}