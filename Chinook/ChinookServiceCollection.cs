using Chinook.Services;

namespace Chinook
{
    public static class ChinookServiceCollection
    {
        public static void AddChinookServices(this IServiceCollection services)
        {
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<ITrackService, TrackService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
        }
    }
}
