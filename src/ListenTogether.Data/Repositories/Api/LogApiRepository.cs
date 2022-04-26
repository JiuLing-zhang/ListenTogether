using ListenTogether.Data.Interfaces;
using ListenTogether.Model;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;

namespace ListenTogether.Data.Repositories.Api;

public class LogApiRepository : ILogRepository
{
    public async Task<bool> WriteListAsync(List<Log> logs)
    {
        var requestLogs = new List<LogRequest>();
        foreach (var log in logs)
        {
            requestLogs.Add(new LogRequest()
            {
                Timestamp = log.Timestamp,
                LogType = log.LogType,
                Message = log.Message
            });
        }
        string content = requestLogs.ToJson();
        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await DataConfig.HttpClientWithToken.PostAsync(DataConfig.ApiSetting.WriteLog, sc);
        var json = await response.Content.ReadAsStringAsync();
        var obj = json.ToObject<Result>();
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }
}