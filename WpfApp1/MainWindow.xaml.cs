using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Xml;
using System.Xml.Schema;

namespace XMLValidation
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string currentXmlFilePath;
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Climb> climbs;
        public ObservableCollection<Climb> Climbs
        {
            get { return climbs; }
            set
            {
                climbs = value;
                OnPropertyChanged(nameof(Climbs));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Climbs = new ObservableCollection<Climb>();
            DataContext = this; // Убедитесь, что контекст данных установлен на текущий объект MainWindow
        }


        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                currentXmlFilePath = openFileDialog.FileName;
                ValidateXmlFile(currentXmlFilePath);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь можно добавить код для создания нового XML документа
            MessageBox.Show("Функционал 'Создать' пока не реализован.");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Climbs.Count > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        // Создание XML-декларации с указанием кодировки
                        XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                        xmlDoc.AppendChild(xmlDeclaration);

                        // Создание основного узла alpinist_diary
                        XmlElement rootElement = xmlDoc.CreateElement("alpinist_diary");

                        // Добавление атрибутов к основному узлу
                        rootElement.SetAttribute("xmlns", "http://tempuri.org/AlpinistDiarySchema.xsd");
                        rootElement.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                        rootElement.SetAttribute("xsi:schemaLocation", "http://tempuri.org/AlpinistDiarySchema.xsd AlpinistDiarySchema.xsd");

                        xmlDoc.AppendChild(rootElement);

                        foreach (Climb climb in Climbs)
                        {
                            XmlElement climbElement = xmlDoc.CreateElement("climb");

                            XmlElement peakNameElement = xmlDoc.CreateElement("peak_name");
                            peakNameElement.InnerText = climb.PeakName;
                            climbElement.AppendChild(peakNameElement);

                            XmlElement heightElement = xmlDoc.CreateElement("height");
                            heightElement.InnerText = climb.Height.ToString();
                            climbElement.AppendChild(heightElement);

                            XmlElement countryElement = xmlDoc.CreateElement("country");
                            countryElement.InnerText = climb.Country;
                            climbElement.AppendChild(countryElement);

                            XmlElement visitDateElement = xmlDoc.CreateElement("visit_date");
                            visitDateElement.InnerText = climb.VisitDate.ToString("yyyy-MM-dd");
                            climbElement.AppendChild(visitDateElement);

                            XmlElement climbTimeElement = xmlDoc.CreateElement("climb_time");
                            climbTimeElement.InnerText = climb.ClimbTime;
                            climbElement.AppendChild(climbTimeElement);

                            XmlElement difficultyCategoryElement = xmlDoc.CreateElement("difficulty_category");
                            difficultyCategoryElement.InnerText = climb.DifficultyCategory;
                            climbElement.AppendChild(difficultyCategoryElement);

                            rootElement.AppendChild(climbElement);
                        }

                        xmlDoc.Save(filePath);

                        MessageBox.Show("XML файл успешно сохранен.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Нет данных для сохранения.");
            }
        }




        private void DisplayXmlData(string filePath)
        {
            try
            {
                AlpinistDiary diary = XmlParser.ParseXml(filePath);
                Climbs.Clear();
                foreach (var climb in diary.Climbs)
                {
                    Climbs.Add(climb);
                }
                MessageBox.Show("Данные успешно добавлены в таблицу.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }


        private void ValidateXmlFile(string filePath)
        {
            try
            {
                // Создание объекта XmlReaderSettings для настройки валидации
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;

                // Загрузка XSD схемы из файла
                settings.Schemas.Add(null, @"XSD\schema.xsd");

                // Установка обработчика для события валидации
                settings.ValidationEventHandler += ValidationCallback;

                // Загрузка XML документа из файла
                XmlReader reader = XmlReader.Create(filePath, settings);

                // Чтение и валидация XML документа
                while (reader.Read()) { }

                // Закрытие XmlReader
                reader.Close();

                MessageBox.Show("XML документ соответствует XSD схеме.\n");
                // Вызов метода DisplayXmlData после успешной валидации
                DisplayXmlData(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла: {ex.Message}");
            }
        }

        private void ValidationCallback(object sender, ValidationEventArgs e)
        {
            MessageBox.Show($"Ошибка валидации: {e.Message}\n");
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
