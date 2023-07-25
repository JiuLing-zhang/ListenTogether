using ListenTogether.Model;

namespace ListenTogether.Service.Interface;
public interface ILogManage
{
    Task<List<Log>> GetAllAsync();
    Task RemoveAllAsync();
}