using DataCollector.Terminal.App.DTOs;
using System.Collections;

namespace DataCollector.Terminal.App.Controls;

public partial class UsersList : ContentView
{
    public static readonly BindableProperty UsersProperty =
        BindableProperty.Create(nameof(Users), typeof(IEnumerable),
            typeof(UsersList), Enumerable.Empty<UserDTO>(), BindingMode.OneWay);

    public static readonly BindableProperty SelectedUsersProperty =
        BindableProperty.Create(nameof(SelectedUsers), typeof(IEnumerable),
            typeof(UsersList), Enumerable.Empty<UserDTO>(), BindingMode.TwoWay);

    public static readonly BindableProperty SelectedUserProperty =
        BindableProperty.Create(nameof(SelectedUsers), typeof(UserDTO),
            typeof(UsersList), null, BindingMode.OneWayToSource);

    public IEnumerable<UserDTO> Users
    {
        get => (IEnumerable<UserDTO>)GetValue(UsersProperty);

        set => SetValue(UsersProperty, value);
    }

    public IEnumerable<UserDTO> SelectedUsers
    {
        get => (IEnumerable<UserDTO>)GetValue(SelectedUsersProperty);

        set => SetValue(SelectedUsersProperty, value);
    }

    public UserDTO? SelectedUser
    {
        get => (UserDTO?)GetValue(SelectedUserProperty);

        private set => SetValue(SelectedUserProperty, value);
    }

    public UsersList()
    {
        InitializeComponent();
    }

    private void OnLongPressed(object sender, CommunityToolkit.Maui.Core.LongPressCompletedEventArgs e)
    {
        _list.SelectionMode = _list.SelectionMode switch
        {
            SelectionMode.Multiple => SelectionMode.Single,
            SelectionMode.Single => SelectionMode.Multiple,
            _ => SelectionMode.Multiple
        };
    }

    private void OnItemTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter == null || e.Parameter is not Guid id)
            return;

        if (_list.SelectionMode == SelectionMode.Multiple)
            return;

        if (SelectedUser != null && SelectedUser.Id == id)
        {
            SelectedUser = null;
            return;
        }

        
    }
}