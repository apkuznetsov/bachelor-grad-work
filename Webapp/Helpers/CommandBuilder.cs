namespace Webapp.Helpers
{
    public static class CommandBuilder
    {
        public static string BuildSensorListenerStartCommand(int testId, string sensorIpAddress, int sensorPort, int durationSeconds)
        {
            return $"-testId {testId} -executionTime {durationSeconds} -sensors {sensorIpAddress}:{sensorPort}";
        }

        public static string BuildSensorOutputParserCommand(
            int testId, 
            string leftTimeBorder, string rightTimeBorder, 
            string sensorIpAddress, int sensorPort, 
            string dirPath)
        {
            return $"-directoryPath {dirPath} " +
                $"-leftTimeBorder {leftTimeBorder} " +
                $"-rightTimeBorder {rightTimeBorder} " +
                $"-testId {testId} " +
                $"-sensors {sensorIpAddress}:{sensorPort}";
    }
}
}
