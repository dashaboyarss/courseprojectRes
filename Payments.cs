using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace курсач3
{
    public class Payments :Plan
    {

        public int Time
        {
            get => time;
            set
            {
                if (value > 0) time = value;
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Срок достижения цели'! Введите целое положительное число");
                    time = -1;
                    isCorrect = false;
                }

            }
        }

        public Payments(string name, int goalAmount, double startAmount, string frequency, double incomePercent, double investIncome, int time, double inflation) : base(name, goalAmount, startAmount, frequency, incomePercent, investIncome, inflation)
        {
            this.Time = time;
            this.remainingTime = time;
        }

        public void PaymentAmount()
        {
            if (this.Frequency == "" || this.amountWithInflation == -1 || this.investIncome == -1 || this.StartAmount == -1)
            {
                this.paymentAmount = -1;
            }
            else
            {
                double payment = 0;
                double paymentSum = this.amountWithInflation - this.investIncome - this.StartAmount - this.StartInvestAmount;

                if (Frequency == "раз в неделю")
                {
                    payment = paymentSum / this.Time;
                }
                else if (Frequency == "раз в 2 недели")
                {
                    payment = paymentSum / this.Time * 2;
                }
                else if (Frequency == "раз в месяц")
                {
                    payment = paymentSum / this.Time * 4;
                }
                else if (Frequency == "раз в 3 месяца")
                {
                    payment = paymentSum / this.Time * 12;
                }
                else if (Frequency == "раз в полгода")
                {
                    payment = paymentSum / this.Time * 24;
                }
                else if (Frequency == "раз в год")
                {
                    payment = paymentSum / this.Time * 48 ;
                }
                this.paymentAmount = payment;
                this.startPaymentAmount = payment;
            }

        }
    }
}
