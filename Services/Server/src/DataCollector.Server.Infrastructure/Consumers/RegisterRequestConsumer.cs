using DataCollector.Server.Infrastructure.DTOs;
using DataCollector.Server.Infrastructure.Sessions;
using DataCollector.Server.Services.Interfaces;

namespace DataCollector.Server.Infrastructure.Consumers;

public class RegisterRequestConsumer
{
    private readonly ISessionService _sessions;
    private readonly IUserService _users;

    public RegisterRequestConsumer(ISessionService sessions, IUserService users)
    {
        _sessions = sessions;
        _users = users;
    }

    public void Consume(RegisterSessionDTO dto)
    {
        var loginResult = _users.TryLogIn(dto.Name, dto.Password);



    }
}
