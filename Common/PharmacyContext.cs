using System.Security.Claims;

public class UserContext
{
    private readonly IHttpContextAccessor _http;

    public UserContext(IHttpContextAccessor http)
    {
        _http = http;
    }

    public int PharmacyId
    {
        get
        {
            var pharmacyId = _http.HttpContext?.User?.FindFirst("PharmacyId")?.Value;
            if (string.IsNullOrWhiteSpace(pharmacyId))
            {
                throw new InvalidOperationException("PharmacyId claim is missing.");
            }

            return int.Parse(pharmacyId);
        }
    }
}
