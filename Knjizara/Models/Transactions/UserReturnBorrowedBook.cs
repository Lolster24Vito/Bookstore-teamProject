using System.ComponentModel.DataAnnotations;
namespace Knjizara.Models.Transactions
{
    public class UserReturnBorrowedBook
    {
        [Key]
        public int Id { get; set; }

        public BookUserBorrow Borrow { get; set; }

        public DateTime ReturnedDate { get; set; }

        public int DaysLate { get; set; }

        public void CalculateDaysLate()
        {
            if (ReturnedDate <= Borrow.ReturnOnDate)
            {
                DaysLate = 0;
            }
            else 
            {
                int daysLate = (ReturnedDate - Borrow.ReturnOnDate).Value.Days;
                DaysLate = daysLate;
            }
            Borrow.User.DaysLate += DaysLate;
            Borrow.User.CalculateLateFee();
        }
    }
}
