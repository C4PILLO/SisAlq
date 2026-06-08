namespace SisAlq.Api.Features.Inquilinos;

public static class ConsultarDocumento
{
    private record DniResponse(
        bool Success,
        string? Nombres,
        string? ApellidoPaterno,
        string? ApellidoMaterno
    );

    private record RucResponse(
        string? RazonSocial,
        string? Estado,
        string? Condicion
    );

    public record Response(
        string NumeroDocumento,
        string Tipo,
        string? NombreCompleto,
        bool Valido,
        string? Mensaje
    );

    public static async Task<IResult> Handle(
        string numero,
        string tipo,
        IConfiguration config,
        IHttpClientFactory httpFactory)
    {
        var tipoUpper = tipo.ToUpper().Trim();
        var token     = config["ApisPeruToken"];
        var http      = httpFactory.CreateClient("ApisPeruClient");

        // CE — solo validación de formato
        if (tipoUpper == "CE")
        {
            var ceValido = numero.Length >= 9 && numero.Length <= 12;
            return Results.Ok(new Response(numero, "CE", null, ceValido,
                ceValido ? null : "CE debe tener entre 9 y 12 caracteres"));
        }

        // DNI
        if (tipoUpper == "DNI")
        {
            if (numero.Length != 8 || !numero.All(char.IsDigit))
                return Results.BadRequest(new Response(numero, "DNI", null, false,
                    "DNI debe tener 8 dígitos numéricos"));
            try
            {
                var url = $"https://dniruc.apisperu.com/api/v1/dni/{numero}?token={token}";
                var dni = await http.GetFromJsonAsync<DniResponse>(url);

                if (dni is null || !dni.Success)
                    return Results.Ok(new Response(numero, "DNI", null, false,
                        "DNI no encontrado en el padrón SUNAT"));

                var nombre = $"{dni.ApellidoPaterno} {dni.ApellidoMaterno} {dni.Nombres}".Trim();
                return Results.Ok(new Response(numero, "DNI", nombre, true, null));
            }
            catch
            {
                return Results.Ok(new Response(numero, "DNI", null, true,
                    "Servicio no disponible — ingrese el nombre manualmente"));
            }
        }

        // RUC
        if (tipoUpper == "RUC")
        {
            if (numero.Length != 11 || !numero.All(char.IsDigit))
                return Results.BadRequest(new Response(numero, "RUC", null, false,
                    "RUC debe tener 11 dígitos numéricos"));
            try
            {
                var url = $"https://dniruc.apisperu.com/api/v1/ruc/{numero}?token={token}";
                var ruc = await http.GetFromJsonAsync<RucResponse>(url);

                if (ruc is null || string.IsNullOrEmpty(ruc.RazonSocial))
                    return Results.Ok(new Response(numero, "RUC", null, false,
                        "RUC no encontrado en SUNAT"));

                if (ruc.Estado != "ACTIVO" || ruc.Condicion != "HABIDO")
                    return Results.Ok(new Response(numero, "RUC", ruc.RazonSocial, false,
                        $"RUC no válido — Estado: {ruc.Estado}, Condición: {ruc.Condicion}"));

                return Results.Ok(new Response(numero, "RUC", ruc.RazonSocial, true, null));
            }
            catch
            {
                return Results.Ok(new Response(numero, "RUC", null, true,
                    "Servicio no disponible — ingrese la razón social manualmente"));
            }
        }

        return Results.BadRequest(new Response(numero, tipo, null, false,
            "Tipo de documento no válido. Use: DNI, RUC o CE"));
    }
}
