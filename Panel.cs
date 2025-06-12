using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static курсач3.Form1;

namespace курсач3
{
    public class Panel
    {
        static int countPanel = 0;
        public static void AddPanel(Plan plan)
        {
            int l = 60; //расстояние между блоками
            int lines; //ряд для расположения нового блока
            int columns; //колонка для расположения нового блока
            if (countPanel < 4) //если блоков менее 4
            {
                lines = 1;
                columns = countPanel + 1;
            }
            else
            {
                if (countPanel % 4 == 0) lines = countPanel / 4 + 1;
                else lines = countPanel / 4 + 1;
                columns = countPanel % 4 + 1;
            }

            //расчет расположения блока
            int x = 60 + (columns - 1) * (160 + l);
            int y = 50 + (lines - 1) * (250 + l);

            //создание блока и установка характеристик
            System.Windows.Forms.Panel newPanel = new System.Windows.Forms.Panel();
            newPanel.Size = new Size(160, 250);

            if (countPanel == 0)//установка расположения первого блока
            {
                newPanel.Location = new System.Drawing.Point(60, 50);
            }
            else newPanel.Location = new System.Drawing.Point(x, y); //установка расположения всех последующих блоков
            countPanel++;
            newPanel.BackColor = System.Drawing.Color.White;
            newPanel.BorderStyle = BorderStyle.FixedSingle;

            TabPage plans = Form1.CurrentForm.GetTabPagePlans();

            //добавление названия плана на панель
            Label label = AddLabel(newPanel.Location.X, newPanel.Location.Y, plan);
            newPanel.Controls.Add(label);
            plans.Controls.Add(label);

            Label label1 = AddLabel(newPanel.Location.X, newPanel.Location.Y);
            newPanel.Controls.Add(label1);
            plans.Controls.Add(label1);

            Label labelGoal = AddLabelGoal(newPanel.Location.X, newPanel.Location.Y, plan);
            newPanel.Controls.Add(labelGoal);
            plans.Controls.Add(labelGoal);

            Label labelProgress = AddLabelProgress(newPanel.Location.X, newPanel.Location.Y);
            newPanel.Controls.Add(labelProgress);
            plans.Controls.Add(labelProgress);

            Label labelProgress1 = AddLabelProgress(newPanel.Location.X, newPanel.Location.Y, plan);
            newPanel.Controls.Add(labelProgress1);
            plans.Controls.Add(labelProgress1);

            ProgressBar progressBar = AddProgressBar(x, y, plan);
            newPanel.Controls.Add(progressBar);
            plans.Controls.Add(progressBar);

            Button button = AddButton(x, y, plan);
            newPanel.Controls.Add(button);
            plans.Controls.Add(button);

            //добавление блока на форму

            //tabPage1.Controls.Add(newPanel);
            plans.Controls.Add(newPanel);
        }

        public static Label AddLabel(int x, int y, Plan plan)
        {
            Label label = new Label();

            label.Text = plan.name;

            label.Location = new System.Drawing.Point(x + 10, y + 15);
            label.Size = new Size(140, 40);
            label.ForeColor = System.Drawing.Color.Black;
            label.BackColor = System.Drawing.Color.White;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = new System.Drawing.Font(label.Font.FontFamily, 10, FontStyle.Bold);

            return label;
        }

        public static Label AddLabelGoal(int x, int y, Plan plan)
        {
            Label label = new Label();

            label.Text = (Math.Round(plan.amountWithInflation, 2)).ToString() + " рублей";

            label.Location = new System.Drawing.Point(x + 10, y + 85);
            label.Size = new Size(140, 30);
            label.ForeColor = System.Drawing.Color.Black;
            label.BackColor = System.Drawing.Color.White;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = new System.Drawing.Font(label.Font.FontFamily, 10, FontStyle.Regular);

            return label;
        }

        public static Label AddLabelProgress(int x, int y, Plan plan)
        {
            Label label = new Label();

            label.Text = $"{plan.currentAmount + plan.currentInvestAmount} / {Math.Round(plan.amountWithInflation, 2)}";

            label.Location = new System.Drawing.Point(x + 15, y + 135);
            label.Size = new Size(130, 20);
            label.ForeColor = System.Drawing.Color.Black;
            label.BackColor = System.Drawing.Color.White;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = new System.Drawing.Font(label.Font.FontFamily, 7, FontStyle.Regular);

            return label;
        }

        public static ProgressBar AddProgressBar(int x, int y, Plan plan)
        {
            ProgressBar progressBar = new ProgressBar();

            progressBar.Size = new System.Drawing.Size(130, 20);
            progressBar.Location = new System.Drawing.Point(x + 15, y + 165);

            if (plan.currentAmount + plan.currentInvestAmount >= plan.amountWithInflation)
                progressBar.Value = 100;
            else progressBar.Value = (int)((plan.currentAmount + plan.currentInvestAmount) / plan.amountWithInflation * 100);

            return progressBar;
        }

        public static Button AddButton(int x, int y, Plan plan)
        {
            Button button = new Button();
            button.Text = "Открыть";
            button.Size = new System.Drawing.Size(70, 30);
            button.Location = new System.Drawing.Point(x + 45, y + 200);

            button.Click += (sender, e) => Button_Click_Plan(sender, e, plan);

            return button;
        }

        private static void Button_Click_Plan(object sender, EventArgs e, Plan plan)
        {
                Form2 form2 = new Form2(plan);
                form2.Show();
        }

        public static Label AddLabel(int x, int y)
        {
            Label label = new Label();
            label.Text = "Целевая сумма с учетом инфляции:";

            label.Location = new System.Drawing.Point(x + 10, y + 60);
            label.Size = new Size(140, 30);
            label.ForeColor = System.Drawing.Color.Black;
            label.BackColor = System.Drawing.Color.White;
            label.Font = new System.Drawing.Font(label.Font.FontFamily, 7, FontStyle.Regular);

            return label;
        }


        public static Label AddLabelProgress(int x, int y)
        {
            Label label = new Label();
            label.Text = "Прогресс:";

            label.Location = new System.Drawing.Point(x + 10, y + 120);
            label.Size = new Size(140, 15);
            label.ForeColor = System.Drawing.Color.Black;
            label.BackColor = System.Drawing.Color.White;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = new System.Drawing.Font(label.Font.FontFamily, 7, FontStyle.Bold);

            return label;
        }
    }
}
