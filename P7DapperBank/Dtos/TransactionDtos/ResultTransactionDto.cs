namespace P7DapperBank.Dtos.TransactionDtos
{
    public class ResultTransactionDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string? CustomerName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public int TypeId { get; set; }
        public string Status { get; set; }
        public int SenderBankId { get; set; }
        public int ReceiverBankId { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal BalanceAfter { get; set; }
    }
}
