namespace GeoHub.GeoNamesClient
{
    public interface IGeoNamesEntryNameBuilder
    {
        string BuildName(GeonameEntry geonameEntry);
    }
}