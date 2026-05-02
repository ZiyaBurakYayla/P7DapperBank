namespace P7DapperBank.Models
{
    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalAccounts { get; set; }
        public int TotalBanks { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalAmount { get; set; }
        public List<TypeStatItem> TypeStats { get; set; } = new();
        public List<MonthlyStatItem> MonthlyStats { get; set; } = new();
        public List<RecentTransactionItem> RecentTransactions { get; set; } = new();
    }

    public class TypeStatItem
    {
        public string TypeName { get; set; }
        public int Count { get; set; }
    }

    public class MonthlyStatItem
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }
        public decimal Total { get; set; }
        public string Label => $"{Year}/{Month:D2}";
    }

    public class RecentTransactionItem
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public string TypeName { get; set; }
    }
}
