namespace warsztat2api;

public class ExchangeRatesTable
{
    public string Table { get; set; }
    public string No { get; set; }
    public DateTime EffectiveDate { get; set; }
    public List<ExchangeRate> Rates { get; set; }
    
}