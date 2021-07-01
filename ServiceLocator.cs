using System;

//Static class to allow the other classes in the project to find the Temperature Control manager without needing to hold a reference to it
public static class ServiceLocator
{
    static ITemperatureControl temperatureControl;

    public static void Provide(ITemperatureControl tempControl)
    {
        temperatureControl = tempControl;
    }

    public static ITemperatureControl GetTemperatureControl()
    {
        return temperatureControl;
    }

    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException()
        {

        }

        public ServiceNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
    }


}
