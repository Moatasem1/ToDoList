namespace Application.Options;

public record MyDataTableResponse<T>(int Draw,
    int RecordsTotal,
    int RecordsFiltered,
    IEnumerable<T> Data);
