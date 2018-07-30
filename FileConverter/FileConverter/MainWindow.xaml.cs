using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FileConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XmlDocument m_xmlDocument = new XmlDocument();

        public MainWindow()
        {
            InitializeComponent();
        }
        private void MenuItem_OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "obj, xml|*.obj;*.xml";
            if (openFile.ShowDialog() == true)
            {
                if (openFile.FileName.EndsWith(".obj"))
                {
                    try
                    {
                        m_xmlDocument = new XmlDocument();
                        m_xmlDocument.AppendChild(m_xmlDocument.CreateProcessingInstruction("xml", "version=\"1.0\"encoding =\"UTF-8\""));
                        XmlElement root = m_xmlDocument.CreateElement("root");
                        m_xmlDocument.AppendChild(root);
                        using (StreamReader sr = new StreamReader(openFile.FileName))
                        {
                            string line;
                            string[] splitLine;
                            bool isVertices = false;
                            bool isNormals = false;
                            bool isUVs = false;
                            bool isParameterSpaceVertices = false;
                            bool isLineElement = false;
                            bool isSmoothShading = false;
                            bool isMtllib = false;
                            XmlNode xmlNode = null;
                            while ((line = sr.ReadLine()) != null)
                            {
                                line = line.Trim();
                                splitLine = line.Split(' ');

                                switch (splitLine[0])
                                {
                                    case "v":
                                        if (!isVertices)
                                        {
                                            xmlNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "Vertices", "");
                                            m_xmlDocument.DocumentElement.AppendChild(xmlNode);
                                            isVertices = true;
                                        }

                                        XmlNode vertexNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "Vertex", "");
                                        xmlNode.AppendChild(vertexNode);

                                        XmlNode xVertexNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "X", "");
                                        xVertexNode.InnerText = splitLine[1];
                                        vertexNode.AppendChild(xVertexNode);

                                        xVertexNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "Y", "");
                                        xVertexNode.InnerText = splitLine[2];
                                        vertexNode.AppendChild(xVertexNode);

                                        xVertexNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "Z", "");
                                        xVertexNode.InnerText = splitLine[3];
                                        vertexNode.AppendChild(xVertexNode);

                                        if (splitLine.Length > 4)
                                        {
                                            xVertexNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "W", "");
                                            xVertexNode.InnerText = splitLine[3];
                                            vertexNode.AppendChild(xVertexNode);
                                        }
                                        continue;

                                    case "vn":
                                        if (!isNormals)
                                        {
                                            xmlNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "Normals", "");
                                            m_xmlDocument.DocumentElement.AppendChild(xmlNode);
                                            isNormals = true;
                                        }

                                        XmlNode normalNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "Normal", "");
                                        xmlNode.AppendChild(normalNode);

                                        XmlNode xNormalNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "X", "");
                                        xNormalNode.InnerText = splitLine[1];
                                        normalNode.AppendChild(xNormalNode);

                                        xNormalNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "Y", "");
                                        xNormalNode.InnerText = splitLine[2];
                                        normalNode.AppendChild(xNormalNode);

                                        xNormalNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "Z", "");
                                        xNormalNode.InnerText = splitLine[3];
                                        normalNode.AppendChild(xNormalNode);
                                        continue;

                                    case "vt":
                                        if (!isUVs)
                                        {
                                            xmlNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "UVs", "");
                                            m_xmlDocument.DocumentElement.AppendChild(xmlNode);
                                            isUVs = true;
                                        }

                                        XmlNode uvNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "UV", "");
                                        xmlNode.AppendChild(uvNode);

                                        XmlNode xUvNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "U", "");
                                        xUvNode.InnerText = splitLine[1];
                                        uvNode.AppendChild(xUvNode);

                                        xUvNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "V", "");
                                        xUvNode.InnerText = splitLine[2];
                                        uvNode.AppendChild(xUvNode);

                                        if (splitLine.Length > 3)
                                        {
                                            xUvNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "W", "");
                                            xUvNode.InnerText = splitLine[3];
                                            uvNode.AppendChild(xUvNode);
                                        }
                                        continue;

                                    case "vp":
                                        if (!isParameterSpaceVertices)
                                        {
                                            xmlNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "VPs", "");
                                            m_xmlDocument.DocumentElement.AppendChild(xmlNode);
                                            isParameterSpaceVertices = true;
                                        }

                                        XmlNode vpNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "VP", "");
                                        xmlNode.AppendChild(vpNode);

                                        XmlNode xVpNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "U", "");
                                        xVpNode.InnerText = splitLine[1];
                                        vpNode.AppendChild(xVpNode);

                                        if (splitLine.Length > 2)
                                        {
                                            xVpNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "V", "");
                                            xVpNode.InnerText = splitLine[2];
                                            vpNode.AppendChild(xVpNode);
                                        }

                                        if (splitLine.Length > 3)
                                        {
                                            xVpNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "W", "");
                                            xVpNode.InnerText = splitLine[3];
                                            vpNode.AppendChild(xVpNode);
                                        }
                                        continue;

                                    case "l":
                                        if (!isLineElement)
                                        {
                                            xmlNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "LineElements", "");
                                            m_xmlDocument.DocumentElement.AppendChild(xmlNode);
                                            isLineElement = true;
                                        }

                                        XmlNode lineNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "LineElement", "");
                                        xmlNode.AppendChild(lineNode);

                                        XmlNode xLineNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "v", "");
                                        xLineNode.InnerText = splitLine[1];
                                        lineNode.AppendChild(xLineNode);

                                        // TODO!
                                        continue;

                                    case "s":
                                        if (!isSmoothShading)
                                        {
                                            xmlNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "SmoothShadings", "");
                                            m_xmlDocument.DocumentElement.AppendChild(xmlNode);
                                            isSmoothShading = true;
                                        }

                                        XmlNode smoothingNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "SmoothShading", "");
                                        xmlNode.AppendChild(smoothingNode);

                                        XmlNode xSmoothingNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "s", "");
                                        xSmoothingNode.InnerText = splitLine[1];
                                        smoothingNode.AppendChild(xSmoothingNode);

                                        // TODO?
                                        continue;

                                    case "mtllib":
                                        if (!isMtllib)
                                        {
                                            xmlNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "mtllibs", "");
                                            m_xmlDocument.DocumentElement.AppendChild(xmlNode);
                                            isMtllib = true;
                                        }

                                        XmlNode mtllibNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "mtllib", "");
                                        xmlNode.AppendChild(mtllibNode);

                                        XmlNode xMtllibNode = m_xmlDocument.CreateNode(XmlNodeType.Element, "path", "");
                                        xMtllibNode.InnerText = splitLine[1];
                                        mtllibNode.AppendChild(xMtllibNode);

                                        // TODO?
                                        continue;

                                    case "usemtl":

                                        continue;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    catch (IndexOutOfRangeException _ex)
                    {
                        MessageBox.Show("Invalid file format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                //    catch (Exception)
                  //  {

                    //    throw;
                    //}
                }
                else if (openFile.FileName.EndsWith(".xml"))
                {

                }
            }
            
        }

        private void MenuItem_SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.CheckPathExists = false;
            
            saveFileDialog.FileName = "lamp.xml";


            //using (FileStream fs = new FileStream("lamp.xml", FileMode.Create))
            //{

            //    using (StreamWriter sw = new StreamWriter(fs))
            //    {
            //        m_xmlDocument.Save(sw);

            //    }
            //}
            //saveFileDialog.OpenFile();
            saveFileDialog.ShowDialog();
            
            
            m_xmlDocument.Save(saveFileDialog.FileName);
        }

        private void MenuItem_SaveFileAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Quit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
