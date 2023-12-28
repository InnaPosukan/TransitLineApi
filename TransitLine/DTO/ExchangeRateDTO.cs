using System.Collections.Generic;
using Newtonsoft.Json;

public class ExchangeRateResponse
{
    [JsonProperty("result")]
    public string Result { get; set; }

    [JsonProperty("conversion_rates")]
    public Dictionary<string, decimal> ConversionRates { get; set; }
}
