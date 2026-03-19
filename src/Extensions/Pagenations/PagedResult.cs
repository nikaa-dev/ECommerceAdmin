namespace src.Extensions;

public class PagedResult<T>
{
    /// <summary>
    /// The items for the current page.
    /// </summary>
    public List<T> Items { get; }

    // --- Pagination Metadata ---

    /// <summary>
    /// The current page number.
    /// </summary>
    private int PageNumber { get; }

    /// <summary>
    /// The number of items per page.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// The total number of items across all pages.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// The total number of pages available.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Indicates if there is a previous page.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indicates if there is a next page.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;


    public PagedResult(List<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}