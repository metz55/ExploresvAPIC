namespace ExploresvAPIC.Endpoints
{
    public static class Startup
    {
        public static void UseEndpoints(this WebApplication app)
        {
            RoleEndpoints.Add(app);
            StatusEndpoints.Add(app);
            UserEndpoints.Add(app);
        }
    }
}