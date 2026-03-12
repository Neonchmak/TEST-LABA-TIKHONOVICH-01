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
    public partial class PhotoAlbumForm : Form
    {
        private PhotoAlbum photoAlbum;
        private ListView listView;
        private Button addPhotoButton;
        private Button addMultipleButton; // Новая кнопка
        private Button removePhotoButton;
        private Button sortByDateButton;
        private Button sortByNameButton;
        private Button statsButton; // Новая кнопка
        private ComboBox formatFilterCombo; // Новый комбобокс для фильтрации
        private Label formatLabel; // Новая метка

        public PhotoAlbumForm()
        {
            this.Text = "Управление фотографиями";
            this.Width = 750;
            this.Height = 500;
            CreateControls();
            photoAlbum = new PhotoAlbum(listView);
        }

        private void CreateControls()
        {
            // Список фотографий
            listView = new ListView
            {
                Location = new System.Drawing.Point(10, 40),
                Size = new System.Drawing.Size(715, 350),
                View = View.Details,
                FullRowSelect = true
            };
            listView.Columns.Add("Путь", 350);
            listView.Columns.Add("Описание", 200);
            listView.Columns.Add("Дата съёмки", 100);
            listView.Columns.Add("Формат", 60);

            // Метка для фильтра
            formatLabel = new Label
            {
                Text = "Фильтр по формату:",
                Location = new System.Drawing.Point(10, 15),
                Size = new System.Drawing.Size(100, 20)
            };

            // Комбобокс для выбора формата
            formatFilterCombo = new ComboBox
            {
                Location = new System.Drawing.Point(115, 12),
                Size = new System.Drawing.Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Заполняем комбобокс значениями из перечисления
            formatFilterCombo.Items.AddRange(Enum.GetNames(typeof(PhotoFormat)));
            formatFilterCombo.SelectedIndex = 0; // "Все"

            formatFilterCombo.SelectedIndexChanged += (sender, e) =>
            {
                if (photoAlbum != null)
                {
                    PhotoFormat selectedFormat = (PhotoFormat)formatFilterCombo.SelectedIndex;
                    photoAlbum.FilterByFormat(selectedFormat);
                }
            };

            // Кнопка добавления одного фото
            addPhotoButton = new Button
            {
                Location = new System.Drawing.Point(280, 10),
                Text = "Добавить фото",
                Size = new System.Drawing.Size(110, 25)
            };
            addPhotoButton.Click += (sender, e) => photoAlbum.AddPhoto();

            // Кнопка добавления нескольких фото
            addMultipleButton = new Button
            {
                Location = new System.Drawing.Point(400, 10),
                Text = "Добавить несколько",
                Size = new System.Drawing.Size(120, 25)
            };
            addMultipleButton.Click += (sender, e) => photoAlbum.AddMultiplePhotos();

            // Кнопка удаления
            removePhotoButton = new Button
            {
                Location = new System.Drawing.Point(530, 10),
                Text = "Удалить фото",
                Size = new System.Drawing.Size(100, 25)
            };
            removePhotoButton.Click += (sender, e) => photoAlbum.RemovePhoto();

            // Кнопка сортировки по дате
            sortByDateButton = new Button
            {
                Location = new System.Drawing.Point(10, 400),
                Text = "По дате",
                Size = new System.Drawing.Size(80, 25)
            };
            sortByDateButton.Click += (sender, e) => photoAlbum.SortPhotosByDate();

            // Кнопка сортировки по названию
            sortByNameButton = new Button
            {
                Location = new System.Drawing.Point(100, 400),
                Text = "По названию",
                Size = new System.Drawing.Size(90, 25)
            };
            sortByNameButton.Click += (sender, e) => photoAlbum.SortPhotosByName();

            // Кнопка статистики
            statsButton = new Button
            {
                Location = new System.Drawing.Point(200, 400),
                Text = "Статистика",
                Size = new System.Drawing.Size(90, 25)
            };
            statsButton.Click += (sender, e) =>
            {
                string stats = photoAlbum.GetFormatStatistics();
                MessageBox.Show(stats, "Статистика по форматам");
            };

            // Добавляем все элементы на форму
            this.Controls.Add(formatLabel);
            this.Controls.Add(formatFilterCombo);
            this.Controls.Add(listView);
            this.Controls.Add(addPhotoButton);
            this.Controls.Add(addMultipleButton);
            this.Controls.Add(removePhotoButton);
            this.Controls.Add(sortByDateButton);
            this.Controls.Add(sortByNameButton);
            this.Controls.Add(statsButton);
        }
    }
}