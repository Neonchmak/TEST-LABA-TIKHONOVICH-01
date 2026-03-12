using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // Добавьте это пространство имен

namespace PhotoProgram
{
    public class PhotoAlbum
    {
        private List<Photo> photos = new List<Photo>();
        private ListView listView;
        private PhotoFormat currentFilter = PhotoFormat.Все; // Текущий фильтр

        public PhotoAlbum(ListView listView)
        {
            this.listView = listView;
            LoadPhotos();
        }

        private void LoadPhotos()
        {
            listView.Items.Clear();

            // Применяем фильтр по формату
            var filteredPhotos = photos;
            if (currentFilter != PhotoFormat.Все)
            {
                string extension = PhotoFormatHelper.GetExtension(currentFilter);
                filteredPhotos = photos.Where(p =>
                    System.IO.Path.GetExtension(p.Path).ToLower() == extension).ToList();
            }

            foreach (var photo in filteredPhotos)
            {
                string format = System.IO.Path.GetExtension(photo.Path).ToUpper();
                listView.Items.Add(new ListViewItem(new[] {
                    photo.Path,
                    photo.Description,
                    photo.DateTaken.ToString("dd.MM.yyyy"),
                    format // Добавляем информацию о формате
                }));
            }

            // Обновляем информацию о количестве
            UpdateStatusInfo();
        }

        private void UpdateStatusInfo()
        {
            int totalCount = photos.Count;
            int filteredCount = listView.Items.Count;

            if (listView.FindForm() != null)
            {
                string status = $"Всего фото: {totalCount}";
                if (currentFilter != PhotoFormat.Все)
                {
                    status += $" | Отображено ({currentFilter}): {filteredCount}";
                }

                // Можно добавить статусную строку, если она есть в форме
                // listView.FindForm().Text = "Управление фотографиями - " + status;
            }
        }

        public void AddPhoto()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory =
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                openFileDialog.Title = "Выберите фото";
                openFileDialog.Filter = PhotoFormatHelper.GetFilter(PhotoFormat.Все);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var photoPath = openFileDialog.FileName;

                    // Открытие формы для ввода описания и даты
                    using (var descForm = new DescriptionForm())
                    {
                        if (descForm.ShowDialog() == DialogResult.OK)
                        {
                            var description = descForm.GetDescription();
                            var parts = description.Split('|');

                            if (parts.Length == 2)
                            {
                                var desc = parts[0];
                                var dateTaken = DateTime.ParseExact(parts[1], "dd.MM.yyyy", null);

                                photos.Add(new Photo(photoPath, desc, dateTaken));
                                LoadPhotos();
                                MessageBox.Show($"Фото добавлено. Формат: {Path.GetExtension(photoPath).ToUpper()}");
                            }
                        }
                    }
                }
            }
        }

        public void RemovePhoto()
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Сначала выберите фото для удаления.");
                return;
            }
            var photoPath = listView.SelectedItems[0].SubItems[0].Text;
            photos.RemoveAll(p => p.Path == photoPath);
            LoadPhotos();
            MessageBox.Show("Фото удалено.");
        }

        public void SortPhotosByDate()
        {
            var sortedPhotos = photos.OrderBy(p => p.DateTaken).ToList();
            photos = new List<Photo>(sortedPhotos);
            LoadPhotos();
            MessageBox.Show("Фото отсортированы по дате.");
        }

        public void SortPhotosByName()
        {
            var sortedPhotos = photos.OrderBy(p => p.Path).ToList();
            photos = new List<Photo>(sortedPhotos);
            LoadPhotos();
            MessageBox.Show("Фото отсортированы по названию.");
        }

        // Новый метод: фильтрация по формату
        public void FilterByFormat(PhotoFormat format)
        {
            currentFilter = format;
            LoadPhotos();

            string message = format == PhotoFormat.Все ?
                "Показаны все фотографии" :
                $"Показаны фотографии формата {format}";
            MessageBox.Show(message);
        }

        // Новый метод: получение статистики по форматам
        public string GetFormatStatistics()
        {
            var stats = new Dictionary<string, int>();

            foreach (var photo in photos)
            {
                string ext = Path.GetExtension(photo.Path).ToUpper();
                if (stats.ContainsKey(ext))
                    stats[ext]++;
                else
                    stats[ext] = 1;
            }

            string result = "Статистика по форматам:\n";
            foreach (var item in stats.OrderBy(x => x.Key))
            {
                result += $"{item.Key}: {item.Value} фото\n";
            }
            result += $"Всего: {photos.Count} фото";

            return result;
        }

        // Новый метод: добавление нескольких фото
        public void AddMultiplePhotos()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory =
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                openFileDialog.Title = "Выберите фото (можно выбрать несколько)";
                openFileDialog.Filter = PhotoFormatHelper.GetFilter(PhotoFormat.Все);
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    int added = 0;
                    int skipped = 0;

                    foreach (var photoPath in openFileDialog.FileNames)
                    {
                        // Проверяем, не добавлено ли уже такое фото
                        if (!photos.Any(p => p.Path == photoPath))
                        {
                            using (var descForm = new DescriptionForm())
                            {
                                if (descForm.ShowDialog() == DialogResult.OK)
                                {
                                    var description = descForm.GetDescription();
                                    var parts = description.Split('|');

                                    if (parts.Length == 2)
                                    {
                                        var desc = parts[0];
                                        var dateTaken = DateTime.ParseExact(parts[1], "dd.MM.yyyy", null);

                                        photos.Add(new Photo(photoPath, desc, dateTaken));
                                        added++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            skipped++;
                        }
                    }

                    LoadPhotos();
                    MessageBox.Show($"Добавлено фото: {added}\nПропущено (уже есть): {skipped}");
                }
            }
        }

        private string GetDescription()
        {
            using (var descriptionForm = new DescriptionForm())
            {
                descriptionForm.ShowDialog();
                return descriptionForm.Description;
            }
        }
    }
}