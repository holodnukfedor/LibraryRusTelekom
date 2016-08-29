using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Library
{
    /// <summary>
    /// Interaction logic for XmlWithXsltTransformation.xaml
    /// </summary>
    public partial class XmlWithXsltTransformation : Window
    {
        private Microsoft.Win32.OpenFileDialog openInputXmldlg = new Microsoft.Win32.OpenFileDialog();
        private Microsoft.Win32.OpenFileDialog openXsltdlg = new Microsoft.Win32.OpenFileDialog();
           
        public XmlWithXsltTransformation()
        {
            InitializeComponent();
            openInputXmldlg.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openInputXmldlg.DefaultExt = ".xml";
            openInputXmldlg.Filter = "Файлы XML (*.xml)|*.xml|Все файлы (*.*)|*.*";

            openXsltdlg.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openXsltdlg.DefaultExt = ".xml";
            openXsltdlg.Filter = "Файлы XML (*.xml)|*.xml|Все файлы (*.*)|*.*";

        }

        private void InputXmlBtn_Click(object sender, RoutedEventArgs e)
        {
            if (openInputXmldlg.ShowDialog() == true)
            {
                InputXml.Text = openInputXmldlg.FileName;
            }
        }

        private void XsltSchemeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (openXsltdlg.ShowDialog() == true)
            {
                XsltScheme.Text = openXsltdlg.FileName;
            }
        }

        private void OutputXml_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            dlg.Filter = "Все файлы (*.*)|*.*";
            if (String.IsNullOrEmpty(InputXml.Text))
            {
                MessageBox.Show("Задайте входной xml. ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(XsltScheme.Text))
            {
                MessageBox.Show("Задайте xslt схему. ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    XslTransform xslt = new XslTransform();
                    xslt.Load(openXsltdlg.FileName);
                    XPathDocument xpathdocument = new
                    XPathDocument(openInputXmldlg.FileName);

                    using (TextWriter textWriter = new StreamWriter(dlg.FileName))
                    {
                        XmlTextWriter writer = new XmlTextWriter(textWriter);
                        writer.Formatting = Formatting.Indented;
                        xslt.Transform(xpathdocument, null, writer, null);
                    }
                    MessageBox.Show("Файл сохранен", "Успешное сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка записи в файл. " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
