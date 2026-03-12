using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoProgram
{
    public partial class DescriptionForm : Form
    {
        public string Description { get; private set; } = "Description here";

        public DescriptionForm()
        {
            InitializeComponent();

            // Настройка формы
            this.Text = "Введите описание и дату";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            CreateControls();
        }

        private void CreateControls()
        {
            // Создание метки для описания
            Label lblDescription = new Label
            {
                Text = "Описание:",
                Location = new Point(20, 20),
                Size = new Size(100, 25)
            };

            // Создание текстового поля для описания
            TextBox txtDescription = new TextBox
            {
                Location = new Point(130, 20),
                Size = new Size(220, 25),
                Text = Description
            };

            // Создание метки для даты
            Label lblDate = new Label
            {
                Text = "Дата (ДД.ММ.ГГГГ):",
                Location = new Point(20, 60),
                Size = new Size(120, 25)
            };

            // Создание текстового поля для даты
            TextBox txtDate = new TextBox
            {
                Location = new Point(150, 60),
                Size = new Size(200, 25),
                Text = DateTime.Now.ToString("dd.MM.yyyy")
            };

            // Создание кнопки OK
            Button btnOK = new Button
            {
                Text = "OK",
                Location = new Point(150, 100),
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };

            // Создание кнопки Cancel
            Button btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(250, 100),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            // Добавление обработчика для кнопки OK
            btnOK.Click += (sender, e) =>
            {
                if (ValidateInput(txtDate.Text))
                {
                    Description = txtDescription.Text + "|" + txtDate.Text;
                }
                else
                {
                    MessageBox.Show("Неверный формат даты. Используйте ДД.ММ.ГГГГ", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                }
            };

            // Добавление элементов на форму
            this.Controls.AddRange(new Control[] {
                lblDescription, txtDescription,
                lblDate, txtDate,
                btnOK, btnCancel
            });
        }

        private bool ValidateInput(string dateText)
        {
            try
            {
                DateTime.ParseExact(dateText, "dd.MM.yyyy", null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetDescription()
        {
            return Description;
        }
    }
}