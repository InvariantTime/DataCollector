using CommunityToolkit.Maui.Extensions;

using DataCollector.Terminal.App.Controls;
using DataCollector.Terminal.App.Forms;

namespace DataCollector.Terminal.App.Pages;

public partial class AdminPage : ContentPage
{


    public List<User> Users { get; private set; }

    public AdminPage()
    {
        InitializeComponent();
        Users = new List<User>
        {
            new User("Taras",20, Roles.Worker), new User("dad Taras", 50, Roles.Worker), new User("friends Taras", 100, Roles.Worker)
        };
        BindingContext = this;
    }

    void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {


        // statusCheckBox.Text = $"—татус: {(e.Value ? "женат/замужем" : "холост/не замужем")}";
    }

    public void OnAdminAction(object sender, EventArgs e)
    {
    }
}

public class User
{
    public string Name { get; }

    public int Age { get; }

    public Roles Role { get; }


    public User(string name, int age, Roles role)
    {
        Name = name;
        Age = age;
        Role = role;
    }

}

public enum Roles
{
    Admin,
    Publisher,
    Worker
}