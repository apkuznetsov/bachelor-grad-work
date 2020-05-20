namespace Webapp.Helpers
{
    public static class CommandBuilder
    {
        public static string BuildSensorListenerStartCommand(int testId, string sensorIpAddress, int sensorPort, int durationSeconds)
        {
            return $"-testId {testId} -executionTime {durationSeconds} -sensors {sensorIpAddress}:{sensorPort}";
        }
    }
}
