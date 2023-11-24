
using L_P.Model;
using L_P.Model.Event;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using TagLib;

namespace L_P.ViewModel
{
    public class AddViewModel
    {
        private ApplicationViewModel app;

        public AddViewModel(ApplicationViewModel applicationViewModel)
        {
            app = applicationViewModel;
        }

        private OpenFileDialog openDialog = new OpenFileDialog();
        public RelayCommand SearchMusicCommand
        {
            get
            {
                return (new RelayCommand(obj =>
                {
                    openDialog.Filter = "Музыкальные файлы (.mp3) | *.mp3";
                    openDialog.Title = "Выберите музыкальные файлы....";
                    bool? succses = openDialog.ShowDialog();
                    openDialog.Multiselect = true;
                    if (succses == true)
                    {
                        foreach (string fileName in openDialog.FileNames)
                        {
                            try
                            {
                                TagLib.File file = TagLib.File.Create(fileName);

                                Music music = new Music
                                {
                                    SongName = file.Tag.Title,
                                    SongerName = string.Join(", ", file.Tag.Performers),
                                    Album = file.Tag.Album,
                                    Date = (int)file.Tag.Year,
                                    Durations = TimeSpan.FromSeconds(file.Properties.Duration.TotalSeconds),
                                    MusicFile = new FileStream(fileName, FileMode.Open),
                                };
                                if (file.Tag.Pictures != null && file.Tag.Pictures.Length > 0)
                                {
                                    IPicture picture = file.Tag.Pictures[0];

                                    using (MemoryStream memoryStream = new MemoryStream(picture.Data.Data))
                                    {
                                        BitmapImage bitmapImage = new BitmapImage();
                                        bitmapImage.BeginInit();
                                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                        bitmapImage.StreamSource = memoryStream;
                                        bitmapImage.EndInit();

                                        music.MusicImage = bitmapImage;
                                    }
                                }
                                app.Music.Add(music);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при обработке файла: {ex.Message}");
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Ни один элемент не был добавлен");
                    }
                }));
            }
        }
        public RelayCommand SearchPodcastCommand
        {
            get
            {
                return (new RelayCommand(obj =>
                {
                    openDialog.Filter = "Подкаст файлы (.mp3) | *.mp3";
                    openDialog.Title = "Выберите подкаст файлы....";
                    bool? succses = openDialog.ShowDialog();
                    openDialog.Multiselect = true;
                    if (succses == true)
                    {
                        foreach (string fileName in openDialog.FileNames)
                        {
                            try
                            {
                                TagLib.File file = TagLib.File.Create(fileName);

                                Podcast podcast = new Podcast
                                {
                                    PodcastName = file.Tag.Title,
                                    PodcasterName = string.Join(", ", file.Tag.Performers),
                                    Date = (int)file.Tag.Year,
                                    Duration = TimeSpan.FromSeconds(file.Properties.Duration.TotalSeconds),
                                    PodcastFile = new FileStream(fileName, FileMode.Open)
                                };
                                if (file.Tag.Pictures != null && file.Tag.Pictures.Length > 0)
                                {
                                    IPicture picture = file.Tag.Pictures[0];

                                    using (MemoryStream memoryStream = new MemoryStream(picture.Data.Data))
                                    {
                                        BitmapImage bitmapImage = new BitmapImage();
                                        bitmapImage.BeginInit();
                                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                        bitmapImage.StreamSource = memoryStream;
                                        bitmapImage.EndInit();

                                        podcast.PodcastImage = bitmapImage;
                                    }
                                }
                                app.Podcasts.Add(podcast);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при обработке файла: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ни один элемент не был добавлен");
                    }
                }));
            }
        }
        public RelayCommand SearchAccordsCommand
        {
            get
            {
                return (new RelayCommand(obj =>
                {
                    openDialog.Filter = "Аккорд файлы (.txt) | *.txt";
                    openDialog.Title = "Выберите аккорд файлы....";
                    openDialog.Multiselect = true;

                    bool? success = openDialog.ShowDialog();

                    if (success == true)
                    {
                        foreach (string fileName in openDialog.FileNames)
                        {
                            try
                            {
                                string title = System.IO.File.ReadAllText(fileName);
                                Accords accords = new Accords
                                {
                                    AccordName = Path.GetFileNameWithoutExtension(fileName),
                                    AccordFileText = title
                                };
                                app.Accords.Add(accords);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при обработке файла: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ни один элемент не был добавлен");
                    }
                }));
            }
        }


    }
}
