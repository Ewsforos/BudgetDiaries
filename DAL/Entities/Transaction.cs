using System;

namespace DAL.Entities
{
    public class Transaction : BaseEntity
    {
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }

        public bool IsRepeatable { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string RepeatType { get; set; }

        //Внешние ключи
        public Guid FK_Account { get; set; }
        public Guid FK_Categoty { get; set; }
    }
}
