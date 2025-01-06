using System.ComponentModel.DataAnnotations;

public class VisitorLog
{
    [Key]
    public int Id { get; set; } // Primary key

    public string UserIdentifier { get; set; } // Unique identifier for tracking users

    public DateTime VisitDate { get; set; } = DateTime.Now; // Timestamp of the visit

    public int ProductsPageVisits { get; set; } // URL of the page visited
    public int CollectionsPageVisits { get; set; } // URL of the page visited

    public string Referrer { get; set; } // URL or source from where the visitor came

    public bool IsConverted { get; set; } = false; // Indicates if the visitor made a purchase
}
