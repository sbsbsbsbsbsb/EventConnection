namespace Core.Providers
{
    //TODO: implement logic
    public static class ConferenceManager
    {
        public static int GetConferenceId()
        {
            return (int) ConfigManager.GetConferenceId();
        }
    }
}
