namespace WoodseatsScouts.Coins.Api.AppLogic;

public class SystemDateTimeProvider
{
    private Func<DateTime> dateTimeProviderFunc = () => DateTime.Now;

    public DateTime Now => dateTimeProviderFunc.Invoke();
    
    public void SetDateTimeToSetTime(DateTime dateTime)
    {
        dateTimeProviderFunc = () => dateTime;
    }

    public void SetDateTimeToSystemClock()
    {
        dateTimeProviderFunc = () => DateTime.Now;
    }
}