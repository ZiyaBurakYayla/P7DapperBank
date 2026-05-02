namespace P7DapperBank.Dtos.TransactionDtos
{
    public class FilterTransactionDto
    {
        public string? CustomerName { get; set; }
        public string? AccountNumber { get; set; }
        public string? Status { get; set; }
        public string? Currency { get; set; }
        public int? TypeId { get; set; }
        public decimal? AmountMin { get; set; }
        public decimal? AmountMax { get; set; }
    }
}
